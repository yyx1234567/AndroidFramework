using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ETModel;
using System.Threading.Tasks;
using System.Xml;

namespace ETHotfix
{
    public class ScriptObjectConfigComponent : Component
    {
        private Dictionary<string, XmlObjectConfig> allConfigCategory = new Dictionary<string, XmlObjectConfig>();

        public async ETTask LoadAsync()
        {
            var projectName = ProjectConfigComponent.Instance.ProjectManger.CurrentProject.name;
            var data = XmlConfigHelper.LoadXml((ConfigType)Enum.Parse(typeof(ConfigType), projectName), "Config");
            this.allConfigCategory.Clear();
            var go = await ResourcesHelper.LoadAsync<GameObject>("config", "Config");
            go = go.GetComponent<ReferenceCollector>().Get<GameObject>("DataConfig");
            foreach (var item in go.GetComponent<ReferenceCollector>().data)
            {
                foreach (XmlNode node in data.NodeList)
                {
                    if (node.Attributes["Script"].Value == item.key)
                    {
                        XmlObjectConfig xmlObjectConfig = XmlConfigHelper.GetXmlObjectData<XmlObjectConfig>(node);
                        xmlObjectConfig.ItemList = new List<BaseConfig>();
                        foreach (XmlNode config in node.SelectNodes("Item"))
                        {
                            var configitem = GetXmlOperateData<BaseConfig>(config, node.Attributes["Script"].Value);

                            xmlObjectConfig.ItemList.Add(configitem);
                        }
                        allConfigCategory.Add(item.key, xmlObjectConfig);
                        break;
                    }
                }
            }
        }
        public static T GetXmlOperateData<T>(XmlNode item, string script)
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
        public virtual IEnumerable<T> GetAll<T>() where T : BaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out XmlObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category?.ItemList.OfType<T>().ToArray();
        }

        public virtual T[] TryGetAll<T>(Predicate<T> predicate) where T : BaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out XmlObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category?.ItemList.OfType<T>().Where(x => predicate(x)).ToArray();

        }
        public T TryGet<T>() where T : BaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out XmlObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return (T)category.ItemList.OfType<T>().Select(x => x);
        }

        public T TryGet<T>(Predicate<T> predicate) where T : BaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out XmlObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category.ItemList.OfType<T>().Select(x => x).Where(x => predicate(x)).FirstOrDefault();
        }
    }
}