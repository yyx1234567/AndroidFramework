using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
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
 
        public void Awake()
        {
            GenerateCameras();
            GenerateLayers();
            Camera = this.GameObject.transform.Find("[UICamera]").GetComponent<Camera>();
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
        public void Add(UI ui)
        {
            ui.GameObject.GetComponent<Canvas>().worldCamera = this.Camera.GetComponent<Camera>();

            this.uis.Add(ui.Name, ui);
            ui.Parent = this;
        }
        public void ResetHierarchy(string name)
        {
            Get(name).GameObject.transform.SetParent(this.layers[this.hierarchys[name]].transform, false);
        }

        public void UpdateHierarchy(string name, string layer)
        {
            Get(name).GameObject.transform.SetParent(this.layers[layer].transform, false);
            Get(name).GameObject.GetComponent<Canvas>().sortingOrder = this.layers[layer].transform.GetSiblingIndex();
        }

        #region 初始化

        /// <summary>
        /// 创建UI相机
        /// </summary>
        private  void GenerateCameras()
        {
            Camera uiCamera = new GameObject("[UICamera]")
                .AddComponent<Camera>();
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = 1 << LayerMask.NameToLayer(LayerNames.UI);
            uiCamera.orthographicSize = Screen.height / 2;
            uiCamera.allowHDR = false;
            uiCamera.allowMSAA = false;
            uiCamera.orthographic = true;
            uiCamera.depth = 1;
            uiCamera.transform.SetParent(this.GameObject.transform, false);
          }

        /// <summary>
        /// 创建UI层级
        /// </summary>
        private void GenerateLayers()
        {
            foreach (HierarchyType item in System.Enum.GetValues(typeof(HierarchyType)))
            {
                layers[item.ToString()] = new GameObject(item.ToString());
                layers[item.ToString()].transform.SetParent(GameObject.transform);
                layers[item.ToString()].layer = LayerNames.GetLayerInt(LayerNames.UI);
            }
        }

        #endregion 初始化
    }
}