using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace ETModel
{
    [Title("路径设置")]
    [HideReferenceObjectPicker]
     public class SettingPath : SettingItemBase
    {
        [HideReferenceObjectPicker]
        public class PathInfoItem
        {
            [TableColumnWidth(150, false)]
            public string Name;
            [FolderPath]
            public string Path;
        }
        [ LabelText("路径列表")]
        [TableList]
         public List<PathInfoItem> pathInfoItems = new List<PathInfoItem>();

        [LabelText("保存的脚本对象")]
        public TextAsset Target;
#if UNITY_EDITOR
        [Button("保存", ButtonSizes.Large)]
        public void Save()
        {
            var sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("\t");
            sb.AppendLine("namespace ETModel");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic  class EditorPathHelper");
            sb.AppendLine("\t{");
            foreach (PathInfoItem item in pathInfoItems)
            {
                sb.AppendLine($"\t\tpublic static string {item.Name}=\"{item.Path}\";");
                sb.AppendLine();
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(AssetDatabase.GetAssetPath(Target), sb.ToString()); ;

            EditorUtility.DisplayDialog("提示", "保存成功！", "确认");
        }
#endif

    }
}
