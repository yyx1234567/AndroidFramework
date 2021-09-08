using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using ETModel;
using UnityEngine.Networking;
using System.Collections;
using LinqUtils;

namespace ETHotfix
{
    public partial class UIStoreWindowComponent : UIWindowComponent
    {
        public UIToggleOdin CurrentAidTog;
        private Dictionary<UIToggleOdin, AidItem> AidDic = new Dictionary<UIToggleOdin, AidItem>();
        private string CurrentSelectedAid;
        public List<string> CurrentSelectedTaskList = new List<string>();
        public bool FirstSelect;
        public string CurrentSelectedTask;
        public class AidItem
        {
            public AidData AidData;
            public Dictionary<string, string> TaskList = new Dictionary<string, string>();
        }
        private void OnTaskSelectEvent(UIToggleOdin obj)
        {
            string name = obj.GetComponentInChildren<Text>().text;

            var tog = AidPanel.GetComponent<UIToggleGroupOdin>().lastUIToggle;
            AidDic.TryGetValue(tog, out AidItem value);


            value.TaskList.TryGetValue(name, out string taskid);

            if (obj.IsOn)
            {
                ShowBtn.interactable = true;
                CurrentSelectedTask = taskid;
            }
            else
            {
                if (obj.Group.ActiveToggle.Count == 0)
                {
                    ShowBtn.interactable = false;
                }
                CurrentSelectedTask = string.Empty;
            }
        }
        private void RegisterEvent()
        {
            ShowBtn.onClick.AddListener(() => { EnterTeaching(); });
            TaskContent.GetComponent<UIGrid>().ItemEvent += OnTaskSelectEvent;
            ShowBtn.interactable = false;
            CreateData();
        }

        private void CreateData()
        {
            List<AidData> aiddataList = new List<AidData>();
            for (int i = 1; i < 2; i++)
            {
                AidData aidData = new AidData();
                aidData.virtualAidCode = $"project";
                aidData.virtualAidId = $"00{i}";
                aidData.virtualAidName = $"项目00{i}";
                aiddataList.Add(aidData);
            }

            var data = "";
            var list = JsonMapper.ToObject<List<AidData>>(data);
            list = aiddataList;
            var aidlist = PlayerPrefs.GetString(PlayerPrefsData.PlayerPrefs_AidList);
            List<AidData> aids = new List<AidData>();
            if (!string.IsNullOrEmpty(aidlist))
            {
                foreach (var aid in JsonMapper.ToObject<List<string>>(aidlist))
                {
                    foreach (var item in list)
                    {
                        if (item.virtualAidCode == aid)
                        {
                            aids.Add(item);
                            break;
                        }
                    }
                }
                foreach (var item in list)
                {
                    if (!aids.Contains(item))
                    {
                        aids.Add(item);
                    }
                }
            }
            else
            {
                aids = list;
            }
            GenerateMission(aids);
        }

        private void EnterTeaching()
        {
            if (string.IsNullOrEmpty(CurrentSelectedAid) || string.IsNullOrEmpty(CurrentSelectedTask))
            {
                return;
            }

            if (!CurrentAidTog.GetComponent<AidUpdateBtn>().UpdateCompelete)
                return;

            var data = PlayerPrefs.GetString(PlayerPrefsData.PlayerPrefs_AidList);

            List<string> aidlist;
            if (string.IsNullOrEmpty(data))
            {
                aidlist = new List<string>();
            }
            else
            {
                aidlist = JsonMapper.ToObject<List<string>>(data);
            }
            if (!aidlist.Contains(CurrentSelectedAid))
            {
                aidlist.Add(CurrentSelectedAid);
            }
            else
            {
                aidlist.Remove(CurrentSelectedAid);
                aidlist.Insert(0, CurrentSelectedAid);
            }
            //ETModel.Global.CurrentMission = new CloudPlatformStartConfig();
            //ETModel.Global.CurrentMission.Mode = CloudPlatformLaunchMode.Teaching;
            //ETModel.Global.CurrentMission.TaskID = CurrentSelectedTask;

            Game.Close();
            ETModel.Init.Instance.LoadProject(CurrentSelectedAid);

            PlayerPrefs.SetString(PlayerPrefsData.PlayerPrefs_AidList, JsonMapper.ToJson(aidlist));
        }

