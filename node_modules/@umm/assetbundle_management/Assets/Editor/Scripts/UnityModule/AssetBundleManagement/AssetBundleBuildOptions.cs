using UnityEditor;

namespace UnityModule.AssetBundleManagement {

    public static class AssetBundleBuildOptions {

        private const string MENU_ITEM_NAME_KEEP_BUILT = "Project/AssetBundle/Options/Keep Built AssetBundles";

        public static bool HasKeepBuiltAssetBundles() {
            return Menu.GetChecked(MENU_ITEM_NAME_KEEP_BUILT);
        }

        [MenuItem(MENU_ITEM_NAME_KEEP_BUILT)]
        public static void ToggleKeepBuiltAssetBundles() {
            Menu.SetChecked(MENU_ITEM_NAME_KEEP_BUILT, !HasKeepBuiltAssetBundles());
        }

    }

}