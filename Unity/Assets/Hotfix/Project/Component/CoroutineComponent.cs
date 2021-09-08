using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETHotfix
{
    [ObjectSystem]
    public class CoroutineAwakeSystem : AwakeSystem<CoroutineComponent>
    {
        public override void Awake(CoroutineComponent self)
        {
            GameObject go = new GameObject("CorountineItem");
            go.transform.SetParent(SceneUnitHelper.Get("Global").transform);
            self.CoroutineObject = go.AddComponent<CorountineItem>();
            self.Awake();
        }
    }
    public class CoroutineComponent : Component
    {
        public CorountineItem CoroutineObject;

        public static CoroutineComponent Instance;

        public Dictionary<string, IEnumerator> ItemDic = new Dictionary<string, IEnumerator>();

        public void Awake()
        {
            Instance = this;
        }

        public void StartCoroutineWithID(IEnumerator func, string iD)
        {
            if (ItemDic.ContainsKey(iD))
            {
                CoroutineObject.StopCoroutine(ItemDic[iD]);
                CoroutineObject.StartCoroutine(func);
            }
            else
            {
                ItemDic.Add(iD, func);
                CoroutineObject.StartCoroutine(func);
            }

        }
        public IEnumerator StartCoroutine(IEnumerator id)
        {
             yield return  CoroutineObject.StartCoroutine(id);
        }

        public void StartCoroutineVoid(IEnumerator id)
        { 
            CoroutineObject.StartCoroutine(id);
        }
        public void StopCorountineWithID(string id)
        {
            if (ItemDic.ContainsKey(id))
            {
                CoroutineObject.StopCoroutine(ItemDic[id]);
                ItemDic.Remove(id);
            }
        }
    }
}
