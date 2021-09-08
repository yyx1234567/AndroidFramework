using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ETModel
{
    public class ConfigData
    {
        public Dictionary<string, string> ConfigDic = new Dictionary<string, string>();
    }


    public class AssetsBundleDownloader : MonoBehaviour
    {
        public Text VersionText;
        public Button ConfirmBtn, Quitbtn;
        public GameObject MessageBox;
        private string RemotePath;
        public Image Progress;
        public static ConfigData configData;
        void Start()
        {
            configData = JsonMapper.ToObject<ConfigData>(LoadFile("Config.txt"));
            var ip = configData.ConfigDic["AssetbundleUrl"];
            PathHelper.Init(ip);
            ConfirmBtn.onClick.AddListener(() =>
            {
                StartCoroutine(CheckDownLoadSize());
                MessageBox.SetActive(false);
            });
            Quitbtn.onClick.AddListener(async () =>
            {
                Application.Quit();
            });
            Progress.transform.parent.gameObject.SetActive(false);

            if (Init.Instance.LoadProjectDirect)
            {
                gameObject.SetActive(false);
                return;
            }
            if (Define.IsAsync)
            {
                StartCoroutine(CheckDownLoadSize());
            }
            else
            {
                gameObject.SetActive(false);
                Init.Instance.LoadPlatForm();
            }
        }

        public static string LoadFile(string filePath)
        {
            string url = Application.streamingAssetsPath + "/" + filePath;
#if UNITY_EDITOR
            return File.ReadAllText(url);
#elif UNITY_ANDROID
            WWW www = new WWW(url);
            while (!www.isDone) { }
            return www.text;
#endif
        }


        public IEnumerator CheckDownLoadSize()
        {
            yield return StartCoroutine(StartAsync());

            if (TotalSize != 0)
            {
                Progress.transform.parent.gameObject.SetActive(true);
                StartCoroutine(DownloadAsync());
            }
            else
            {
                CheckProject();
            }
        }

        private VersionConfig remoteVersionConfig;

        private Dictionary<string, FileVersionInfo> hasDonwLoadedFile = new Dictionary<string, FileVersionInfo>();
        private byte[] remoteVersionConfigData;

        public Queue<string> bundles = new Queue<string>();

        [System.NonSerialized]
        public long TotalSize;

        public HashSet<string> downloadedBundles = new HashSet<string>();

        [System.NonSerialized]
        public string downloadingBundle;

        [System.NonSerialized]
        public UnityWebRequest webRequest;
        public int ProgressValue
        {
            get
            {
                if (this.TotalSize == 0)
                {
                    return 0;
                }

                long alreadyDownloadBytes = 0;
                foreach (string downloadedBundle in this.downloadedBundles)
                {
                    long size = this.remoteVersionConfig.FileInfoDict[downloadedBundle].Size;
                    alreadyDownloadBytes += size;
                }
                if (this.webRequest != null)
                {
                    alreadyDownloadBytes += (long)this.webRequest.downloadedBytes;
                }
                //DownLoadSize.text = $"剩余大小: {ConventToSize(TotalSize - alreadyDownloadBytes)}";

                return (int)(alreadyDownloadBytes * 100f / this.TotalSize);
            }
        }
        public IEnumerator StartAsync()
        {
            // 获取远程的Version.txt
            string versionUrl = "";
            versionUrl = $"{PathHelper.RemoteLoadPath}/{Application.productName}/Version.txt";
            RemotePath = $"{PathHelper.RemoteLoadPath}/{Application.productName}";
            var webRequestAsync = UnityWebRequest.Get(versionUrl);
            yield return webRequestAsync.SendWebRequest();
            remoteVersionConfigData = webRequestAsync.downloadHandler.data;
            if (webRequestAsync.downloadHandler.text.Length == 0)
            {
                MessageBox.SetActive(true);
                StopAllCoroutines();
                yield break;
            }
            remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.downloadHandler.text);
            try
            {
                // 获取streaming目录的Version.txt
                VersionConfig streamingVersionConfig;
                string versionPath = Path.Combine(PathHelper.SavePath, "Version.txt");
                if (File.Exists(versionPath))
                {
                    streamingVersionConfig = JsonHelper.FromJson<VersionConfig>(File.ReadAllText(versionPath));
                }
                else
                {
                    streamingVersionConfig = new VersionConfig();
                }
                // 删掉远程不存在的文件
                DirectoryInfo directoryInfo = new DirectoryInfo(PathHelper.SavePath);
                if (directoryInfo.Exists)
                {
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        if (remoteVersionConfig.FileInfoDict.ContainsKey(fileInfo.Name))
                        {
                            continue;
                        }
                        if (fileInfo.Name == "Version.txt")
                        {
                            continue;
                        }
                        fileInfo.Delete();
                    }
                }
                else
                {
                    directoryInfo.Create();
                }

                // 对比MD5
                foreach (FileVersionInfo fileVersionInfo in remoteVersionConfig.FileInfoDict.Values)
                {
                    // 对比md5
                    string localFileMD5 = BundleHelper.GetBundleMD5(streamingVersionConfig, fileVersionInfo.File);


                    if (fileVersionInfo.MD5 == localFileMD5)
                    {
                        hasDonwLoadedFile.Add(fileVersionInfo.File, fileVersionInfo);
                        continue;
                    }

                    this.bundles.Enqueue(fileVersionInfo.File);

                    this.TotalSize += fileVersionInfo.Size;
                }
            }

            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.StackTrace);
            }


        }

        public IEnumerator DownloadAsync()
        {
            if (this.bundles.Count == 0 && this.downloadingBundle == "")
            {
                yield break;
            }

            while (true)
            {
                if (this.bundles.Count == 0)
                {
                    break;
                }

                this.downloadingBundle = this.bundles.Dequeue();

                while (true)
                {
                    webRequest = UnityWebRequest.Get(RemotePath + "/" + this.downloadingBundle);
                    yield return webRequest.SendWebRequest();
                    byte[] data = this.webRequest.downloadHandler.data;

                    string path = Path.Combine(PathHelper.SavePath, this.downloadingBundle);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }
                    hasDonwLoadedFile.Add(this.downloadingBundle, remoteVersionConfig.FileInfoDict[this.downloadingBundle]);
                    VersionConfig config = new VersionConfig();
                    config.FileInfoDict = hasDonwLoadedFile;

                    var byteArray = System.Text.Encoding.UTF8.GetBytes(JsonMapper.ToJson(config));
                    using (FileStream fs = new FileStream(Path.Combine(PathHelper.SavePath, "Version.txt"), FileMode.Create))
                    {
                        fs.Write(byteArray, 0, byteArray.Length);
                    }
                    break;
                }
                this.downloadedBundles.Add(this.downloadingBundle);
                this.downloadingBundle = "";
                this.webRequest = null;
            }
        }


        private void Update()
        {
            if (TotalSize != 0)
            {
                if (webRequest != null)
                {
                    Progress.fillAmount = ProgressValue / 100f;
                    if (ProgressValue == 100)
                    {
                        using (FileStream fs = new FileStream(Path.Combine(PathHelper.SavePath, "Version.txt"), FileMode.Create))
                        {
                            fs.Write(remoteVersionConfigData, 0, remoteVersionConfigData.Length);
                        }
                        CheckProject();
                    }
                }
                else if (ProgressValue >= 99)
                {
                    CheckProject();
                }
            }
        }
        private static bool open;
        private async void CheckProject()
        {
            if (open)
                return;
            open = true;
            await Task.Delay(2000);
            gameObject.SetActive(false);
            Init.Instance.LoadPlatForm();
        }
    }
}
