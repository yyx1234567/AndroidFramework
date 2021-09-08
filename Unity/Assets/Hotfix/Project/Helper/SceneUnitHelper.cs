using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETHotfix
{
    public static class SceneUnitHelper
    {
        private static Dictionary<string, GameObject> CacheDatas = new Dictionary<string, GameObject>();
        public static GameObject Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            if (CacheDatas == null)
                CacheDatas = new Dictionary<string, GameObject>();
            if (CacheDatas.ContainsKey(name))
            {
                if (CacheDatas[name] != null)
                {
                    return CacheDatas[name];
                }
                CacheDatas.Remove(name);
            }
            var go =GameObject.Find(name);
            if (go == null)
            {
                Debug.LogError($"找不到物体{name}");
            }
            CacheDatas.Add(name,go);
            return go;
        }
    }
}