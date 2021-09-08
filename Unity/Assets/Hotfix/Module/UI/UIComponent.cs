using ETModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIComponentAwakeSystem : AwakeSystem<UIComponent>
    {
        public override void Awake(UIComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIComponent : Component
    {
        public Camera Camera;

        private Dictionary<string, GameObject> layers = new Dictionary<string, GameObject>();
        private Dictionary<string, string> hierarchys = new Dictionary<string, string>();
        private Dictionary<string, UI> uis = new Dictionary<string, UI>();
        private Dictionary<UIWindowComponent, HierarchyType> hierarchySorting = new Dictionary<UIWindowComponent, HierarchyType>();
        private Dictionary<HierarchyType, int> currentIndexDic = new Dictionary<HierarchyType, int>();
        public void Awake()
        {
            GenerateLayers();
            Camera = ETModel.Game.Scene.GetComponent<ETModel.UIComponent>().Camera;
        }


        public void Remove(string uiName)
        {
            if (!this.uis.TryGetValue(uiName, out UI ui))
            {
                return;
            }
            this.uis.Remove(uiName);
            ui.Dispose();
        }

        public void Add(UI ui)
        {
            ui.GameObject.GetComponent<Canvas>().worldCamera = this.Camera.GetComponent<Camera>();
            this.uis.Add(ui.Name, ui);
            ui.Parent = this;
        }

        public UI Get(string name)
        {
            this.uis.TryGetValue(name, out UI ui);
            return ui;
        }

        public T Get<T>(string name) where T : Component
        {
            this.uis.TryGetValue(name, out UI ui);
            return ui?.GetComponent<T>();
        }



        public T CreateUI<T>(string name) where T : UIWindowComponent, new()
        {
            var config = ETModel.Game.Scene.GetComponent<ETModel.ScriptObjectConfigComponent>().TryGet<UIConfig>(x => x.Name == name.StringToAsset());
            GameObject go = ResourcesHelper.Load<GameObject>("uiwindow", name.StringToAsset());
            go = GameObject.Instantiate(go);
            go.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = ComponentFactory.Create<UI, string, GameObject>(name, go);
            ui.Canvas.worldCamera = this.Camera;
            Add(ui);
            T component = ui.AddComponent<T>();
            if (!hierarchySorting.ContainsKey(component))
            {
                hierarchySorting.Add(component, config.Hierarchy);
            }
            ui.Canvas.sortingOrder = currentIndexDic[config.Hierarchy];
            UpdateHierarchy(name, config.Hierarchy.ToString());
            return component;
        }


        public void ResetHierarchy(string name)
        {
            Get(name).GameObject.transform.SetParent(this.layers[this.hierarchys[name]].transform, false);
        }

        public void UpdateHierarchy(string name, string layer)
        {
            Get(name).GameObject.transform.SetParent(this.layers[layer].transform, false);
        }
        public void UpdateSortingOrder(UIWindowComponent component)
        {
            hierarchySorting.TryGetValue(component, out HierarchyType layer);

            currentIndexDic[layer]++;

            component.m_Canvas.sortingOrder = currentIndexDic[layer];
        }


        #region 初始化

        /// <summary>
        /// 创建UI层级
        /// </summary>
        private void GenerateLayers()
        {
            foreach (HierarchyType item in System.Enum.GetValues(typeof(HierarchyType)))
            {
                currentIndexDic.Add(item, (int)item);
                layers[item.ToString()] = new GameObject(item.ToString());
                layers[item.ToString()].transform.SetParent(GameObject.transform);
                layers[item.ToString()].layer = LayerNames.GetLayerInt(LayerNames.UI);
            }
        }

        #endregion 初始化
    }
}