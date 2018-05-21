using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityModule.AssetBundleManagement {

    public static class AssetBundleNameHandler {

        private const string MENU_ITEM_NAME_APPLY = "Project/AssetBundle/Name/Apply";

        private const string MENU_ITEM_NAME_STRIP = "Project/AssetBundle/Name/Strip";

        private const string MENU_ITEM_NAME_STRIP_ALL = "Project/AssetBundle/Name/Strip All";

        private const string MENU_ITEM_NAME_REMOVE_UNUSED = "Project/AssetBundle/Name/Remove Unused";

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// Resolver for AssetBundle name
        /// </summary>
        /// <remarks>上書きする場合は、任意の Editor スクリプトの static コンストラクタなどで直接代入してください。</remarks>
        public static IAssetBundleNameResolver AssetBundleNameResolver { get; set; }

        [MenuItem(MENU_ITEM_NAME_APPLY)]
        public static void ApplyAssetBundleName() {
            // Project View 上で Asset を一つ以上選択していない場合はエラー
            if (!Selection.objects.Any(AssetDatabase.IsMainAsset)) {
                Debug.LogError("Please select any object(s) in Project View.");
                return;
            }
            if (AssetBundleNameResolver == default(IAssetBundleNameResolver)) {
                AssetBundleNameResolver = new DefaultAssetBundleNameResolver();
            }
            GetTargetAssetGUIDs().ToList().ForEach(ApplyAssetBundleName);
        }

        [MenuItem(MENU_ITEM_NAME_STRIP)]
        public static void StripAssetBundleName() {
            // Project View 上で Asset を一つ以上選択していない場合はエラー
            if (!Selection.objects.Any(AssetDatabase.IsMainAsset)) {
                Debug.LogError("Please select any object(s) in Project View.");
                return;
            }
            GetTargetAssetGUIDs().ToList().ForEach(StripAssetBundleName);
        }

        [MenuItem(MENU_ITEM_NAME_STRIP_ALL)]
        public static void StripAllAssetBundleName() {
            AssetDatabase.GetAllAssetBundleNames().ToList().ForEach(x => AssetDatabase.RemoveAssetBundleName(x, true));
        }

        [MenuItem(MENU_ITEM_NAME_REMOVE_UNUSED)]
        public static void RemoveUnusedAssetBundleName() {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        private static void ApplyAssetBundleName(string guid) {
            AssetImporter assetImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
            assetImporter.assetBundleName = AssetBundleNameResolver.Resolve(guid);
        }

        private static void StripAssetBundleName(string guid) {
            AssetImporter assetImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
            assetImporter.assetBundleName = string.Empty;
        }

        private static IEnumerable<string> GetTargetAssetGUIDs() {
            IEnumerable<string> assetsInDirectories = Selection
                .objects
                // Hierarchy 上のオブジェクトは排除
                .Where(AssetDatabase.IsMainAsset)
                // ディレクトリのみで絞り込み
                .Where(x => AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)))
                // 配下の全 Asset の GUID を取得
                .SelectMany(x => AssetDatabase.FindAssets(string.Empty, new[] { AssetDatabase.GetAssetPath(x) }))
                // ディレクトリを除外
                .Where(x => !AssetDatabase.IsValidFolder(AssetDatabase.GUIDToAssetPath(x)));
            IEnumerable<string> assetsInSelection = Selection
                .objects
                // Hierarchy 上のオブジェクトは排除
                .Where(AssetDatabase.IsMainAsset)
                // ディレクトリ以外で絞り込み
                .Where(x => !AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(x)))
                // GUID に変換
                .Select(x => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(x)));
            return assetsInDirectories
                .Concat(assetsInSelection)
                .Distinct();
        }

    }

}