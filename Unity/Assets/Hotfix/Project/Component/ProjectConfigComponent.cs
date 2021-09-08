using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
namespace ETHotfix
{
    [ObjectSystem]
    public class ProjectConfigAwakeComponent : AwakeSystem<ProjectConfigComponent>
    {
        public override void Awake(ProjectConfigComponent self)
        {
            self.Awake();
        }
    }

    public class ProjectConfigComponent : Component
    {
        public static ProjectConfigComponent Instance;

        private ProjectScriptObjectConfig _projectInfo;

        public EditorProjectManager ProjectManger;
        public int CurrentStep;
        internal void Awake()
        {
            Instance = this;
            var go = ResourcesHelper.Load<GameObject>("config", "Config");
            go = go.GetComponent<ReferenceCollector>().Get<GameObject>("OperateConfig");

            ProjectManger = go.GetComponent<ReferenceCollector>().Get<EditorProjectManager>("EditorProjectManager");
            if (ProjectManger == null)
            {
                Debug.LogError("找不到项目数据");
                return;
            }
             var data = XmlConfigHelper.LoadXml((ConfigType)Enum.Parse(typeof(ConfigType), ProjectManger.CurrentProject.name), "Config");
            _projectInfo = new ProjectScriptObjectConfig();
            _projectInfo.Name = data.Xml.SelectSingleNode("Config").Attributes["Name"].Value;
            _projectInfo.Prefab = go.GetComponent<ReferenceCollector>().Get<GameObject>(data.Xml.SelectSingleNode("Config").Attributes["Prefab"].Value);
        }

        public void Load()
        {
            try
            {
                _projectInfo.InitData();
            }
            catch (Exception EX)
            {
                Debug.Log(EX.StackTrace);
            }
        }

        public ProjectScriptObjectConfig GetProjectData()
        {
            return _projectInfo;
        }
        public List<OperateItemScriptObject> TryGetAllMission()
        {
            var operate = _projectInfo.GetData<ExperimentalMethodData>();

            if (operate == null)
            {
                Debug.LogError($"找不到任务数据");
                return null;
            }
            return operate.AllProjectList;
        }

        public List<DisplayInfo> TryGetAllPartData()
        {
            var operate = _projectInfo.GetData<InstrumentUnitData>();

            if (operate == null)
            {
                Debug.LogError($"找不到任务数据");
                return null;
            }
            return operate.DisplayInfos;
        }


        public OperateItemScriptObject TryGetMission(System.Predicate<OperateItemScriptObject> predicate)
        {
            var operate = _projectInfo.GetData<ExperimentalMethodData>();
            if (operate == null)
            {
                Debug.LogError($"找不到任务数据");
                return null;
            }
            return operate.TryGetMission(predicate);
        }

        public List<NoteItem> TryGetNote()
        {
            var operate = _projectInfo.GetData<ExperimentPurposeData>();
            if (operate == null)
            {
                Debug.LogError($"找不到任务数据");
                return null;
            }
            return operate.NoteList;
        }

        public OperateItemScriptObject TryStartProject()
        {
            var operate = _projectInfo.GetData<ExperimentalMethodData>();
            if (operate == null)
            {
                Debug.LogError($"找不到任务数据");
                return null;
            }
            operate.AllProjectList.FirstOrDefault()?.OperateInfo.StartOperate();
            return operate.AllProjectList.FirstOrDefault();
        }

    }
}