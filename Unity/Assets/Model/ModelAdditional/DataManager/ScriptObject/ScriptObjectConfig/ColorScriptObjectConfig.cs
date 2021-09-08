using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace ETModel
{
    [CreateAssetMenu(menuName = "DataManager/Config/ColorData")]
    public class ColorScriptObjectConfig : ScriptObjectConfig
    {
        public TextAsset asset;

#if UNITY_EDITOR
        [Button("保存",ButtonSizes.Large)]
        public void Save()
        {
            var sb = new StringBuilder();
            sb.Clear();
             
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("\t");
            sb.AppendLine("namespace ETModel");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic  class ColorHelper");
            sb.AppendLine("\t{");
            foreach (ColorConfig item in ItemList)
            {
                 sb.AppendLine($"\t\tpublic static Color {item.TypeName}=new Color({Math.Round(item.ColorValue.r,3)}f,{Math.Round(item.ColorValue.g, 3)}f,{Math.Round(item.ColorValue.b, 3)}f,{Math.Round(item.ColorValue.a, 3)}f);");
                 sb.AppendLine();
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(AssetDatabase.GetAssetPath(asset), sb.ToString());

            EditorUtility.DisplayDialog("提示", "保存成功！", "确认");
         }
#endif
    }
}