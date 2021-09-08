using System.Collections.Generic;
using System.IO;
using System.Linq;
 using LitJson;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using ETModel;
using ETEditor;

namespace Utils.Editor
{
	public class BundleInfo
	{
		public List<string> ParentPaths = new List<string>();
	}

	public enum PlatformType
	{
		None,
		Android,
		IOS,
		PC,
		MacOS,
	}

	public enum BuildType
	{
		Development,
		Release,
	}


	public class BuildEditor : EditorWindow
	{
		private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

		private PlatformType platformType;
		private bool isBuildExe;
		private bool isContainAB;
		private string targetname;
		private string buildVersionName;
		private int buildVersionIndex;
		private int majorVersion;
		private int minorVersion;
		private int patchVersion;
		private string[] buildVersionOptions = new string[] { "alpha", "beta", "release", "full version" };
		private bool readLocalVersion;
		private BuildType buildType;
		private BuildOptions buildOptions = BuildOptions.AllowDebugging | BuildOptions.Development;
		private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;

		private string UpdateDescription;
		private string UpdateDetails;


		private static string EncryptABPath
		{
			get { return "../Release/"; }
		}

		private static string[] suffixIgnore = new string[]
		{
		  ".meta",
		  ".manifest",
		  ".txt"
		};

		[MenuItem("Tools/加密/加密部分AB包")]
		public static void EncryptPartAB()
		{
			DirectoryInfo directory = new DirectoryInfo(EncryptABPath);
			FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				var count = suffixIgnore.Where(x => files[i].Name.EndsWith(x)).Count();
				if (count > 0)
					continue;
				AES.AESFileEncrypt(files[i].FullName, AES.EncryptKey);
			}
			Debug.Log("加密完成！");
		}
		[MenuItem("Tools/加密/解密部分AB包")]
		public static void DecrptyPartAB()
		{
			DirectoryInfo directory = new DirectoryInfo(EncryptABPath);
			FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				var count = suffixIgnore.Where(x => files[i].Name.EndsWith(x)).Count();
				if (count > 0)
					continue;

				AES.AESFileDecrypt(files[i].FullName, AES.EncryptKey);
			}
			Debug.Log("解密完成！");
		}


		[MenuItem("Tools/打包工具")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuildEditor));
		}


		private void OnGUI()
		{
			if (!readLocalVersion)
			{
#if UNITY_ANDROID
				platformType = PlatformType.Android;
#endif
#if UNITY_IPHONE
				platformType = PlatformType.IOS;
#endif
#if UNITY_STANDALONE_WIN
				platformType = PlatformType.PC;
#endif
				UpdateDescription = "";
				UpdateDetails = "";
				targetname = Application.productName.ToLower();
				var path = Path.Combine(Application.streamingAssetsPath, "BuildVersion.json");
				if (File.Exists(path))
				{
					var data = File.ReadAllText(path);
					var info = JsonMapper.ToObject<VersionInfo>(data);
					majorVersion = info.majorVersion;
					minorVersion = info.minorVersion;
					patchVersion = info.patchVersion;
					targetname = info.buildVersionName;
					buildVersionIndex = info.buildVersionIndex;
				}
				readLocalVersion = true;
			}
			this.platformType = (PlatformType)EditorGUILayout.EnumPopup(platformType);

			this.buildType = (BuildType)EditorGUILayout.EnumPopup("BuildType: ", this.buildType);

			using (EditorGUILayout.HorizontalScope horizontal = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("打包版本：", EditorStyles.boldLabel, GUILayout.Width(100));

				for (int i = 0; i < this.buildVersionOptions.Length; i++)
				{
					using (EditorGUILayout.HorizontalScope toggleHorizontal = new EditorGUILayout.HorizontalScope())
					{
						if (EditorGUILayout.Toggle(this.buildVersionIndex == i, EditorStyles.toggle, GUILayout.Width(14)) && this.buildVersionIndex != i)
						{
							this.buildVersionIndex = i;
						}
						EditorGUILayout.LabelField(this.buildVersionOptions[i], EditorStyles.label, GUILayout.Width(80));
					}
				}
			}
			using (EditorGUILayout.VerticalScope vertical = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("版本号：", EditorStyles.boldLabel, GUILayout.Width(100));

				using (EditorGUILayout.VerticalScope versions = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
				{
					using (EditorGUILayout.HorizontalScope majorHorizontal = new EditorGUILayout.HorizontalScope())
					{
						EditorGUILayout.LabelField("主版本号（当 API 的兼容性变化时，X 需递增）：", EditorStyles.label);
						this.majorVersion = EditorGUILayout.IntField(this.majorVersion, GUILayout.ExpandWidth(false));
					}
					using (EditorGUILayout.HorizontalScope minorHorizontal = new EditorGUILayout.HorizontalScope())
					{
						EditorGUILayout.LabelField("次版本号（当增加功能时(不影响 API 的兼容性)，Y 需递增）：", EditorStyles.label);
						this.minorVersion = EditorGUILayout.IntField(this.minorVersion, GUILayout.ExpandWidth(false));
					}
					using (EditorGUILayout.HorizontalScope majorHorizontal = new EditorGUILayout.HorizontalScope())
					{
						EditorGUILayout.LabelField("修订号（当做 Bug 修复时(不影响 API 的兼容性)）：", EditorStyles.label);
						this.patchVersion = EditorGUILayout.IntField(this.patchVersion, GUILayout.ExpandWidth(false));
					}
				}
				if (this.buildVersionIndex == this.buildVersionOptions.Length - 1)
				{
					this.buildVersionName = $"{this.majorVersion}.{this.minorVersion}.{this.patchVersion}";
				}
				else
				{
					this.buildVersionName = $"{this.majorVersion}.{this.minorVersion}.{this.patchVersion}.{this.buildVersionOptions[this.buildVersionIndex]}{System.DateTime.Now.ToString("yyyyMMddHHmmss")}";
				}
				EditorGUILayout.TextField($"待发布版本号：{this.buildVersionName}", EditorStyles.label, GUILayout.ExpandWidth(true));
			}

			targetname = EditorGUILayout.TextField("打包名称", targetname);

			switch (buildType)
			{
				case BuildType.Development:
					this.buildOptions = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;
					break;
				case BuildType.Release:
					this.buildOptions = BuildOptions.None;
					break;
			}

			this.buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("BuildAssetBundleOptions(可多选): ", this.buildAssetBundleOptions);


			UpdateDescription = EditorGUILayout.TextField("更新内容概述", UpdateDescription);
			UpdateDetails = EditorGUILayout.TextField("更新详情", UpdateDetails);
			isBuildExe = EditorGUILayout.Toggle("打包进EXE", isBuildExe);

 
			if (GUILayout.Button("开始打包"))
			{
				if (this.platformType == PlatformType.None)
				{
					Log.Error("请选择打包平台!");
					return;
				}
				var path = Path.Combine(Application.streamingAssetsPath, "BuildVersion.json");
				VersionInfo info = new VersionInfo();
				info.majorVersion = majorVersion;
				info.minorVersion = minorVersion;
				info.patchVersion = patchVersion;
				info.buildVersionIndex = buildVersionIndex;
				info.buildVersionName = targetname;
				var data = JsonMapper.ToJson(info);
				File.WriteAllText(path, data);
				BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB, targetname, buildVersionName, UpdateDescription, UpdateDetails);
			}
		}

		private void SetPackingTagAndAssetBundle()
		{
			ClearPackingTagAndAssetBundle();

			SetIndependentBundleAndAtlas("Assets/Bundles/Independent");

			SetBundleAndAtlasWithoutShare("Assets/Bundles/UI");

			SetRootBundleOnly("Assets/Bundles/Unit");

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
		}

		private static void SetNoAtlas(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

			foreach (string path in paths)
			{
				List<string> pathes = CollectDependencies(path);

				foreach (string pt in pathes)
				{
					if (pt == path)
					{
						continue;
					}

					SetAtlas(pt, "", true);
				}
			}
		}

		// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
		private static void SetBundles(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);
			}
		}

		// 会将目录下的每个prefab引用的资源打成一个包,只给顶层prefab打包
		private static void SetRootBundleOnly(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);
			}
		}

		// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
		private static void SetIndependentBundleAndAtlas(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				AssetImporter importer = AssetImporter.GetAtPath(path1);
				if (importer == null || go == null)
				{
					Log.Error("error: " + path1);
					continue;
				}
				importer.assetBundleName = $"{go.name}.unity3d";

				List<string> pathes = CollectDependencies(path1);

				foreach (string pt in pathes)
				{
					if (pt == path1)
					{
						continue;
					}

					SetBundleAndAtlas(pt, go.name, true);
				}
			}
		}

		private static void SetBundleAndAtlasWithoutShare(string dir)
		{
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);

				//List<string> pathes = CollectDependencies(path1);
				//foreach (string pt in pathes)
				//{
				//	if (pt == path1)
				//	{
				//		continue;
				//	}
				//
				//	SetBundleAndAtlas(pt, go.name);
				//}
			}
		}

		private static List<string> CollectDependencies(string o)
		{
			string[] paths = AssetDatabase.GetDependencies(o);

			//Log.Debug($"{o} dependecies: " + paths.ToList().ListToString());
			return paths.ToList();
		}

		// 分析共享资源
		private void SetShareBundleAndAtlas(string dir)
		{
			this.dictionary.Clear();
			List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

			foreach (string path in paths)
			{
				string path1 = path.Replace('\\', '/');
				Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

				SetBundle(path1, go.name);

				List<string> pathes = CollectDependencies(path1);
				foreach (string pt in pathes)
				{
					if (pt == path1)
					{
						continue;
					}

					// 不存在则记录下来
					if (!this.dictionary.ContainsKey(pt))
					{
						// 如果已经设置了包
						if (GetBundleName(pt) != "")
						{
							continue;
						}
						Log.Info($"{path1}----{pt}");
						BundleInfo bundleInfo = new BundleInfo();
						bundleInfo.ParentPaths.Add(path1);
						this.dictionary.Add(pt, bundleInfo);

						SetAtlas(pt, go.name);

						continue;
					}

					// 依赖的父亲不一样
					BundleInfo info = this.dictionary[pt];
					if (info.ParentPaths.Contains(path1))
					{
						continue;
					}
					info.ParentPaths.Add(path1);

					DirectoryInfo dirInfo = new DirectoryInfo(dir);
					string dirName = dirInfo.Name;

					SetBundleAndAtlas(pt, $"{dirName}-share", true);
				}
			}
		}

		private static void ClearPackingTagAndAssetBundle()
		{
			//List<string> bundlePaths = EditorResHelper.GetAllResourcePath("Assets/Bundles/", true);
			//foreach (string bundlePath in bundlePaths)
			//{
			//	SetBundle(bundlePath, "", true);
			//}

			List<string> paths = EditorResHelper.GetAllResourcePath("Assets/Res", true);
			foreach (string pt in paths)
			{
				SetBundleAndAtlas(pt, "", true);
			}
		}

		private static string GetBundleName(string path)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return "";
			}
			if (path.Contains("Resources"))
			{
				return "";
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return "";
			}

			return importer.assetBundleName;
		}

		private static void SetBundle(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return;
			}

			if (importer.assetBundleName != "" && overwrite == false)
			{
				return;
			}

			//Log.Info(path);
			string bundleName = "";
			if (name != "")
			{
				bundleName = $"{name}.unity3d";
			}

			importer.assetBundleName = bundleName;
		}

		private static void SetAtlas(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if (textureImporter == null)
			{
				return;
			}

			if (textureImporter.spritePackingTag != "" && overwrite == false)
			{
				return;
			}

			textureImporter.spritePackingTag = name;
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
		}

		private static void SetBundleAndAtlas(string path, string name, bool overwrite = false)
		{
			string extension = Path.GetExtension(path);
			if (extension == ".cs" || extension == ".dll" || extension == ".js" || extension == ".mat")
			{
				return;
			}
			if (path.Contains("Resources"))
			{
				return;
			}

			AssetImporter importer = AssetImporter.GetAtPath(path);
			if (importer == null)
			{
				return;
			}

			if (importer.assetBundleName == "" || overwrite)
			{
				string bundleName = "";
				if (name != "")
				{
					bundleName = $"{name}.unity3d";
				}

				importer.assetBundleName = bundleName;
			}

			TextureImporter textureImporter = importer as TextureImporter;
			if (textureImporter == null)
			{
				return;
			}

			if (textureImporter.spritePackingTag == "" || overwrite)
			{
				textureImporter.spritePackingTag = name;
				AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
			}
		}
	}
}
