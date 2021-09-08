using Coffee.UIExtensions;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ETModel
{
    public class AidUpdateBtn : MonoBehaviour
    {
        public GameObject ProgressPanel;
        public GameObject UpdateTip;

        public Sprite Norml, Selected;

        public UnityEngine.UI.Button ConfirmBtn;

        private string _ProjectName;
        public string ProjectName
        {
            get { return _ProjectName.ToLower(); }

            set { _ProjectName = value; }
        }

        public Action OnUpadteCompelete;
        public Action OnNeedUpdate;

        public bool UpdateCompelete;

        private ProjectBtnState BtnState;
        public string RemotePath;
        public string VersionUrl;

        public string SavePath
        {
            get
            {
                string game = ProjectName;
                string path = Application.streamingAssetsPath;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{game}/";
                }
                return path + "/" + game;
            }
        }

        public void InitProjectBtn(string projectName)
        {
            ProjectName = projectName;
            Init.Instance.StartCoroutine(CheckDownLoadSize());
            ConfirmBtn.onClick.AddListener(() => { DownLoadBundle(); });
        }

        public ProjectBtnState GetState()
        {
            return BtnState;
        }

        public async void SetState(ProjectBtnState state)
        {
            switch (state)
            {
                case ProjectBtnState.Updating:
                    ProgressPanel.SetActive(true);
                    UpdateTip.SetActive(false);
                    break;
                case ProjectBtnState.NeedUpdate:
                    UpdateTip.SetActive(true);
                    break;
                case ProjectBtnState.Ready:
                    UpdateCompelete = true;
                    ConfirmBtn.gameObject.SetActive(false);
                    ProgressPanel.SetActive(false);
                    OnUpadteCompelete?.Invoke();
                    break;
            }
            BtnState = state;
        }

        public IEnumerator CheckDownLoadSize()
        {
            while (true)
            {
                TotalSize = 0;
                //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                //watch.Start();            
                yield return Init.Instance.StartCoroutine(StartAsync());
                //watch.Stop();
                //Debug.Log(watch.Elapsed.Seconds);
                if (TotalSize != 0)
                {
                    SetState(ProjectBtnState.NeedUpdate);
                }
                else
                {
                    SetState(ProjectBtnState.Ready);
                }
                break;
            }
        }

        public VersionConfig remoteVersionConfig;

        private Dictionary<string, FileVersionInfo> hasDonwLoadedFile = new Dictionary<string, FileVersionInfo>();
        private byte[] remoteVersionConfigData;

        public Queue<string> bundles = new Queue<string>();

        public long TotalSize;

        public HashSet<string> downloadedBundles = new HashSet<string>();

        public string downloadingBundle;

        public UnityWebRequest webRequest;

        private Dictionary<string, ulong> downloaderSize = new Dictionary<string, ulong>();
        public long alreadyDownloadBytes = 0;
        public float ProgressValue
        {
            get
            {
                if (this.TotalSize == 0)
                {
                    return 0;
                }

                alreadyDownloadBytes = 0;

                foreach (string downloadedBundle in this.downloadedBundles)
                {
                    //Debug.Log("<color=green> 路径" + downloadedBundle + "</color>");
                    long size = this.remoteVersionConfig.FileInfoDict[downloadedBundle].Size;
                    alreadyDownloadBytes += size;
                }
                if (this.webRequest != null)
                {
                    alreadyDownloadBytes += (long)this.webRequest.downloadedBytes;
                    //if(downloaderSize.ContainsKey(downloadingBundle))
                    //alreadyDownloadBytes += (long)downloaderSize[downloadingBundle];
                }
                //Debug.Log($"{ProjectName}........alreadyDownloadBytes Length: " + alreadyDownloadBytes);
                return (alreadyDownloadBytes * 100f / this.TotalSize);
            }
        }

        public IEnumerator StartAsync()
        {
            // 获取远程的Version.txt
            hasDonwLoadedFile.Clear();
            downloadedBundles.Clear();

            VersionUrl = $"{AssetsBundleDownloader.configData.ConfigDic["AssetbundleUrl"]}/Unity/Android/{ProjectName}";
            RemotePath = VersionUrl;
            var webRequestAsync = UnityWebRequest.Get($"{RemotePath}/Version.txt");
            yield return webRequestAsync.SendWebRequest();
            remoteVersionConfigData = webRequestAsync.downloadHandler.data;
            remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.downloadHandler.text);

            if (webRequestAsync.downloadHandler.text.Length == 0)
            {
                SetState(ProjectBtnState.Error);
                Init.Instance.StopAllCoroutines();
                yield break;
            }
            Global.ServerState = ServerConnect.Success;

            try
            {
                // 获取streaming目录的Version.txt
                VersionConfig streamingVersionConfig;
                string versionPath = Path.Combine(SavePath, "Version.txt");
                if (File.Exists(versionPath))
                {
                    streamingVersionConfig = JsonHelper.FromJson<VersionConfig>(File.ReadAllText(versionPath));
                }
                else
                {
                    streamingVersionConfig = new VersionConfig();
                }


                // 删掉远程不存在的文件
                DirectoryInfo directoryInfo = new DirectoryInfo(SavePath);
                if (directoryInfo.Exists)
                {
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    int directoryFolderLength = directoryInfo.FullName.Length;
                    foreach (FileInfo fileInfo in fileInfos)
                    {

                        if (remoteVersionConfig.FileInfoDict.ContainsKey(fileInfo.FullName.Substring(directoryFolderLength).Replace("\\", "")))
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
                    string path = Path.Combine(SavePath, this.downloadingBundle);
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }
                    hasDonwLoadedFile.Add(this.downloadingBundle, remoteVersionConfig.FileInfoDict[this.downloadingBundle]);
                    VersionConfig config = new VersionConfig();
                    config.FileInfoDict = hasDonwLoadedFile;

                    var byteArray = System.Text.Encoding.UTF8.GetBytes(JsonMapper.ToJson(config));
                    using (FileStream fs = new FileStream(Path.Combine(SavePath, "Version.txt"), FileMode.Create))
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
        public void DownLoadBundle()
        {
            switch (BtnState)
            {
                case ProjectBtnState.Error:
                    Init.Instance.StartCoroutine(CheckDownLoadSize());
                    break;
                case ProjectBtnState.Ready:
                    Init.Instance.LoadProject(ProjectName);
                    break;
                case ProjectBtnState.NeedUpdate:
                    Init.Instance.StartCoroutine(DownloadAsync());
                    SetState(ProjectBtnState.Updating);
                    break;
            }
        }

        private float time;
        private long lastByte;
        public long speed;
        public int count;
        private void Update()
        {
            if (time >= 1)
            {
                speed = alreadyDownloadBytes - lastByte;
                lastByte = alreadyDownloadBytes;

                time = 0;
            }
            if (TotalSize != 0)
            {
                if (webRequest != null)
                {
                    time += Time.deltaTime;
                }

                ProgressPanel.transform.Find("SliderBg/LoadingProgress").GetComponent<Image>().fillAmount = ProgressValue / 100;

                if (ProgressValue == 100)
                {
                    SetState(ProjectBtnState.Ready);
                    TotalSize = 0;
                }
                //Debug.LogError($"ProjectBtn: {ProjectName}  " + alreadyDownloadBytes);
                //Debug.LogError("ProjectBtn:  " + alreadyDownloadBytes + "-------" + TotalSize);
            }
        }

        private bool downloadCompelete;
        public IEnumerator StartT(string url, string filePath)
        {
            downloadCompelete = false;

            var headRequest = UnityWebRequest.Head(url);

            yield return headRequest.SendWebRequest();

            var totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));
            //Debug.Log("<color=yellow> 路径" + url + "</color>");
            //Debug.Log("<color=yellow> 下载长度" + totalLength + "</color>");
            var dirPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var fileLength = fs.Length;

                if (fileLength < totalLength)
                {
                    fs.Seek(fileLength, SeekOrigin.Begin);

                    webRequest = UnityWebRequest.Get(url);
                    // webRequest.SetRequestHeader("Range", "bytes=" + fileLength + "-" + totalLength);
                    yield return webRequest.SendWebRequest();
                    fs.Write(webRequest.downloadHandler.data, 0, webRequest.downloadHandler.data.Length);
                    var index = 0;
                    while (!webRequest.isDone)
                    {
                        yield return 0;
                        var buff = webRequest.downloadHandler.data;
                        Debug.Log("部分字节长度：" + buff.Length);
                        if (buff != null)
                        {
                            var length = buff.Length - index;
                            fs.Write(buff, index, length);
                            index += length;
                            fileLength += length;
                        }
                    }
                    if (webRequest == null)
                    {
                        downloadCompelete = false;
                    }
                    else
                    {
                        downloadCompelete = webRequest.isDone;
                    }
                }
                //Debug.Log("<color=yellow> 文件长度" + fs.Length + "</color>");
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
