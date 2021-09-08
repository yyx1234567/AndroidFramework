using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using UnityEditor;
using System.Xml.Linq;
using System.Xml;
 
using System.Collections.Generic;
using ETModel;

public class IDEditor : OdinEditorWindow
{
    public TextAsset Config;

    public GameObject Target;
    public GameObject Car;

    public string TargetElement;
    public string TargetAttribute = "Name";

    public string ResultAttribute = "ID";

    public string Title;
 
    [MenuItem("Data/IDHelper")]
    private static void OpenWindow()
    {
        var window = GetWindow<IDEditor>();
    
        // Nifty little trick to quickly position the window in the middle of the editor.
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }

    [Button("生成ID")]
    public void Create()
    {
        //if (Config != null)
        //{
        //    var list = XmlConfigHelper.LoadXmlLinq(Config, out XDocument doc, false,TargetElement);
        //    int index = 0;
        //    Dictionary<string, string> PartIDList = new Dictionary<string, string>();
        //    foreach (var item in list)
        //    {
        //        string target =  item.Attribute(TargetAttribute).Value;
        //        if (!PartIDList.ContainsKey(target))
        //        {
        //            index++;
        //            PartIDList.Add(target, Title + "_" + index);
        //        }
        //         item.Attribute(ResultAttribute).SetValue(PartIDList[target]);
        //    }
        //     doc.Save(AssetDatabase.GetAssetPath(Config));
        //}
        //Debug.Log("完成ID替换");
    }
    
   
}
