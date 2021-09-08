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

namespace ETModel
{
    public enum ProjectBtnState
    {
        NeedDownLoad,
        NeedUpdate,
        Updating,
        Pause,
        Ready,
        Error,
    }

    public class ProjectBtn : MonoBehaviour
    {

        public UnityEngine.UI.Text Info;
        public UnityEngine.UI.Image Progress;
        public UnityEngine.UI.Button ConfirmBtn;
        public string ProjectName { get; set; }

        public Action OnUpadteCompelete;
        public Action OnNeedUpdate;

        public bool Pause { get; set; }
        private bool Inited { get; set; }

        public bool mainProject;

        private ProjectBtnState BtnState;

        public string RemotePath;

        public string VersionUrl;

        public string SavePath
        {
            get { return PathHelper.BuildPath + "/" + ProjectName; }
        }

        public void InitProjectBtn(string projectName)
        {
            ProjectName = projectName.ToLower();
            Init.Instance.StartCoroutine(CheckDownLoadSize());
            //VersionUrl = ApiCore.HttpRequestGetString(AssetsBundleComponent.configData.ConfigDic["AidUrl"] + ProjectName);
             // ConfirmBtn.onClick.AddListener(()=> { Game.EventSystem.Run(EventIdType.Event_UpdateProject, projectName); });
        }

        public ProjectBtnState GetState()
        {
            return BtnState;
        }

        public async void SetState(ProjectBtnState state)
        {
            if (mainProject)
            {
                foreach (var item in Global.SameProjectDic[ProjectName])
                {
                    if (!item.mainProject)
                    {
                        item.SetState(state);
                    }
                }
            }
            switch (state)
            {
                case ProjectBtnState.Error:
                   // Info.text = "加载失败";
                    break;
                case ProjectBtnState.Pause:
                   // Info.text = "继续";
                    //Progress.color = Color.yellow;
                    //Progress.GetComponent<UIGradient>().color1 = new Color(0, 0, 0, 0.3f);
                    //Progress.GetComponent<UIGradient>().color2 = new Color(0, 0, 0, 0.3f);
                    break;
                case ProjectBtnState.Ready:
                     if (Global.NeedUpdateProjectDic.ContainsKey(ProjectName))
                    {
                        TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();
                       await timer.WaitAsync(100);
                       // Global.NeedUpdateProjectDic.Remove(ProjectName);
                        if (Global.NeedUpdateProjectDic.Count == 0)
                        {
                            // Game.Scene.GetComponent<UIComponent>().GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow).SetDownLoadBtnState(false);
                        }
                    }
                   // Progress.fillAmount = 0;
                    OnUpadteCompelete?.Invoke();
                    //Progress.color = new Color(0.6f, 0.6f, 0.9f);
                    break;
                case ProjectBtnState.NeedUpdate:
                  //  Info.text = "更新";
                    break;
                case ProjectBtnState.NeedDownLoad:
                    //Info.text = "下载";
                    break;
                case ProjectBtnState.Updating:
                    //Progress.GetComponent<UIGradient>().color1 = ColorHelper.GradientBlue;
                    //Progress.GetComponent<UIGradient>().color2 = ColorHelper.GradientGreen;
                   // Info.text = "暂停";
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

                    OnNeedUpdate?.Invoke();
                    if (!Global.NeedUpdateProjectDic.ContainsKey(ProjectName))
                    {
                        Global.NeedUpdateProjectDic.Add(ProjectName, this);
                        //Game.Scene.GetComponent<UIComponent>().GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow).SetDownLoadBtnState(true);
                        
                        Game.EventSystem.Run(EventIdType.ControlUpdateIconEvent);
                    }
                   
                    if (!Inited)
                    {
                        if (!Global.SameProjectDic.ContainsKey(ProjectName))
                        {
                            Inited = true;
                            mainProject = true;
                            Global.SameProjectDic.Add(ProjectName, new List<ProjectBtn>() { this });
                        }
                        else
                        {
                            Inited = true;
                            mainProject = false;
                            Global.SameProjectDic[ProjectName].Add(this);
                        }
                    }
                     SetState(ProjectBtnState.NeedUpdate);
                }
                else
                {
                    SetState(ProjectBtnState.Ready);
                }
                break;
                //while (webRequest != null&&!Pause)
                //{
                //    yield return new WaitForSeconds(10);
                //}
                //yield return new WaitForSeconds(10);
             }
        }

