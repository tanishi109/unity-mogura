using UnityEditor;

namespace UnityModule.AssetBundleManagement {

    public interface IAssetBundleNameResolver {

        string Resolve(string guid);

    }

    public class DefaultAssetBundleNameResolver : IAssetBundleNameResolver {

        public string Resolve(string guid) {
            return string.Format("{0}{1}", AssetDatabase.GUIDToAssetPath(guid).ToLower(), Constants.ASSET_BUNDLE_EXTENSION);
        }

    }

}