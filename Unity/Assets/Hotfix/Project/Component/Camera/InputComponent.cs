using ETModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETHotfix
{
    [ObjectSystem]
    public class InputComponentAwakeSystem : AwakeSystem<InputComponent>
    {
        public override void Awake(InputComponent self)
        {
            self.Awake();
        }
    }

    public class InputComponent : Component
    {
        public Vector2 TranslationDelta => CalculateTranslationDelta();
        public Vector2 RotationDelta => CalculateRotationDelta();
        public float ScrollDelta => CalculateScrollDelta();

        private Vector2 translationDelta;
        private Vector2 rotationDelta;
        private float previousScrollDistance;
        private float currentScrollDistance;
        private float scrollDelta;

        private const int uiLayer = 5;
        private AdvancedStandaloneInputModule inputModule;
        private UnityEngine.EventSystems.EventSystem unityEventSystem;

        public bool IsEnabled { get; private set; }

        public static InputComponent Instance;
        public void Awake()
        {
            Instance = this;
            this.inputModule = this.GameObject.AddComponent<UnityEngine.EventSystems.AdvancedStandaloneInputModule>();
            this.unityEventSystem = UnityEngine.EventSystems.EventSystem.current;
            this.IsEnabled = true;
        }

        public void Activate()
        {
            this.unityEventSystem.enabled = true;
            this.IsEnabled = true;
        }

        public void Deactivate()
        {
            this.unityEventSystem.enabled = false;
            this.IsEnabled = false;
        }

        /// <summary>
        /// 是否有指针悬停在UI层物体上
        /// </summary>
        /// <returns></returns>
        public bool IsPointerEnterUI()
        {
            foreach (PointerEventData data in this.inputModule.PointerData.Values)
            {
                if (data != null && data.pointerEnter != null && data.pointerEnter.layer == uiLayer)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否有指针拖拽UI层物体
        /// </summary>
        /// <returns></returns>
        public bool IsPointerDragUI()
        {
            foreach (PointerEventData data in this.inputModule.PointerData.Values)
            {
                if (data != null && data.pointerDrag != null && data.pointerDrag.layer == uiLayer)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算旋转增量
        /// </summary>
        /// <returns></returns>
        private Vector2 CalculateRotationDelta()
        {
            if (!this.IsEnabled)
            {
                rotationDelta = Vector2.zero;
            }
            else
            {
                if (Input.touchCount == 1)
                {
                    rotationDelta = Input.GetTouch(0).deltaPosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    rotationDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
                else
                {
                    rotationDelta = Vector2.zero;
                }
            }
            return rotationDelta;
        }

        /// <summary>
        /// 计算缩放增量
        /// </summary>
        /// <returns></returns>
        private float CalculateScrollDelta()
        {
            if (!this.IsEnabled)
            {
                scrollDelta = 0;
                previousScrollDistance = currentScrollDistance;
            }
            else
            {
                if (Input.touchCount == 2)
                {
                    currentScrollDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                    if (previousScrollDistance == 0)
                    {
                        previousScrollDistance = currentScrollDistance;
                    }
                    scrollDelta = (currentScrollDistance - previousScrollDistance);
                    previousScrollDistance = currentScrollDistance;
                }
                else
                {
                    scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                }
            }
            return scrollDelta;
        }

        /// <summary>
        /// 计算平移增量
        /// </summary>
        /// <returns></returns>
        private Vector2 CalculateTranslationDelta()
        {
            if (!this.IsEnabled)
            {
                translationDelta = Vector2.zero;
            }
            else
            {
                if (Input.touchCount == 3)
                {
                    translationDelta = Vector2.zero;
                    translationDelta += Input.GetTouch(0).deltaPosition;
                    translationDelta += Input.GetTouch(1).deltaPosition;
                    translationDelta += Input.GetTouch(2).deltaPosition;
                }
                else if (Input.GetMouseButton(2))
                {
                    translationDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                }
                else
                {
                    translationDelta = Vector2.zero;
                }
            }
            return translationDelta;
        }
    }
}