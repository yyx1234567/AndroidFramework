using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace ETModel {

    [ObjectSystem]
    public class FreeLookCameraUpdateSystem : UpdateSystem<FreeLookCameraComponent>
    {
        public override void Update(FreeLookCameraComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class FreeLookCameraAwakeSystem : AwakeSystem<FreeLookCameraComponent>
    {
        public override void Awake(FreeLookCameraComponent self)
        {
            self.Awake();
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

        public void Awake()
        {
             //Collider = GameObject.Find("Collider").GetComponent<Collider>();
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

        public void SetInputValue (ViewConfig config)
        {
            Target = GameObject.Find(config.Target).transform;
         }

        private Tween _lastMoveTargetTween;
        public void MoveToTarget(Camera camera , ViewConfig config)
        {
 
            GameObject go =GameObject.Find(config.Target);
            if (go == null)
            {
                Debug.LogError($"找不到对象{config.Target}");
                return;
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

            float time = Vector3.Distance(CurrentCamera.position, endpos) * 0.1f;

            _lastMoveTargetTween = CurrentCamera.DOLocalRotate(new Vector3(InputParameter.SummationY, InputParameter.SummationX), 1);

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
                 IsChangingTarget = false;
            };
        }


        public  void Update()
        {
             if (IsChangingTarget)
            {
                return;
            }

            if (Target == null)
            {
                 return;
            }
            if (Input.touchCount > 0)
            {
                foreach (var item in inputs)
                {
                    if (item.TouchInput())
                    {
                        item.UpdatInput(CurrentCamera, _targetPos.transform, Collider);
                    }
                }
            }
            else
            {
                foreach (var item in inputs)
                {
                    if (item.MouseInput())
                    {
                        item.UpdatInput(CurrentCamera, _targetPos.transform, Collider);
                    }
                }
            }
        }
    }
}
