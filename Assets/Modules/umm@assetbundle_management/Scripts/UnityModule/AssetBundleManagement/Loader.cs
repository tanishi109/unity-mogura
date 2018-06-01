using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityModule.ContextManagement;
using Object = UnityEngine.Object;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBePrivate.Global

namespace UnityModule.AssetBundleManagement {

    /// <inheritdoc />
    /// <summary>
    /// Scene を表現
    /// </summary>
    /// <remarks>Loader.LoadAssetAsObservable&lt;T&gt;() は型引数の制約として UnityEngine.Object を採るため、自前 Object として ScriptableObject を継承したクラスを作る必要がある</remarks>
    /// <remarks>直接 UnityEngine.Object を継承してしまうと new する手段がないため、ScriptableObject で代用している</remarks>
    public sealed class SceneObject : ScriptableObject {

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Scene Scene { get; set; }

    }

    public class Loader : IDisposable {

        private const double TIMEOUT_SECONDS = 30.0;

        private const int RETRY_COUNT = 3;

        private const int MAXIMUM_PARALLEL_DOWNLOAD_COUNT = 1;

        private const string LOCAL_ASSETBUNDLE_DIRECTORY = "AssetBundles";

        private const string LOCAL_SINGLE_MANIFEST_DIRECTORY = "SingleManifests";

        private static Dictionary<string, Loader> instanceMap;

        private static Dictionary<string, Loader> InstanceMap {
            get {
                return instanceMap ?? (instanceMap = new Dictionary<string, Loader>());
            }
        }

        public static Loader GetInstance<TProjectContext>(TProjectContext projectContext)
            where TProjectContext : IDownloadableProjectContext, IProjectContext {
            return GetInstance(projectContext.Name, projectContext.AssetBundleURLResolverSingleManifest, projectContext.AssetBundleURLResolver);
        }

        public static Loader GetInstance<TEnum, TURLResolverSingleManifest, TURLResolverNormal>(TEnum contextEnum, TURLResolverSingleManifest urlResolverSingleManifest, TURLResolverNormal urlResolverNormal)
            where TEnum : struct
            where TURLResolverSingleManifest : IURLResolver
            where TURLResolverNormal : IURLResolver {
            return GetInstance(contextEnum.ToString(), urlResolverSingleManifest, urlResolverNormal);
        }

        public static Loader GetInstance<TURLResolverSingleManifest, TURLResolverNormal>(string contextName, TURLResolverSingleManifest urlResolverSingleManifest, TURLResolverNormal urlResolverNormal)
            where TURLResolverSingleManifest : IURLResolver
            where TURLResolverNormal : IURLResolver {
            if (!InstanceMap.ContainsKey(contextName) || InstanceMap[contextName] == default(Loader)) {
                if (urlResolverSingleManifest == null) {
                    throw new ArgumentException("Arguments 'urlResolverSingleManifest' cannot be null.");
                }
                if (urlResolverNormal == null) {
                    throw new ArgumentException("Arguments 'urlResolverNormal' cannot be null.");
                }
                InstanceMap[contextName] = new Loader() {
                    URLResolverSingleManifest = urlResolverSingleManifest,
                    URLResolverNormal = urlResolverNormal,
                };
            }
            return InstanceMap[contextName];
        }

        public static void DestroyInstance<TEnum>(TEnum contextEnum) where TEnum : struct {
            DestroyInstance(contextEnum.ToString());
        }

        public static void DestroyInstance(string contextName) {
            if (InstanceMap.ContainsKey(contextName)) {
                InstanceMap[contextName].Dispose();
                InstanceMap.Remove(contextName);
            }
        }

        public static void DestroyAllInstance() {
            InstanceMap.ToList().ForEach(pair => DestroyInstance(pair.Key));
        }

        // GetInstance() メソッド経由での呼び出しを強制する
        private Loader() {
        }

        public IURLResolver URLResolverSingleManifest { get; private set; }

        public IURLResolver URLResolverNormal { get; private set; }

        private AssetBundleManifest SingleManifest { get; set; }

        private readonly Dictionary<string, IProgress<float>> progressMap = new Dictionary<string, IProgress<float>>();

        private Dictionary<string, IProgress<float>> ProgressMap {
            get {
                return this.progressMap;
            }
        }

        private readonly Dictionary<string, float> progressedValueMap = new Dictionary<string, float>();

