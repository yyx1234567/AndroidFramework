using ETModel;
using LinqUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETHotfix
{
    public class UIHelper
    {
        /// <summary>
        /// 获取或创建UI,默认关闭
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="active">ui打开或关闭</param>
        /// <returns></returns>
        public static T GetUI<T>(string name = "") where T : UIWindowComponent, new()
        {
            name = GetUIName<T>(name);
            var uiComponent = Game.Scene.GetComponent<UIComponent>() ?? Game.Scene.AddComponent<UIComponent>();
            T ui = uiComponent.Get(name)?.GetComponent(typeof(T)) as T ?? uiComponent.CreateUI<T>(name);
            return ui;
        }

        public static T OpenUI<T>(string name = "") where T : UIWindowComponent, new()
        {
            name = GetUIName<T>(name);
            var uiComponent = Game.Scene.GetComponent<UIComponent>() ?? Game.Scene.AddComponent<UIComponent>();
            T ui = uiComponent.Get(name)?.GetComponent(typeof(T)) as T ?? uiComponent.CreateUI<T>(name);
            ui.SetActive(true);
            return ui;
        }

        public static T CloseUI<T>(string name = "") where T : UIWindowComponent, new()
        {
            name = GetUIName<T>(name);
            var uiComponent = Game.Scene.GetComponent<UIComponent>() ?? Game.Scene.AddComponent<UIComponent>();
            T ui = uiComponent.Get(name)?.GetComponent(typeof(T)) as T ?? uiComponent.CreateUI<T>(name);
            ui.SetActive(false);
            return ui;
        }

        private static string GetUIName<T>(string name)
        {
            return string.IsNullOrEmpty(name) ? $"{typeof(T).Name.Replace("Component", string.Empty)}" : name;
        }

        public static void DestroyUI(string UIName)
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Get(UIName);
            if (ui != null)
            {
                ui.Dispose();
            }
        }
        private static List<string> m_OperateList = new List<string>();

        public static void OperateUI(string Target, Action callBack)
        {
            if (m_OperateList.Count == 0)
            {
                return;
            }
            if (m_OperateList.FirstOrDefault() != Target)
                return;
            m_OperateList.Remove(m_OperateList[0]);
            callBack.Invoke();
        }

        //private static async ETTask<T> CreateUIAsync<T>(string name) where T : Component, new()
        //{
        //    try
        //    {
        //        GameObject prefab = await ResourcesHelper.LoadAsync<GameObject>(name);
        //        GameObject go = UnityEngine.Object.Instantiate(prefab);
        //        go.layer = LayerMask.NameToLayer(LayerNames.UI);
        //        UI ui = ComponentFactory.Create<UI, string, GameObject>(name, go);
        //        Game.Scene.GetComponent<UIComponent>().Add(ui);
        //        T component = ui.AddComponent<T>();
        //        return component;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log(ex.Message);
        //    }
        //    return null;
        //}


    }
}