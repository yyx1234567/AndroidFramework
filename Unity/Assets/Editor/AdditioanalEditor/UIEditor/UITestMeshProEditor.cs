using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace TMPro.EditorUtilities
{

    [CustomEditor(typeof(UITextMeshPro)), CanEditMultipleObjects]
    public class UITestMeshProEditor : TMP_EditorPanelUI
    {

        public SerializedObject tMP_Font;
        public SerializedProperty tMP_Font_property;
        protected override void OnEnable()
        {
            base.OnEnable();
            UITextMeshPro uitestmeshpro = (UITextMeshPro)target;
            tMP_Font = new SerializedObject(uitestmeshpro);
            tMP_Font_property= tMP_Font.FindProperty("fontAsset");
        }

        public override void OnInspectorGUI()
        {
             EditorGUILayout.PropertyField(tMP_Font_property);
             tMP_Font.ApplyModifiedProperties();
             base.OnInspectorGUI();
        }
    }
}