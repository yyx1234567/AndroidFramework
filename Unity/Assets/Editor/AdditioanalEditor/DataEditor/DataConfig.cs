
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
  
public class DataConfig
{

    [MenuItem("Data/数据操作窗口")]
    private static void OpenEditorWindow()
    {
        var window = Editor.CreateInstance<DataWindow>();
        window.Show();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }
}
#endif






