using System.IO;
using UnityEngine;

namespace ETModel
{
    public static class PathHelper
    {
       // public static string RemoteBuildPath => UnityEngine.AddressableAssets.Addressables.RuntimePath;

        public static string IPAddress;

        /// <summary>
        /// 初始化服务器地址
        /// </summary>
        /// <param name="ip"></param>
        public static void Init(string ip)
        {
             IPAddress = ip;
        }
#if UNITY_ANDROID
        public static  string RemoteLoadPath => $"{IPAddress}/Unity/Android";
        public static  string BuildPath => Application.streamingAssetsPath;
#endif
#if UNITY_IPHONE
        public static string RemoteLoadPath =  $"{IPAddress}/Unity/IOS";
        public static string BuildPath = "Assets/ServerData/IOS";
#endif

#if UNITY_STANDALONE_WIN
        public static string RemoteLoadPath => $"{IPAddress}/Unity/StandaloneWindows64";
        public static string BuildPath = Application.streamingAssetsPath;
#endif
        public static string SavePath
        {
            get
            {
                return AppHotfixResPath;
            }
        }

        /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                string game = ETModel.Global.LoadProjectName;
                string path = AppResPath;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{game}/";
                }
                return path+"/"+game;
            }
         }

 

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPath4Web
        {
            get
            {
#if UNITY_IOS || UNITY_STANDALONE_OSX
                return $"file://{Application.streamingAssetsPath}";
#else
                return Application.streamingAssetsPath;
#endif

            }
        }

        /// <summary>
        /// 设置项目路径
        /// </summary>
        /// <param name="path"></param>
        public static void SetProjectPath(string path)
        {
            LoadTargetName = path;
        }
        private static string LoadTargetName;
    }
}
