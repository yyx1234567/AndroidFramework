using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
namespace ETHotfix
{
    public class NoteItem
    {
        public string Title;
        public string Content;
    }
    public class ExperimentPurposeData : IData
    {
        public string DataName => "实验目的";

        public List<NoteItem> NoteList = new List<NoteItem>();
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
                    NoteItem noteItem = XmlConfigHelper.GetXmlObjectData<NoteItem>(node);
                    NoteList.Add(noteItem);
                }
             }
        }


    }
}