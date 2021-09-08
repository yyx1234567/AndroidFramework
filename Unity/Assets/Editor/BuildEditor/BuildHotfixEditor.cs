using System;
using System.IO;
using System.Linq;
using System.Text;
using ETModel;
using UnityEditor;

namespace ETEditor
{
    [InitializeOnLoad]
    public class Startup
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/Res/Code/";
        private const string HotfixDll = "Unity.Hotfix.dll";
        private const string HotfixPdb = "Unity.Hotfix.pdb";

        static Startup()
        {
            //File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            //File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            //CreateScript("ETHotfix", "Assets/Hotfix/Module/Config/AssetsBundleAddress_Generate.cs");
            //CreateScript("ETModel", "Assets/Model/Module/AssetsBundle/AssetsBundleAddress_Generate.cs");
            Log.Info($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
            //if(!UnityEngine.Application.isPlaying)
            //AssetDatabase.Refresh ();
        }
        private static void CreateScript(string NameSpace, string path)
        {
             StringBuilder sb = new StringBuilder();
             sb.Clear();
             sb.AppendLine("using System.Collections.Generic;");
             sb.AppendLine($"namespace {NameSpace}");
             sb.AppendLine("{");
             sb.AppendLine($"\tpublic partial class AssetsBundleAddress");
             sb.AppendLine("\t{");
             var data = AssetBundleBrowser.AssetBundleModel.Model.DataSource;
             foreach (var item in data.GetAllAssetBundleNames())
             {
                if (!item.Contains(UnityEngine.Application.productName))
                {
                    continue;
                }
                 foreach (var asset in data.GetAssetPathsFromAssetBundle(item))
                 {
                     var assetName = asset.Split('/').LastOrDefault().Split('.').FirstOrDefault();
                     var bundleName = item.Split('_').LastOrDefault().Split('.').FirstOrDefault();
                     sb.AppendLine($"\t\tpublic const string {bundleName}_{assetName} =\"{bundleName}_{assetName}\";");
                     sb.AppendLine("\t");
                 }
             }
             sb.AppendLine("\t\tpublic static void Init()");
             sb.AppendLine("\t\t{");
             foreach (var item in data.GetAllAssetBundleNames())
             {
                if (!item.Contains(UnityEngine.Application.productName))
                {
                    continue;
                }
                foreach (var asset in data.GetAssetPathsFromAssetBundle(item))
                 {
                     var assetName = asset.Split('/').LastOrDefault().Split('.').FirstOrDefault();
                     var bundleName = item.Split('_').LastOrDefault().Split('.').FirstOrDefault();
                     sb.AppendLine($"\t\t\tassetbundleDic.Add({$"\"{bundleName}_{assetName}\""}, new AssetBundleInfo(){{BundleName = \"{item}\", AssetName=\"{assetName}\"}});");
                 }
             }
             sb.AppendLine("\t\t}");
             sb.AppendLine("\t}");
             sb.AppendLine("}");
             
             sb.AppendLine();
             
             System.IO.File.WriteAllText(path, sb.ToString());
        }

    }
}