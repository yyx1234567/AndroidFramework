using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ETModel;
using System.Threading.Tasks;

namespace ETModel
{
    public class ScriptObjectConfigComponent:Component
    {
        private Dictionary<string, ScriptObjectConfig> allConfigCategory = new Dictionary<string, ScriptObjectConfig>();
        private Dictionary<string, ScriptObjectConfig> allCloneConfigCategory = new Dictionary<string, ScriptObjectConfig>();

        public async ETTask LoadAsync()
        {
            try
            {
                this.allConfigCategory.Clear();
                this.allCloneConfigCategory.Clear();
                var go = await ResourcesHelper.LoadAsync<GameObject>("config", "Config");
                go = go.GetComponent<ReferenceCollector>().Get<GameObject>("DataConfig");
                foreach (var item in go.GetComponent<ReferenceCollector>().data)
                {
                    allConfigCategory.Add(item.key, (item.gameObject as ScriptObjectConfig));
                    if (item.gameObject as ScriptObjectConfig == null)
                        continue;
                    allCloneConfigCategory.Add(item.key, GameObject.Instantiate(item.gameObject as ScriptObjectConfig));
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.StackTrace);
            }
        }

        public virtual IEnumerable<T> GetAll<T>() where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category?.ItemList.OfType<T>().ToArray();
        }

        public virtual IEnumerable<T> GetAllDynamicData<T>() where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allCloneConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category?.ItemList.OfType<T>().ToArray();
        }

        public T TryGetDynamicData<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allCloneConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category.ItemList.OfType<T>().Select(x => x).Where(x => predicate(x)).FirstOrDefault();
        }


        public virtual T[] TryGetAll<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category?.ItemList.OfType<T>().Where(x => predicate(x)).ToArray();

        }
        public T TryGet<T>() where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return (T)category.ItemList.OfType<T>().Select(x => x);
        }

        public T TryGet<T>(Predicate<T> predicate) where T : ETModel.ScriptObjectBaseConfig
        {
            if (!this.allConfigCategory.TryGetValue(typeof(T).Name, out ETModel.ScriptObjectConfig category))
            {
                throw new Exception($"AddressableConfigComponent not found key: {typeof(T).FullName}");
            }
            return category.ItemList.OfType<T>().Select(x => x).Where(x => predicate(x)).FirstOrDefault();
        }


    }
}