 using UnityEngine;
namespace ETModel
{
    [ObjectSystem]
    public class DisplayUpdateComponent : UpdateSystem<DisplayComponent>
    {
        public override void Update(DisplayComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class DisplayAwakeComponent : AwakeSystem<DisplayComponent>
    {
        public override void Awake(DisplayComponent self)
        {
            self.Awake();
        }
    }

    public class DisplayComponent : Component
    {
        #region parameter
        public GameObject Target;

        public float Speed;

        public float NewX;

        public float NewY;

        public float NewZ;

        public float SummationX;

        public float SummationY;

        public float SummationZ;

        public Vector2 NewPos;

        private float ScaledValue;

        private float MaxScale;
        private float MinScale;
        #endregion
        public Camera DisplayCamera;


        public void Awake()
        {
            var go = new GameObject("[DisplayCamera]");
            go.transform.SetParent(GameObject.transform);
            DisplayCamera = go.AddComponent<Camera>();
            DisplayCamera.clearFlags = CameraClearFlags.Depth;
            DisplayCamera.cullingMask = LayerMask.NameToLayer("Display");
         }
        public void SetTarget(GameObject target)
        {
            Target = target;
            MaxScale = target.transform.localScale.x * 3;
            MinScale = target.transform.localScale.x * 0.5f;
            SummationX = -45;
            SummationY = 45;
            var x = -45 - Target.transform.eulerAngles.x;
            var y = 45 - Target.transform.eulerAngles.y;
            var z = 0 - Target.transform.eulerAngles.z;

            Target.transform.Rotate(new Vector3(x, 0, 0), Space.World);
            Target.transform.Rotate(new Vector3(0, y, z), Space.Self);
        }

        public void Update()
        {
            if (Target == null)
                return;
            if (Input.touchCount > 0)
            {
                TouchOperate();
            }
            else
            {
                MouseOperate();
            }
            UpdateTargetState();
        }

        public void QuitDisPlay()
        {
            GameObject.DestroyImmediate(Target);
        }

        public void DisplayTarget(GameObject gameObject)
        {
            var go = GameObject.Instantiate(gameObject);
            go.transform.SetParent(GameObject.transform);
            go.transform.localPosition = Vector3.forward * 3;
            go.layer = LayerMask.NameToLayer("Display");
            SetTarget(go);
        }

        private void UpdateTargetState()
        {
            NewX = Mathf.Lerp(NewX, 0, Time.deltaTime * 10);
            NewY = Mathf.Lerp(NewY, 0, Time.deltaTime * 10);
            NewZ = Mathf.Lerp(NewZ, 0, Time.deltaTime * 10);
            SummationY += NewX;
            NewPos = Vector2.Lerp(NewPos, Vector2.zero, Time.deltaTime * 30);

            if (SummationY > 180 || SummationY < 0)
            {
                SummationY = Mathf.Clamp(SummationY, 0, 180);
                NewX = 0;
            }
            Target.transform.Rotate(new Vector3(NewX, 0, 0), Space.World);
            Target.transform.Rotate(new Vector3(0, NewY, 0), Space.Self);

            //Target.transform.eulerAngles = new Vector3(Mathf.Clamp(Target.transform.eulerAngles.x, -90, 90), Target.transform.eulerAngles.y, Target.transform.localEulerAngles.z);
            Vector3 pos = Target.transform.position + new Vector3(NewPos.x, NewPos.y, 0);
            if (DisplayCamera.WorldToViewportPoint(pos).x < 0 || DisplayCamera.WorldToViewportPoint(pos).x > 1)
            {
                NewPos.x = 0;
            }
            if (DisplayCamera.WorldToViewportPoint(pos).y < 0 || DisplayCamera.WorldToViewportPoint(pos).y > 1)
            {
                NewPos.y = 0;
            }


            Target.transform.position += new Vector3(NewPos.x, NewPos.y, 0);

            if (Target.transform.localScale.x + NewZ > MaxScale || Target.transform.localScale.x + NewZ < MinScale)
            {
                return;
            }
            Target.transform.localScale += Vector3.one * NewZ;

        }

        private void MouseOperate()
        {
            if (Input.GetMouseButton(0))
            {
                RotateOperate(Input.mousePosition);
            }
            else
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                ScaleOperate(Input.mouseScrollDelta);
            }
            else
            if (Input.GetMouseButton(2))
            {
                MoveOperate(Input.mousePosition);
            }
            else
            {
                ResetTouch();
            }
        }

        private void ScaleOperate(Vector2 mouseScrollDelta)
        {
            NewZ = mouseScrollDelta.y * 0.05F;
        }

        private int lastCount = 0;
        private void TouchOperate()
        {
            if (lastCount != Input.touchCount)
            {
                ResetTouch();
            }
            lastCount = Input.touchCount;
            if (Input.touchCount == 1)
            {
                RotateOperate(Input.GetTouch(0).position);
            }
            else
            if (Input.touchCount == 2)
            {
                ScaleOperate(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            else
           if (Input.touchCount == 3)
            {
                MoveOperate(Input.GetTouch(0).position);
            }
        }

        private Vector2 _lastPos;
        private void MoveOperate(Vector2 position1)
        {
            if (_lastPos == Vector2.zero)
            {
                _lastPos = position1;
            }
            Vector2 pos = position1 - _lastPos;
            NewPos.x = pos.x * 0.01f;
            NewPos.y = pos.y * 0.01f;
            _lastPos = position1;
        }

        private Vector2 _lastRotatePos;
        private void RotateOperate(Vector2 position)
        {
            if (_lastRotatePos == Vector2.zero)
            {
                _lastRotatePos = position;
            }
            Vector2 pos = _lastRotatePos - position;
            NewX = -pos.y * 0.2f;
            NewY = pos.x * 0.2f;
            _lastRotatePos = position;
        }

        private float _lastDistance;
        public void ScaleOperate(Vector2 point1, Vector2 point2)
        {
            float distance = Vector2.Distance(point1, point2);
            if (_lastDistance == 0)
            {
                _lastDistance = distance;
            }
            float delta = _lastDistance - distance;
            NewZ = -delta * 0.001F;

            _lastDistance = distance;
        }

        private void ResetTouch()
        {
            _lastRotatePos = Vector2.zero;
            _lastPos = Vector2.zero;
            _lastDistance = 0;
        }
    }
}