        private Dictionary<string, float> ProgressedValueMap {
            get {
                return this.progressedValueMap;
            }
        }

        private readonly ReactiveProperty<float> progressSummary = new ReactiveProperty<float>(0.0f);

        private ReactiveProperty<float> ProgressSummary {
            get {
                return this.progressSummary;
            }
        }

        private readonly ReactiveProperty<int> parallelDownloadCount = new ReactiveProperty<int>(0);

        private ReactiveProperty<int> ParallelDownloadCount {
            get {
                return this.parallelDownloadCount;
            }
        }

        private readonly Dictionary<string, AssetBundle> loadedAssetBundleMap = new Dictionary<string, AssetBundle>();

        private Dictionary<string, AssetBundle> LoadedAssetBundleMap {
            get {
                return this.loadedAssetBundleMap;
            }
        }

        private int Count { get; set; }

        public AssetBundleManifest GetSingleManifest() {
            return this.SingleManifest;
        }

        public IObservable<AssetBundleManifest> LoadSingleManifestAsObservable() {
            return LoadSingleManifest(this.URLResolverSingleManifest)
                .Select(
                    (singleManifest) => {
                        // .First() が強引だが、こうする以外に手が思いつかず…。
                        this.SingleManifest = singleManifest.LoadAsset<AssetBundleManifest>(singleManifest.GetAllAssetNames().First());
                        this.URLResolverNormal.SetSingleManifest(this.SingleManifest);
                        this.Count = this.URLResolverNormal.GetSingleManifest().GetAllAssetBundles().Length;
                        singleManifest.Unload(false);
                        return this.SingleManifest;
                    }
                );
        }

