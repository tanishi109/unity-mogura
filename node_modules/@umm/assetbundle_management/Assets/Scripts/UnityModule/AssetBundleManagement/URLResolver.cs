using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityModule.ContextManagement;
// ReSharper disable ConvertToAutoProperty
// ReSharper disable UseStringInterpolation
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedMember.Global

namespace UnityModule.AssetBundleManagement {

    public interface IURLResolver {

        string Resolve();

        string Resolve(string assetBundleName);

        AssetBundleManifest GetSingleManifest();

        void SetSingleManifest(AssetBundleManifest assetBundleManifest);

    }

    public abstract class URLResolverBase : IURLResolver {

        protected const string DEFAULT_PATH_PREFIX = "AssetBundles";

        private static readonly Dictionary<RuntimePlatform, string> PLATFORM_NAME_MAP = new Dictionary<RuntimePlatform, string>() {
            { RuntimePlatform.IPhonePlayer , "iOS" },
            { RuntimePlatform.Android      , "Android" },
            { RuntimePlatform.LinuxEditor  , "Standalone" },
            { RuntimePlatform.OSXEditor    , "Standalone" },
            { RuntimePlatform.WindowsEditor, "Standalone" },
        };

#if UNITY_EDITOR
        private static readonly Dictionary<UnityEditor.BuildTarget, string> PLATFORM_NAME_MAP_FOR_EDITOR = new Dictionary<UnityEditor.BuildTarget, string>() {
            { UnityEditor.BuildTarget.iOS    , "iOS" },
            { UnityEditor.BuildTarget.Android, "Android" },
        };
#endif

        private Func<string> protocolResolver;

        protected virtual Func<string> ProtocolResolver {
            get {
                return this.protocolResolver ?? (this.protocolResolver = () => "");
            }
            set {
                this.protocolResolver = value;
            }
        }

        private Func<string> hostnameResolver;

        protected virtual Func<string> HostnameResolver {
            get {
                return this.hostnameResolver ?? (this.hostnameResolver = () => "");
            }
            set {
                this.hostnameResolver = value;
            }
        }

        private Func<string, string> pathResolver;

        protected virtual Func<string, string> PathResolver {
            get {
                return this.pathResolver ?? (this.pathResolver = (assetBundleName) => "");
            }
            set {
                this.pathResolver = value;
            }
        }

        private bool appendPathPrefix = true;

        protected virtual bool AppendPathPrefix {
            get {
                return this.appendPathPrefix;
            }
            set {
                this.appendPathPrefix = value;
            }
        }

        private AssetBundleManifest SingleManifest { get; set; }

        public string Resolve() {
            return this.Resolve(null);
        }

        public string Resolve(string assetBundleName) {
            return new UriBuilder {
                Scheme = this.ProtocolResolver(),
                Host = this.HostnameResolver(),
                Path = this.PathResolver(assetBundleName),
            }.ToString();
        }

        public AssetBundleManifest GetSingleManifest() {
            return this.SingleManifest;
        }

        public void SetSingleManifest(AssetBundleManifest assetBundleManifest) {
            this.SingleManifest = assetBundleManifest;
        }

        protected virtual string DefaultPathResolver(string assetBundleName) {
            return string.IsNullOrEmpty(assetBundleName) ? this.CreateSingleManifestPath() : this.CreatePath(assetBundleName);
        }

        protected virtual string CreateSingleManifestPath() {
            return Path.Combine(this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty, GetPlatformPathName(), GetPlatformPathName());
        }

        protected virtual string CreatePath(string assetBundleName) {
            return Path.Combine(this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty, GetPlatformPathName(), assetBundleName);
        }

        protected static string GetPlatformPathName() {
#if UNITY_EDITOR
            // PostprocessBuildAssetBundle などで iOS/Android 向けの URL を作成するなどの処理が必要になるため、現在向いている Platform を正として処理する
            if (PLATFORM_NAME_MAP_FOR_EDITOR.ContainsKey(UnityEditor.EditorUserBuildSettings.activeBuildTarget)) {
                return PLATFORM_NAME_MAP_FOR_EDITOR[UnityEditor.EditorUserBuildSettings.activeBuildTarget];
            }
#endif
            return PLATFORM_NAME_MAP[Application.platform];
        }

    }

