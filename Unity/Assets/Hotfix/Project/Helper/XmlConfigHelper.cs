using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Linq;

namespace ETHotfix
{
    public enum ConfigType
    {
        ViewData,
        SceneUnitData,
        UIConfig,
        Project_001,
        Project_002,
        Project_003,
        Project_004,
    }



    public class XmlData
    {
        public XmlNodeList NodeList;
        public XmlDocument Xml;
    }

    public class XmlConfigHelper
    {
        /// <summary>
        /// 反射实例化类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T GetXmlObjectData<T>(XmlNode item)
        {
            var instance = System.Activator.CreateInstance(typeof(T));
            for (int i = 0; i < item.Attributes.Count; i++)
            {
                foreach (var field in typeof(T).GetFields())
                {
                    if (field.Name == item.Attributes[i].Name)
                    {
                        field.SetValue(instance, item.Attributes[i].Value);
                    }
                }
                foreach (var properties in typeof(T).GetProperties())
                {
                    if (properties.Name == item.Attributes[i].Name)
                    {
                        properties.SetValue(instance, item.Attributes[i].Value);
                    }
                }
            }
            return (T)instance;
        }
        public static T GetXmlData<T>(XmlNode item, string script)
        {
            var type = Type.GetType($"ETHotfix.{script}");
            var instance = System.Activator.CreateInstance(type);
            for (int i = 0; i < item.Attributes.Count; i++)
            {
                foreach (var field in type.GetFields())
                {
                    if (field.Name == item.Attributes[i].Name)
                    {
                        field.SetValue(instance, item.Attributes[i].Value);
                    }
                }
                foreach (var properties in type.GetProperties())
                {
                    if (properties.Name == item.Attributes[i].Name)
                    {
                        properties.SetValue(instance, item.Attributes[i].Value);
                    }
                }
            }
            return (T)instance;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static XmlData LoadXml(ConfigType type, string rootname = "Config")
        {
            var xmldata = new XmlData();
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = true;
            XmlDocument _xml = new XmlDocument();
            TextAsset text = GetTextAssetAsync(type);
            XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), set);
            _xml.Load(reader);
            xmldata.NodeList = _xml.SelectSingleNode(rootname).ChildNodes;
            xmldata.Xml = _xml;
            return xmldata;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static XmlNodeList LoadXml(TextAsset text, string rootname = "Config")
        {
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = true;
            XmlDocument _xml = new XmlDocument();
            XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), set);
            _xml.Load(reader);
            return _xml.SelectSingleNode(rootname).ChildNodes;
        }


        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static XmlNodeList LoadXml(TextAsset text, out XmlDocument _xml, string rootname = "Config")
        {
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = true;
            _xml = new XmlDocument();
            XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), set);
            _xml.Load(reader);
            return _xml.SelectSingleNode(rootname).ChildNodes;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async ETModel.ETTask<IEnumerable<XElement>> LoadXmlLinq(ConfigType type, string rootname = "Config")
        {
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = true;
            TextAsset text = GetTextAssetAsync(type);
            XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), set);
            XDocument _xml = XDocument.Load(reader);
            return _xml.Descendants(rootname);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> LoadXmlLinq(TextAsset text, out XDocument _xml, bool ignore = true, string rootname = "Config")
        {
            XmlReaderSettings set = new XmlReaderSettings();
            set.IgnoreComments = ignore;
            XmlReader reader = XmlReader.Create(new MemoryStream(text.bytes), set);
            _xml = XDocument.Load(reader);
            return _xml.Descendants(rootname);
        }

        private static TextAsset GetTextAssetAsync(ConfigType type)
        {
            if (!Application.isPlaying)
            {
                var result = File.ReadAllText($"Assets/Res/Config/OperateConfig/{type.ToString()}.xml");
                TextAsset text = new TextAsset(result);
                return text;
            }
            else
            {
                try
                {
                    var result = ETModel.Game.Scene.GetComponent<ETModel.ResourcesComponent>().GetAsset<GameObject>("config", "Config");
                    var xml = result.GetComponent<ReferenceCollector>().Get<GameObject>("XmlConfig");
                    TextAsset configStr = xml.Get<TextAsset>(type.ToString());
                    return configStr;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.StackTrace);
                }
            }
            return new TextAsset();
        }

    }
}