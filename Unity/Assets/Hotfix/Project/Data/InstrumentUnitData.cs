using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.Xml;
using System;

namespace ETHotfix
{
    public class InstrumentUnitData : IData
    {
        public string DataName => "实验器材";

        public List<DisplayInfo> DisplayInfos = new List<DisplayInfo>();

        public void Init()
        {
            var projectName = ProjectConfigComponent.Instance.ProjectManger.CurrentProject.name;
            var data = XmlConfigHelper.LoadXml((ConfigType)Enum.Parse(typeof(ConfigType), projectName), "Config");
            foreach (XmlNode item in data.NodeList)
            {
                if (item.Attributes["Name"].Value != DataName)
                {
                    continue;
                }
                foreach (XmlNode node in item.SelectNodes("Item"))
                {
                    DisplayInfo noteItem = XmlConfigHelper.GetXmlObjectData<DisplayInfo>(node);
                    DisplayInfos.Add(noteItem);
                }
            }
        }
    }

    public class DisplayInfo  
    {
        public int StepIndex;

        public string Name;

        public string Target;

        public string ViewID;

        public string Content;

        public AudioClip Audio;
 
    }
}