        public VersionConfig remoteVersionConfig;

        private Dictionary<string, FileVersionInfo> hasDonwLoadedFile = new Dictionary<string, FileVersionInfo>();
        private byte[] remoteVersionConfigData;

        public List<string> bundles = new List<string>();

        public long TotalSize;

        public HashSet<string> downloadedBundles = new HashSet<string>();

        public string downloadingBundle;

        public UnityWebRequest webRequest;

        private Dictionary<string, ulong> downloaderSize = new Dictionary<string, ulong>();
        public  long alreadyDownloadBytes = 0;
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

             string versionUrl = "";
             VersionUrl = ApiCore.HttpRequestGetString(AssetsBundleDownloader.configData.ConfigDic["AidUrl"] + ProjectName);
             var jsondata = JsonMapper.ToObject(VersionUrl);

            versionUrl = jsondata["hashUrl"].ToString();
            RemotePath = versionUrl.Replace("Version.txt", "");
             var webRequestAsync = UnityWebRequest.Get(versionUrl);
            yield return webRequestAsync.SendWebRequest();
            remoteVersionConfigData = webRequestAsync.downloadHandler.data;
            remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.downloadHandler.text);

            if (webRequestAsync.downloadHandler.text.Length == 0)
            {
                if (!Inited)
                {
                    if (!Global.SameProjectDic.ContainsKey(ProjectName))
                    {
                        Inited = true;
                        mainProject = true;
                        Global.SameProjectDic.Add(ProjectName, new List<ProjectBtn>() { this });
                    }
                    else
                    {
                        Inited = true;
                        mainProject = false;
                        Global.SameProjectDic[ProjectName].Add(this);
                    }
                }
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


                    this.bundles.Add(fileVersionInfo.File);

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
            // if (this.bundles.Count == 0 && this.downloadingBundle == "")
            //{
            //     SetState(ProjectBtnState.Ready);
            //    yield break;
            //}

            while (true)
            {
                if (this.bundles.Count == 0)
                {
                    //SetState(ProjectBtnState.Ready);
                    yield break;
                }

                this.downloadingBundle = bundles[0];
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
 
                   if (!hasDonwLoadedFile.ContainsKey(this.downloadingBundle))
                   {
                       hasDonwLoadedFile.Add(this.downloadingBundle, remoteVersionConfig.FileInfoDict[this.downloadingBundle]);
                   }
                   VersionConfig config = new VersionConfig();
                    config.FileInfoDict = hasDonwLoadedFile;
                    config.UpdateInfoDic = remoteVersionConfig.UpdateInfoDic;


                    var byteArray = System.Text.Encoding.UTF8.GetBytes(JsonMapper.ToJson(config));
                    using (FileStream Version = new FileStream(Path.Combine(SavePath, "Version.txt"), FileMode.Create))
                    {
                        Version.Write(byteArray, 0, byteArray.Length);
                    }
                   bundles.RemoveAt(0);
                   break;
                }               
                this.downloadedBundles.Add(this.downloadingBundle);
               // UnityEngine.Debug.Log($"设置WebRequest为空: {downloadingBundle}");
                this.downloadingBundle = "";
                //if ((this.bundles.Count == 0))
                //{
                //    yield return new WaitForSeconds(0.02f);
                //}
                this.webRequest = null;
            }
            #region
             //while (true)
             //{
             //    if (this.bundles.Count == 0)
             //    {
             //        SetState(ProjectBtnState.Ready);
             //        yield break;
             //    }
             
             //    this.downloadingBundle = this.bundles.Dequeue();
             //    while (true)
             //    {
             //       webRequest = UnityWebRequest.Get(RemotePath + "/" + this.downloadingBundle);
             //       yield return   webRequest.SendWebRequest();
             //       if (this.webRequest.downloadHandler.data.Length==0)
             //       {
             //             Game.EventSystem.Run(EventIdType.Event_NetError);
             //             Init.Instance.StopAllCoroutines();
             //            yield break;
             //       }
             //          while (true)
             //         {
             //             if (Pause)
             //             {
             //                 yield return new WaitForSeconds(0.1f);
             //             }
             //             if (!Pause)
             //                 break;
             //         }
             //         byte[] data = this.webRequest.downloadHandler.data;
             
             //         string path = Path.Combine(SavePath, this.downloadingBundle);
             //         if( !Directory.Exists(SavePath))
             //             Directory.CreateDirectory(SavePath);
             //         using (FileStream fs = new FileStream(path, FileMode.Create))
             //         {
             //             fs.Write(data, 0, data.Length);
             //         }
             //       if (!hasDonwLoadedFile.ContainsKey(this.downloadingBundle))
             //       {
             //           hasDonwLoadedFile.Add(this.downloadingBundle, remoteVersionConfig.FileInfoDict[this.downloadingBundle]);
             //       }
             //       VersionConfig config = new VersionConfig();
             //         config.FileInfoDict = hasDonwLoadedFile;
             
             //         var byteArray = System.Text.Encoding.UTF8.GetBytes(JsonMapper.ToJson(config));
             //         using (FileStream fs = new FileStream(Path.Combine(SavePath, "Version.txt"), FileMode.Create))
             //         {
             //             fs.Write(byteArray, 0, byteArray.Length);
             //         }
             
             //        break;
             //    }
             //    this.downloadedBundles.Add(this.downloadingBundle);
             //    this.downloadingBundle = "";
             //    this.webRequest = null;
             //}
            #endregion
        }

         public void DownLoadBundle()
        {
              switch (BtnState)
            {
                 case ProjectBtnState.Error:
                    Init.Instance.StartCoroutine(CheckDownLoadSize());
                    break;
                case ProjectBtnState.Updating:
                    Pause = true;
                    SetState(ProjectBtnState.Pause);
                    break;
                case ProjectBtnState.Ready:
                    Init.Instance.LoadProject(ProjectName);
                    break;
                case ProjectBtnState.Pause:
                    Pause = false;
                    Init.Instance.StartCoroutine(DownloadAsync());
                    SetState(ProjectBtnState.Updating);
                    break;
                case ProjectBtnState.NeedUpdate:
                    Pause = false;
                    Init.Instance.StartCoroutine(DownloadAsync());
                    SetState(ProjectBtnState.Updating);
                    break;
                case ProjectBtnState.NeedDownLoad:
                    break;
            }
        }
 
        private float time;
        private long lastByte;
        public long speed;
        public int count;
        private void Update()
        {
            if (mainProject)
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
                    if (ProgressValue == 100)
                    {
                         SetState(ProjectBtnState.Ready);
                        TotalSize = 0;
                    }
                    //Debug.LogError($"ProjectBtn: {ProjectName}  " + alreadyDownloadBytes);
                    //Debug.LogError("ProjectBtn:  " + alreadyDownloadBytes + "-------" + TotalSize);
                }
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
                         if (Pause)
                        {
                             if (downloaderSize.ContainsKey(downloadingBundle))
                            {
                                downloaderSize[downloadingBundle] += webRequest.downloadedBytes;
                            }
                            else
                            {
                                downloaderSize.Add(downloadingBundle, webRequest.downloadedBytes);
                            }
                             webRequest.Abort();
                            webRequest.Dispose();
                            webRequest = null;
                             break;
                        }
                        yield return 0;
                        var buff = webRequest.downloadHandler.data;
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