    public sealed class EditorURLResolver : URLResolverBase {

        public EditorURLResolver() {
            // file プロトコルの利用は推奨されていないが、エディタ実行という前提と、インタフェース統一のために妥協している。
            //   本来は AssetBundle.LoadFromFile() の利用が望ましいもよう。
            //   See also: https://unity3d.com/jp/learn/tutorials/topics/best-practices/assetbundle-fundamentals
            this.ProtocolResolver = () => "file";
            this.PathResolver = this.DefaultPathResolver;
        }

        protected override string DefaultPathResolver(string assetBundleName) {
            return Path.Combine(Application.dataPath, base.DefaultPathResolver(assetBundleName));
        }

    }

    public sealed class AmazonS3URLResolver : URLResolverBase {

        private const int HASH_SUBSTRING_DIGIT = 2;

        private const string SINGLE_MANIFEST_DIRECTORY_NAME = "SingleManifests";

        private string Region { get; set; }

        private string BucketName { get; set; }

        public AmazonS3URLResolver(string region, string bucketName) {
            this.Region = region;
            this.BucketName = bucketName;
            this.ProtocolResolver = () => "https";
            this.HostnameResolver = () => string.Format("s3-{0}.amazonaws.com", this.Region);
            this.PathResolver = this.DefaultPathResolver;
            this.AppendPathPrefix = true;
        }

        protected override string CreateSingleManifestPath() {
            return Path.Combine(
                this.BucketName,
                this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty,
                ContextManager.CurrentProject.Name,
                GetPlatformPathName(),
                SINGLE_MANIFEST_DIRECTORY_NAME,
                ContextManager.CurrentProject.As<IDownloadableProjectContext>().AssetBundleSingleManifestVersion.ToString()
            );
        }

        protected override string CreatePath(string assetBundleName) {
            string hashString = this.GetSingleManifest().GetAssetBundleHash(assetBundleName).ToString();
            return Path.Combine(
                this.BucketName,
                this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty,
                ContextManager.CurrentProject.Name,
                GetPlatformPathName(),
                hashString.Substring(0, HASH_SUBSTRING_DIGIT),
                hashString
            );
        }

    }

    public sealed class AmazonCloudFrontURLResolver : URLResolverBase {

        private const int HASH_SUBSTRING_DIGIT = 2;

        private const string SINGLE_MANIFEST_DIRECTORY_NAME = "SingleManifests";

        public AmazonCloudFrontURLResolver(string domainNamePrefix) {
            this.ProtocolResolver = () => "https";
            this.HostnameResolver = () => string.Format("{0}.cloudfront.net", domainNamePrefix);
            this.PathResolver = this.DefaultPathResolver;
            this.AppendPathPrefix = true;
        }

        protected override string CreateSingleManifestPath() {
            return Path.Combine(
                this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty,
                ContextManager.CurrentProject.Name,
                GetPlatformPathName(),
                SINGLE_MANIFEST_DIRECTORY_NAME,
                ContextManager.CurrentProject.As<IDownloadableProjectContext>().AssetBundleSingleManifestVersion.ToString()
            );
        }

        protected override string CreatePath(string assetBundleName) {
            string hashString = this.GetSingleManifest().GetAssetBundleHash(assetBundleName).ToString();
            return Path.Combine(
                this.AppendPathPrefix ? DEFAULT_PATH_PREFIX : string.Empty,
                ContextManager.CurrentProject.Name,
                GetPlatformPathName(),
                hashString.Substring(0, HASH_SUBSTRING_DIGIT),
                hashString
            );
        }

    }

    internal static class Path {

        public static string Combine(params string[] arguments) {
            return arguments.Aggregate(string.Empty, System.IO.Path.Combine);
        }

    }

}