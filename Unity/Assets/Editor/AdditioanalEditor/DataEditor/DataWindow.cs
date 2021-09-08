using ETModel;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DataWindow : OdinMenuEditorWindow
{
    public List<string> OperateName = new List<string>();

    public Dictionary<string, string> NameDic = new Dictionary<string,string>();

    private static readonly string basepath = "Res/ScriptObject/";
    private static string ScriptObjectProjectDataConfig => basepath + "ScriptObjectConfig/ProjectConifg";
    private static string ScriptObjectCommonDataConfig => basepath + "ScriptObjectConfig/CommonConfig";
    private static string ScriptObjectData => basepath + "ScriptObjectData";
    private static string ProjectScriptObject => basepath + "Manager/ProjectManagerData/Task";
    private static string ScriptObjectTool => basepath + "EditorScriptObject";
    private static string ScriptObjectCreator => basepath + "ScriptObjectCreator";
    private static string ComponentPath => "Assets/"+ basepath + "Manager/ComponentManager.asset";
    private static string UIToolPath => basepath + "UIToolScirptObject";
    private static string ComponentInfoPath => basepath + "Component/ComponentInfo";
    private static string ComponentProjectInfoPath => basepath + "Component/ProjectComponentInfo";

    private static string SettingPath => basepath + "SettingConfig/Item";
    private static string SettingObject => "Assets/"+ basepath + "SettingConfig/Main/SettingConfig.asset";

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
        {
          
        };
      
        tree.Config.DrawSearchToolbar = true;
         
        tree.AddAllAssetsAtPath("配置表/数据", ScriptObjectProjectDataConfig, typeof(ScriptObjectConfig), true, true).AddThumbnailIcons();

        tree.AddAllAssetsAtPath("配置表/通用", ScriptObjectCommonDataConfig, typeof(ScriptObjectConfig), true, true).AddThumbnailIcons();
 
        tree.AddAllAssetsAtPath("数据创建器", ScriptObjectCreator, typeof(DataCreator), true, true).AddThumbnailIcons();

        tree.AddAllAssetsAtPath("数据", ScriptObjectData, typeof(CharacterBaseData), true, true).AddThumbnailIcons();
 
        tree.AddAllAssetsAtPath("工具", ScriptObjectTool, typeof(Sirenix.OdinInspector.SerializedScriptableObject), true, true).AddThumbnailIcons();

        tree.AddAllAssetsAtPath("UI工具", UIToolPath, typeof(Sirenix.OdinInspector.SerializedScriptableObject), true, true).AddThumbnailIcons();

        tree.Add("组件", AssetDatabase.LoadAssetAtPath<ScriptableObject>(ComponentPath));

        tree.AddAllAssetsAtPath("组件/框架组件", ComponentInfoPath, typeof(ComponentInfo), true, true);
        tree.AddAllAssetsAtPath("组件/项目组件", ComponentProjectInfoPath, typeof(ComponentInfo), true, true);

        tree.Add("项目/自定义案例", AssetDatabase.LoadAssetAtPath<ScriptableObject>(EditorPathHelper.ProjectManager), EditorIcons.Paperclip);
        tree.Add("项目/自定义案例", AssetDatabase.LoadAssetAtPath<ScriptableObject>(EditorPathHelper.ProjectManager),EditorIcons.Paperclip);
        tree.AddAllAssetsAtPath("项目/项目任务列表", ProjectScriptObject, typeof(Sirenix.OdinInspector.SerializedScriptableObject), true, true).AddThumbnailIcons();
        foreach (var item in GetDataPath<ProjectScriptObjectConfig>(EditorPathHelper.ProjectData, (x)=> { return x.Name; }))
        {
            tree.Add("项目/自定义案例/" + item.Key, AssetDatabase.LoadAssetAtPath<ScriptableObject>(item.Value),EditorIcons.GridImageText);
        }

         
        //tree.AddAllAssetsAtPath("组件列表", ComponentInfoPath, typeof(ComponentInfo), true, true).AddThumbnailIcons();

        // tree.AddAllAssetsAtPath("设置", SettingPath, typeof(ScriptableObject), true, true).AddThumbnailIcons();

        tree.Add("设置", AssetDatabase.LoadAssetAtPath<ScriptableObject>(SettingObject), icon: EditorIcons.SettingsCog);

        tree.Add("设置/Player Settings", Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault());
        tree.AddAllAssetsAtPath("设置", SettingPath, typeof(ScriptableObject), true, true);
         
        return tree;
    }

    public static Dictionary<string, string> GetDataPath<T>(string DataPath, System.Func<T, string> GetData) where T : UnityEngine.Object, new()
    {
        Dictionary<string, string> NameDic = new Dictionary<string, string>();
        if (Directory.Exists(DataPath))
        {
            DirectoryInfo direction = new DirectoryInfo(DataPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                string path = files[i].FullName;
                path = path.Replace(@"\", "/");
                path = "Assets" + path.Replace(Application.dataPath, "");
                 T part = AssetDatabase.LoadAssetAtPath<T>(path);
                if (part == null)
                    continue;
                var name = GetData.Invoke(part);
                if (!NameDic.ContainsKey(name))
                {
                    NameDic.Add(name, path);
                }
            }
        }
        return NameDic;
    }
}
