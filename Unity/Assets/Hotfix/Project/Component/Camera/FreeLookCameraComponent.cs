using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class FreeLookCameraUpdateSystem : UpdateSystem<FreeLookCameraComponent>
    {
        public override void Update(FreeLookCameraComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class FreeLookCameraStartSystem : StartSystem<FreeLookCameraComponent>
    {
        public override void Start(FreeLookCameraComponent self)
        {
            self.Start();
        }
    }

    public class FreeLookCameraComponent : Component
    {
        public Transform Target;

        public Collider Collider;

        private Transform _targetPos;

        public Transform CurrentCamera;

        private GameObject CameraRoot;

        public bool IsChangingTarget;

        private List<IInputControl> inputs = new List<IInputControl>();

        public void Start()
        {
            Collider = SceneUnitHelper.Get("BoundingBox")?.GetComponent<Collider>();
            _targetPos = new GameObject("[TargetPosition]").transform;
            _targetPos.SetParent(GameObject.transform);
            inputs.Add(new MoveInput());
            inputs.Add(new RotateInput());
            inputs.Add(new ZoomInput());
        }

        public void SetCamera(Camera camera)
        {
            CurrentCamera = camera.transform;
            if (CameraRoot == null)
            {
                GameObject go = new GameObject("[CameraRoot]");
                var parent = camera.transform.parent;
                camera.transform.SetParent(go.transform);
                go.transform.SetParent(parent);
                camera.transform.localEulerAngles = Vector3.zero;
                camera.transform.localPosition = Vector3.zero;
                CameraRoot = go;
            }
        }

        public void SetInputValue(ViewConfig config)
        {
            Target = SceneUnitHelper.Get(config.Target).transform;
        }

        private Tween _lastMoveTargetTween;
        public void MoveToTarget(Camera camera, ViewConfig config)
        {
             GameObject go = SceneUnitHelper.Get(config.Target);
            if (go == null)
            {
                Debug.LogError($"找不到对象{config.Target}");
                return;
            }
            if (go.transform.Find($"{config.Target}_Camera")!=null)
            {
                go = go.transform.Find($"{config.Target}_Camera").gameObject;
            }
            SetCamera(camera);

            Target = go.transform;
            _targetPos.transform.position = Target.position;
            _targetPos.transform.rotation = Target.rotation;

            InputParameter.SetParameter(config);

            _lastMoveTargetTween.Kill();

            IsChangingTarget = true;

            SetInputValue(config);

            Vector3 initAngle = CurrentCamera.localEulerAngles;


            CurrentCamera.localEulerAngles = new Vector3(InputParameter.SummationY, InputParameter.SummationX);

            var endpos = CurrentCamera.rotation * new Vector3(0.0f, 0.0f, -InputParameter.SummationZ) + _targetPos.transform.position;

            CurrentCamera.localEulerAngles = initAngle;

            float time = Vector3.Distance(CurrentCamera.position, endpos);
            time = Mathf.Clamp(time,0.3f,0.5f);
            _lastMoveTargetTween = CurrentCamera.DOLocalRotate(new Vector3(InputParameter.SummationY, InputParameter.SummationX), time);

            _lastMoveTargetTween = CurrentCamera.DOMove(endpos, time);

            _lastMoveTargetTween.onComplete += () =>
           {
               if (CurrentCamera.parent.parent != null)
               {
                   var _dragTarget = CurrentCamera.parent.parent;
                   CurrentCamera.parent.SetParent(null);
                   _dragTarget.transform.position = CurrentCamera.position;
                   _dragTarget.transform.localEulerAngles = new Vector3(0, CurrentCamera.localEulerAngles.y, 0);
                   CurrentCamera.parent.SetParent(_dragTarget.transform);
               }
               _isOverUI = false;
               IsChangingTarget = false;
           };
        }

        private bool _isOverUI;
        public void Update()
        {
            if (IsChangingTarget)
            {
                return;
            }

            if (Target == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)||Input.mouseScrollDelta!=Vector2.zero)
            {  //排除UI的遮挡（指针在UI上时不做操作）
                if(InputComponent.Instance.IsPointerEnterUI())
                _isOverUI = true;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
            {
                _isOverUI = false;
            }
            if (!_isOverUI)
            {
                if (Input.touchCount > 0)
                {
                    foreach (var item in inputs)
                    {
                        item.TouchInput();
                        item.UpdatInput(CurrentCamera, _targetPos.transform, Collider);
                    }
                }
                else
                {
                    foreach (var item in inputs)
                    {
                        item.MouseInput();
                        item.UpdatInput(CurrentCamera, _targetPos.transform, Collider);
                    }
                }
            }
            else
            {
                foreach (var item in inputs)
                {
                    item.Reset();
                 }
            }
         }
    }
}
