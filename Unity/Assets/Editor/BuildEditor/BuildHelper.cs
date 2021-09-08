using System.IO;
 using UnityEditor;
using UnityEngine;
using ETModel;
using System.Text;
using System.Linq;
namespace Utils.Editor
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

#if UNITY_ANDROID
        public static string BuildFolder = "../Release/Unity/Android/";
#endif
#if UNITY_IPHONE
        public static string BuildFolder = "../Release/Unity/StandaloneWindows64/";
#endif

#if UNITY_STANDALONE_WIN
        public static string BuildFolder = "../Release/Unity/StandaloneWindows64/";
#endif
 
        //[MenuItem("Tools/编译Hotfix")]
        public static void BuildHotfix()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string unityDir = System.Environment.GetEnvironmentVariable("Unity");
            if (string.IsNullOrEmpty(unityDir))
            {
                Log.Error("没有设置Unity环境变量!");
                return;
            }
            process.StartInfo.FileName = $@"{unityDir}\Editor\Data\MonoBleedingEdge\bin\mono.exe";
            process.StartInfo.Arguments = $@"{unityDir}\Editor\Data\MonoBleedingEdge\lib\mono\xbuild\14.0\bin\xbuild.exe .\Hotfix\Unity.Hotfix.csproj";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = @".\";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string info = process.StandardOutput.ReadToEnd();
            process.Close();
            Log.Info(info);
        }

        [MenuItem("Tools/web资源服务器")]
        public static void OpenFileServer()
        {
            string currentDir = System.Environment.CurrentDirectory;
            Debug.Log(currentDir);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "FileServer.dll";
            process.StartInfo.WorkingDirectory = "../FileServer/";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, string target, string version, string UpdateDescription, string UpdateDetails)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string exeName = "ET";
            switch (type)
            {
                case PlatformType.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
            }

            string fold = BuildFolder + target;
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }
            else
            {
                 FileHelper.CleanDirectory(fold);
            }

            Log.Info("开始资源打包");

             GenerateAssetInfo();
            
            BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

          

            ///加密
            BuildEditor.EncryptPartAB();

            GenerateVersionInfo(fold, version, UpdateDescription, UpdateDetails);

            Log.Info("完成资源打包");

            if (isContainAB)
            {
                FileHelper.CleanDirectory("Assets/StreamingAssets/");
                FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            }

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/Scenes/Init.unity",
                };
                Log.Info("开始EXE打包");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}/{exeName}", buildTarget, buildOptions);
                Log.Info("完成exe打包");
            }
        }

        private static void GenerateAssetInfo()
        {
            //CreateScript("ETHotfix", "Assets/Hotfix/Module/Config/AssetsBundleAddress_Generate.cs");
            //CreateScript("ETModel", "Assets/Model/Module/AssetsBundle/AssetsBundleAddress_Generate.cs");
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

        private static void GenerateVersionInfo(string dir, string version, string UpdateDescription, string UpdateDetails)
        {
            VersionConfig versionProto = new VersionConfig();
            versionProto.Version = version;
            GenerateVersionProto(dir, versionProto, "", UpdateDescription, UpdateDetails);

             using (FileStream fileStream = new FileStream($"{dir}/Version.txt", FileMode.Create))
            {
                byte[] bytes = JsonHelper.ToJson(versionProto).ToByteArray();
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static void GenerateVersionProto(string dir, VersionConfig versionProto, string relativePath, string UpdateDescription, string UpdateDetails)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                string md5 = MD5Helper.FileMD5(file);
                FileInfo fi = new FileInfo(file);
                long size = fi.Length;
                string filePath = relativePath == "" ? fi.Name : $"{relativePath}/{fi.Name}";
                //string filePath = fi.Name;
                versionProto.FileInfoDict.Add(filePath, new FileVersionInfo
                {
                    File = filePath,
                    MD5 = md5,
                    Size = size,
                });
                if (!versionProto.UpdateInfoDic.ContainsKey("1"))
                {
                    versionProto.UpdateInfoDic.Add("1", new UpdateInfo
                    {
                        Description = UpdateDescription,
                        Details = UpdateDetails,
                    });
                }
            }



            foreach (string directory in Directory.GetDirectories(dir))
            {
                DirectoryInfo dinfo = new DirectoryInfo(directory);
                string rel = relativePath == "" ? dinfo.Name : $"{relativePath}/{dinfo.Name}";
                GenerateVersionProto($"{dir}/{dinfo.Name}", versionProto, rel, UpdateDescription, UpdateDetails);
            }
        }
    }
}