        private void GenerateMission(List<AidData> useraids)
        {
            List<string> nameList = new List<string>();
            foreach (var item in useraids)
            {
                nameList.Add(item.virtualAidName);
            }
            var AidsPanelGrid = AidPanel.GetComponent<UIGrid>();
            AidsPanelGrid.Show(nameList);
            foreach (var item in AidsPanelGrid.GetComponentsInChildren<UIToggleOdin>())
            {
                item.GetComponent<ETModel.AidUpdateBtn>().OnUpadteCompelete += AidUpdateCompeleteHandle;
                item.ToggleEventSelf += SelectAidEvent;
            }
            //AidsPanelGrid.ItemEvent += SelectAidEvent;
            for (int i = 0; i < AidsPanelGrid.m_ShowList.Count; i++)
            {
                AidsPanelGrid.m_ShowList[i].GetComponent<AidUpdateBtn>().InitProjectBtn(useraids[i].virtualAidCode);
                GetSprite(AidsPanelGrid.m_ShowList[i].transform.Find("TrainAidBtn").GetComponent<Image>(), useraids[i].virtualAidImage);
                var aiditem = new AidItem();
                aiditem.AidData = useraids[i];
                AidDic.Add(AidsPanelGrid.m_ShowList[i].GetComponent<UIToggleOdin>(), aiditem);
                //AidsPanelGrid.m_ShowList[i].GetComponent<AidUpdateBtn>().OnUpadteCompelete += OnUpadteCompelete;
            }
        }
        private void GetSprite(Image target, string virtualAidImage)
        {
            //WebRequestController.Instance.StartCoroutine(GetNetSprite(virtualAidImage, target));
        }
        private IEnumerator GetNetSprite(string url, Image target)
        {
            UnityWebRequest unityWeb = new UnityWebRequest(url);
            DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
            unityWeb.downloadHandler = texDl;
            yield return unityWeb.SendWebRequest();
            Texture2D tex = new Texture2D(180, 180);
            tex = texDl.texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            target.sprite = sprite;
        }
        private void SelectAidEvent(UIToggleOdin obj)
        {
            if (obj.IsOn)
            {
                ShowBtn.interactable = false;
                CurrentAidTog = obj;
                AidDic.TryGetValue(obj, out AidItem value);
                if (value != null && value.TaskList.Count == 0)
                {
                    //var data =  ApiCore.HttpRequestGetAsyncUrlDym($"{ApiCore.Api_get_useraids}/{value.AidData.virtualAidId}/tasks", token: true);
                    //var taskdata = JsonMapper.ToObject<TaskItem>(data);

                    var taskdata = new TaskItem()
                    {
                        items = new List<Items>()
                      {
                         new Items(){ name="任务001" ,code="001"},
                         new Items(){ name="任务002" ,code="002"},
                         new Items(){ name="任务003" ,code="003"},
                      }
                    };

                    foreach (var item in taskdata.items)
                    {
                        if (!value.TaskList.ContainsKey(item.name))
                        {
                            value.TaskList.Add(item.name, item.code);
                        }
                    }
                }
                var tasklist = value.TaskList.Select(x => x.Key).ToList();

                var TaskPanelGrid = TaskContent.GetComponent<UIGrid>();
                TaskPanelGrid.Show(tasklist);
                CurrentSelectedAid = value.AidData.virtualAidCode;
                if (obj.GetComponent<AidUpdateBtn>().GetState() != ProjectBtnState.Ready)
                {
                    foreach (var item in TaskPanelGrid.m_ShowList)
                    {
                        //item.GetComponent<UIToggleOrdin>().Interactable = true;
                        item.GetComponent<UIToggleOdin>().IsOn = false;
                        item.GetComponent<UIToggleOdin>().ChangeState(ToggleState.UnIsOn);
                        item.GetComponent<UIToggleOdin>().Interactable = false;
                    }
                }
                else
                {
                    foreach (var item in TaskPanelGrid.m_ShowList)
                    {
                        item.GetComponent<UIToggleOdin>().Interactable = true;
                        item.GetComponent<UIToggleOdin>().IsOn = false;
                        item.GetComponent<UIToggleOdin>().ChangeState(ToggleState.UnIsOn);
                    }
                }
                if (TaskPanelGrid.m_ShowList.Count > 0)
                {
                    CurrentSelectedTaskList.Clear();
                    if (TaskPanelGrid.m_ShowList.First().GetComponent<UIToggleOdin>().Interactable &&
                      TaskPanelGrid.m_ShowList.First().GetComponent<UIToggleOdin>().Group.ActiveToggle.Count == 0)
                    {
                        TaskPanelGrid.m_ShowList.First().GetComponent<UIToggleOdin>().IsOn = true;
                    }
                    FirstSelect = false;
                }
            }
        }

        private void AidUpdateCompeleteHandle()
        {
            if (CurrentAidTog != null && CurrentAidTog.GetComponent<AidUpdateBtn>().UpdateCompelete)
            {
                foreach (var item in TaskContent.GetComponent<UIGrid>().m_ShowList)
                {
                    if (item.GetComponent<UIToggleOdin>().Interactable == false)
                    {
                        item.GetComponent<UIToggleOdin>().Interactable = true;
                        item.GetComponent<UIToggleOdin>().SetState(ToggleState.UnIsOn);
                    }
                }
            }
        }
    }
}

