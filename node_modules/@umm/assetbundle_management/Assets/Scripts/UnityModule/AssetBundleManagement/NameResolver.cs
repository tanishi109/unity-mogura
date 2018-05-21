using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
// ReSharper disable UnusedMember.Global

namespace UnityModule.AssetBundleManagement {

    public interface INameResolver {

        /// <summary>
        /// AssetBundle 名を解決する
        /// </summary>
        /// <param name="name">Asset 名</param>
        /// <param name="includeAssetBundleExtension">AssetBundle の拡張子 (.unity3d) を含めるかどうか</param>
        /// <typeparam name="T">取得対象の型</typeparam>
        /// <returns>AssetBundle 名</returns>
        /// <remarks>Scene のみ SceneObject という独自のクラスを型引数に渡す</remarks>
        /// <remarks>元 Asset が `Images/Sprites/Foo.png` の場合、 `Resolve&lt;Texture&gt;("Foo")` とする。</remarks>
        /// <remarks>元 Asset が `Scenes/Hoge/Fuga/Piyo.unity` の場合、 `Resolve&lt;SceneObject&gt;("Hoge/Fuga/Piyo")` とする。</remarks>
        /// <remarks>最終的に全て小文字に変換される</remarks>
        string Resolve<T>(string name, bool includeAssetBundleExtension = true) where T : Object;

    }

    public class PathFormatNameResolver : INameResolver {

        private static readonly Dictionary<Type, string> PATH_FORMAT_MAP = new Dictionary<Type, string>();

        private static readonly Dictionary<Type, string> DIRECTORY_NAME_MAP = new Dictionary<Type, string>() {
            { typeof(SceneObject)     , "Scenes/" },
            { typeof(Texture)         , "Images/Sprites/" },
            { typeof(AudioClip)       , "Sounds/" },
            { typeof(ScriptableObject), "Entities/" }, // CAFU 由来
        };

        private static readonly Dictionary<Type, string> EXTENSION_MAP = new Dictionary<Type, string>() {
            { typeof(SceneObject)     , ".unity" },
            { typeof(Texture)         , ".png" },
            { typeof(AudioClip)       , ".mp3" },
            { typeof(ScriptableObject), ".asset" },
        };

        private static string defaultPathFormat = "assets/{0}{1}{2}{3}";

        private static string DefaultPathFormat {
            get {
                return defaultPathFormat;
            }
            set {
                defaultPathFormat = value;
            }
        }

        public string Resolve<T>(string name, bool includeAssetBundleExtension = true) where T : Object {
            return string.Format(
                PATH_FORMAT_MAP.ContainsKey(typeof(T)) ? PATH_FORMAT_MAP[typeof(T)] : DefaultPathFormat,
                DIRECTORY_NAME_MAP.ContainsKey(typeof(T)) ? DIRECTORY_NAME_MAP[typeof(T)].ToLower() : string.Empty,
                name.ToLower(),
                EXTENSION_MAP.ContainsKey(typeof(T)) ? EXTENSION_MAP[typeof(T)].ToLower() : string.Empty,
                includeAssetBundleExtension ? Constants.ASSET_BUNDLE_EXTENSION : string.Empty
            );
        }

        /// <summary>
        /// パスフォーマットを設定
        /// </summary>
        /// <param name="pathFormat">パスフォーマット</param>
        /// <typeparam name="T">対象の型</typeparam>
        /// <remarks>0: ベースディレクトリ名, 1: アセット名, 2: アセット拡張子, 3: AssetBundle 拡張子</remarks>
        public static void SetPathFormat<T>(string pathFormat) {
            PATH_FORMAT_MAP[typeof(T)] = pathFormat;
        }

        public static void SetDirectoryName<T>(string directoryName) {
            DIRECTORY_NAME_MAP[typeof(T)] = directoryName;
        }

        public static void SetExtension<T>(string extension) {
            EXTENSION_MAP[typeof(T)] = extension;
        }

        // ReSharper disable once ParameterHidesMember
        public static void SetDefaultPathFormat(string defaultPathFormat) {
            DefaultPathFormat = defaultPathFormat;
        }

    }

    public static class NameResolverManager {

        private static INameResolver DefaultNameResolver { get; set; }

        private static Dictionary<Type, INameResolver> typeBasedNameResolverMap;

        private static Dictionary<Type, INameResolver> TypeBasedNameResolverMap {
            get {
                return typeBasedNameResolverMap ?? (typeBasedNameResolverMap = new Dictionary<Type, INameResolver>());
            }
        }

        public static INameResolver GetNameResolver<T>() where T : Object {
            if (!TypeBasedNameResolverMap.ContainsKey(typeof(T))) {
                TypeBasedNameResolverMap[typeof(T)] = DefaultNameResolver ?? new PathFormatNameResolver();
            }
            return TypeBasedNameResolverMap[typeof(T)];
        }

        public static void RegisterTypeBasedNameResolver<T>(INameResolver nameResolver) {
            TypeBasedNameResolverMap[typeof(T)] = nameResolver;
        }

        public static void RegisterDefaultNameResolver(INameResolver nameResolver) {
            DefaultNameResolver = nameResolver;
        }

    }

}