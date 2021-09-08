using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class CreateLuaAuto
{
    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        //EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;//hierarchy面板绘制GUI的委托，可以自己添加gui委托
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)//Unity hierarchy绘制的时候传递的2个参数
    {
        if (Event.current != null && Event.current.button == 1 && Event.current.type <= EventType.MouseUp) //右键，点击向上
        {
            Vector2 mousePosition = Event.current.mousePosition;
            EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Assets/", null);//在鼠标的位置弹出菜单，菜单的路径
         }
    }


    [MenuItem("Assets/Create/创建ET脚本/Component", false, 70)]
    public static void CreateComponentCS()
    {
         ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
           ScriptableObject.CreateInstance<CreateEventCSScriptAsset>(),
           GetSelectPathOrFallback() + "/NewScript.cs", null,
           "Assets/Editor/AdditioanalEditor/CreateScriptEditor/ETComponentExample.cs");
    }

    [MenuItem("Assets/Create/创建ET脚本/Event", false, 70)]
    public static void CreateEventCS()
    {
         ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
           ScriptableObject.CreateInstance<CreateEventCSScriptAsset>(),
           GetSelectPathOrFallback() + "/NewScript.cs", null,
           "Assets/Editor/AdditioanalEditor/CreateScriptEditor/ETEventExample.cs");
    }
     public static string GetSelectPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}

 class CreateEventCSScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
         UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(obj); 
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
         string fullPath = Path.GetFullPath(pathName);
         StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
 
         text = Regex.Replace(text, "ScriptNameComponent", fileNameWithoutExtension);
        bool encoderShouldEmitUTF8Identifier = true;  
        bool throwOnInvalidBytes = false; 
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
         StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        AssetDatabase.Refresh();
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}

class CreateComponentCSScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

        text = Regex.Replace(text, "ScriptNameComponent", fileNameWithoutExtension);
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        AssetDatabase.Refresh();
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}
