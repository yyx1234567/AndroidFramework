using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEditor;
using System.Linq;
using System;
using System.Xml;

namespace ETHotfix
{
     public class ExperimentalMethodData : IData
    {
        public string DataName => "实验方法";
        public List<OperateItemScriptObject> AllProjectList = new List<OperateItemScriptObject>();
        public void Init()
        {
            var projectName = ProjectConfigComponent.Instance.ProjectManger.CurrentProject.name;
            var data = XmlConfigHelper.LoadXml((ConfigType)Enum.Parse(typeof(ConfigType), projectName), "Config");
            int index = 1;
 
            foreach (XmlNode item in data.NodeList)
            {
                if (item.Attributes["Name"].Value != DataName)
                {
                    continue;
                }
                foreach (XmlNode node in item.SelectNodes("Item"))
                {
                    OperateItemScriptObject step = XmlConfigHelper.GetXmlObjectData<OperateItemScriptObject>(node);
                    step.OperateInfo = GetXmlOperateData(data,node);
                    step.OperateInfo.Index = index;
                    step.OperateInfo.DataInfo = step;
                    step.StepIndex = index;
                     AllProjectList.Add(step);
                    index++;
                }
            }
         }
        /// <summary>
        /// 反射实例化类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static OperateBase GetXmlOperateData(XmlData data, XmlNode item)
        {


            var instance = System.Activator.CreateInstance(Type.GetType($"ETHotfix.{item.Attributes["Operate"].Value}"));

            return (OperateBase)instance;
        }
        public OperateItemScriptObject TryGetMission(System.Predicate<OperateItemScriptObject> predicate)
        {
            var operate = AllProjectList.Select(x => x).Where(x => predicate.Invoke(x)).FirstOrDefault();
            if (operate == null)
            {
                Debug.LogError($"找不到任务");
                return null;
            }
            return operate;
        }
    }
}