        public IObservable<Unit> DownloadAllAsObservable() {
            return this
                // SingleManfiest を読み込む
                .LoadSingleManifestAsObservable()
                .SelectMany(
                    _ => this
                        .SingleManifest
                        // 全ての AssetBundle 名を取得
                        .GetAllAssetBundles()
                        .Select(
                            assetBundleName => Observable
                                // 並列処理待ち
                                .FromCoroutine(this.WaitParallelDownload)
                                // 並列カウントをインクリメント
                                .Do(__ => this.ParallelDownloadCount.Value++)
                                // UnityWebRequest に変換
                                .SelectMany(
                                    __ => {
                                        if (this.LoadedAssetBundleMap.ContainsKey(assetBundleName)) {
                                            return Observable.Return(this.LoadedAssetBundleMap[assetBundleName]);
                                        }
                                        return ObservableUnityWebRequest
                                            .GetAssetBundle(this.URLResolverNormal.Resolve(assetBundleName), this.SingleManifest.GetAssetBundleHash(assetBundleName), 0, null, this.GetProgress(assetBundleName))
                                            // 読み込み完了時に読み込み済マップに入れておく
                                            //   AssetBundle を開きっぱなしにしておくコストは殆ど無いとのことなので、Unload は原則行わない
                                            //   See also: http://tsubakit1.hateblo.jp/entry/2016/08/23/233604 の「SceneをAssetBundleに含める方法」セクションの最後
                                            .Do(assetBundle => this.LoadedAssetBundleMap[assetBundleName] = assetBundle)
                                            .Timeout(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
                                    })
                                // OnError にせよ OnCompleted にせよ「完了」したら並列ダウンロード数をデクリメント
                                .Finally(() => this.ParallelDownloadCount.Value--)
                        )
                        .WhenAll()
                )
                .AsUnitObservable();
        }

        public IObservable<T> LoadAssetAsObservable<T>(string name) where T : Object {
            return this.DownloadAllAsObservable()
                .SelectMany(_ => this.LoadWithDependenciesAsObservable(NameResolverManager.GetNameResolver<T>().Resolve<T>(name)))
                .Select(assetBundle => LoadAssetFromAssetBundle<T>(assetBundle, NameResolverManager.GetNameResolver<T>().Resolve<T>(name, false)));
        }

        public IObservable<float> OnChangeProgressAsObservable() {
            return this.ProgressSummary.Select(x => x / this.Count).AsObservable();
        }

        public static void ClearCachedSingleManifest() {
            if (HasSingleManifest()) {
                File.Delete(CreateLocalSingleManifestPath());
            }
        }

        public void Dispose() {
            this.LoadedAssetBundleMap
                .ToList()
                .ForEach(
                    pair => {
                        pair.Value.Unload(true);
                    }
                );
        }

        private IObservable<AssetBundle> LoadWithDependenciesAsObservable(string assetBundleName) {
            if (!this.SingleManifest.GetDirectDependencies(assetBundleName).Any()) {
                return this.LoadAsObservable(assetBundleName);
            }
            return this
                .SingleManifest
                .GetDirectDependencies(assetBundleName)
                // 再帰的に依存 AssetBundle を読み込む
                .Select(this.LoadWithDependenciesAsObservable)
                .WhenAll()
                .SelectMany(_ => this.LoadAsObservable(assetBundleName));
        }

        private IObservable<AssetBundle> LoadAsObservable(string assetBundleName) {
            if (this.LoadedAssetBundleMap.ContainsKey(assetBundleName)) {
                return Observable.Return(this.LoadedAssetBundleMap[assetBundleName]);
            }
            return ObservableUnityWebRequest
                .GetAssetBundle(this.URLResolverNormal.Resolve(assetBundleName), 0)
                .Do(assetBundle => this.LoadedAssetBundleMap[assetBundleName] = assetBundle)
                .Timeout(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
        }

        private static T LoadAssetFromAssetBundle<T>(AssetBundle assetBundle, string name) where T : Object {
            // StreamedSceneAssetBundle の場合 LoadAllAssets などは無効
            //   恐らく UnityWebRequest.GetAssetBundle が完了した段階で内部的な Scene 一覧に追加されるモノと思われる
            //   See also: http://tsubakit1.hateblo.jp/entry/2016/08/23/233604 の「StreamedSceneAssetBundleの判定」セクション
            if (assetBundle.isStreamedSceneAssetBundle) {
                // Scene を表現する UnityEngine.Object を継承したクラスは存在しないため、 ScriptableObject を継承した SceneObject を作る
                SceneObject sceneObject = ScriptableObject.CreateInstance<SceneObject>();
                sceneObject.Scene = SceneManager.GetSceneByPath(assetBundle.GetAllScenePaths().FirstOrDefault(x => x.ToLower() == name));
                return sceneObject as T;
            }
            return assetBundle.LoadAsset<T>(name);
        }

        private static string CreateLocalSingleManifestPath() {
            return Path.Combine(
                Application.persistentDataPath,
                LOCAL_ASSETBUNDLE_DIRECTORY,
                ContextManager.CurrentProject.Name,
                LOCAL_SINGLE_MANIFEST_DIRECTORY,
                ContextManager.CurrentProject.As<IDownloadableProjectContext>().AssetBundleSingleManifestVersion.ToString()
            );
        }

        private static bool HasSingleManifest() {
            return File.Exists(CreateLocalSingleManifestPath());
        }

        private static void SaveSingleManifest(byte[] data) {
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(CreateLocalSingleManifestPath()))) {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(CreateLocalSingleManifestPath()));
            }
            File.WriteAllBytes(CreateLocalSingleManifestPath(), data);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(CreateLocalSingleManifestPath());
#endif
        }

        private static IObservable<AssetBundle> LoadSingleManifest(IURLResolver urlResolverSingleManifest) {
            Func<IObservable<AssetBundle>> createStream = () => AssetBundle.LoadFromFileAsync(CreateLocalSingleManifestPath()).AsAsyncOperationObservable().Select(assetBundleCreateRequest => assetBundleCreateRequest.assetBundle);
            if (!HasSingleManifest()) {
                return ObservableUnityWebRequest
                    .GetData(urlResolverSingleManifest.Resolve())
                    .Timeout(TimeSpan.FromSeconds(TIMEOUT_SECONDS))
                    .Retry(RETRY_COUNT)
                    .Do(SaveSingleManifest)
                    .SelectMany(_ => createStream());
            }
            return createStream();
        }

        private IProgress<float> GetProgress(string assetBundleName) {
            if (!this.ProgressMap.ContainsKey(assetBundleName)) {
                this.ProgressMap[assetBundleName] = new Progress<float>(progress => {
                    this.ProgressedValueMap[assetBundleName] = progress;
                    this.ProgressSummary.Value = this.ProgressedValueMap.Values.Sum();
                });
            }
            return this.ProgressMap[assetBundleName];
        }

        private IEnumerator WaitParallelDownload() {
            // 最大同時ダウンロード数を上回っている場合は待つ
            while (this.ParallelDownloadCount.Value >= MAXIMUM_PARALLEL_DOWNLOAD_COUNT) {
                yield return null;
            }
        }

    }

}