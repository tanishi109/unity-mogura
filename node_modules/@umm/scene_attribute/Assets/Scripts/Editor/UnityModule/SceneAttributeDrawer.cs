using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityModule {

    /// <summary>
    /// [SceneName] や [ScenePath] 用の PropertyDrawer の親クラス
    /// </summary>
    public abstract class SceneAttributeDrawer : PropertyDrawer {

        /// <summary>
        /// Unity lifecycle: OnGUI
        /// </summary>
        /// <param name="position">描画座標</param>
        /// <param name="property">対象プロパティ</param>
        /// <param name="label">ラベル</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUIContent[] displayOptions = this.CreateDisplayOptions();

            int selectedIndex = 0;
            for (int i = 0; i < displayOptions.Length; i++) {
                if (displayOptions[i].text != property.stringValue) {
                    continue;
                }
                selectedIndex = i;
                break;
            }
            selectedIndex = EditorGUI.Popup(
                position,
                label,
                selectedIndex,
                displayOptions
            );
            if (displayOptions.Length <= selectedIndex) {
                return;
            }
            property.stringValue = displayOptions[selectedIndex].text;
        }

        protected abstract GUIContent[] CreateDisplayOptions();

    }

    /// <summary>
    /// [SceneName] 用の PropertyDrawer
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneNameAttribute))]
    public class SceneNameAttributeDrawer : SceneAttributeDrawer {

        protected override GUIContent[] CreateDisplayOptions() {
            int count = SceneManager.sceneCountInBuildSettings;
            GUIContent[] displayedOptions = new GUIContent[count];
            for (int i = 0; i < count; i++) {
                displayedOptions[i] = new GUIContent(Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path));
            }
            return displayedOptions;
        }

    }

    /// <summary>
    /// [ScenePath] 用の PropertyDrawer
    /// </summary>
    [CustomPropertyDrawer(typeof(ScenePathAttribute))]
    public class ScenePathAttributeDrawer : SceneAttributeDrawer {

        protected override GUIContent[] CreateDisplayOptions() {
            int count = SceneManager.sceneCountInBuildSettings;
            GUIContent[] displayedOptions = new GUIContent[count];
            for (int i = 0; i < count; i++) {
                displayedOptions[i] = new GUIContent(EditorBuildSettings.scenes[i].path);
            }
            return displayedOptions;
        }

    }

}