 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 namespace ETModel {

 
    [ObjectSystem]
    public class FirstPersonPerspectiveUpdateSystem : UpdateSystem<FirstPersonPerspectiveComponent>
    {
        public override void Update(FirstPersonPerspectiveComponent self)
        {
            self.Update();
        }
    }
    [ObjectSystem]
    public class FirstPersonPerspectiveLateUpdateSystem : LateUpdateSystem<FirstPersonPerspectiveComponent>
    {
        public override void LateUpdate(FirstPersonPerspectiveComponent self)
        {
            self.LateUpdate();
        }
    }

    public class FirstPersonPerspectiveComponent : ETModel.Component
    {
        public const int RoleCameraCanPenetrableLayers = 1 << 0 | 1 << 12 | 1 << 2 | 1 << 13 | 1 << 9 | 1 << 14;//

        private float horizontalSpeed = 60f;                 //定义摄像机水平移动速度
        private float verticalSpeed = 35f;                   //摄像机垂直移动速度
        private float MouseCenterSpeed = 2f;                //摄像机中键速度
        private float tempEulerX;                            //限制摄像机上下角度
        public float CameraWorldSpreed = 0.01f;              //摄像机跟随速度
        private float SaveCameraCenter;                      //储存摄像机的中键值
        private float CameraRotateMax = 8.5f;                //最远的摄像机距离
        private float CameraRotateMin = 0.7f;                //最近的摄像机距离
        private float CameraXMin = -59f;                     //最大向下角度
        private float CameraXMax = 10;                       //最大向上角度
        private float Mouse2Distance = 3;                    //鼠标中键滚动了多少
        public float ColliderDistance = 0.2f;               //摄像机与碰撞体之间的最小距离
        public GameObject CameraHand;                        //摄像机父级
        public Transform CameraHandParent;                   //摄像机的横向旋转父级
        private Vector3 Offsetpos;                           //摄像机与人物之间的差值
        private Vector3 RefCameraForWorld;                   //暂存摄像机的缓动值
        private Vector3 RefCameraForWorld2;                  //暂存摄像机的缓动值
        private Vector3 RefCameraForWorld3;                  //暂存摄像机的缓动值
        public GameObject PlayerModel;                       //角色自身
        public bool IsMouse1 = false;                        //鼠标右键点击旋转摄像机
        public bool IsUse = true;                            //是否允许使用摄像机的控制权
        public KeyValuePair<Transform, Transform> mPlayerNav;                         //角色的寻路脚本
        public Vector3 PlayerCamerahandInitPos;              //初始cameraHand的位置
                                                             //通过计算后摄像机上下移动和左右移动的值
        public float Jup;
        public float Jright;
        [HideInInspector]
        public float MouseSpeedX = 3.0f;
        [HideInInspector]
        public float MouseSpeedY = 3.0f;
        private List<Vector3> AllDistance;

        private void Awake(GameObject camera)
        {
            GameObject = camera.transform.Find("CameraHand/CameraPos").gameObject;
            CameraHand = GameObject.transform.parent.gameObject;
            CameraHandParent = GameObject.transform.parent.parent;
            AllDistance = new List<Vector3>();
            for (float i = 0; i < ColliderDistance; i += 0.01f)
            {
                if (i < 0.2f)
                {
                    AllDistance.Add(new Vector3(0, i + 0.1f, 0));
                }
                else
                {
                    AllDistance.Add(new Vector3(0, i, 0));
                }
              }
          }
 
        public void Update()
        {
            if (!IsUse)
            {
                return;
            }

            if (null == PlayerModel)
                return;
            //CalculateCameraDistance();
            if (!IsUse)
            {
                return;
            }

            if (null == PlayerModel)
                return;
            //获取人物的欧拉角
            Vector3 TempModleEuluer = PlayerModel.transform.eulerAngles;
            PlayerModel.transform.eulerAngles = TempModleEuluer;
            CameraHand.transform.position = Vector3.SmoothDamp(CameraHand.transform.position, mPlayerNav.Value.position, ref RefCameraForWorld2, CameraWorldSpreed);
            CameraHandParent.transform.position = Vector3.SmoothDamp(CameraHandParent.transform.position, mPlayerNav.Key.position, ref RefCameraForWorld3, CameraWorldSpreed);

            //相机是否可以移动
            IsCameraMove();
            //寻路
         }
        public void LateUpdate()
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,GameObject.transform.position, ref RefCameraForWorld, CameraWorldSpreed);
            Camera.main.transform.LookAt(CameraHand.transform);
        }
        /// <summary>
        /// 初始化摄像机系统
        /// </summary>
        /// <param name="thePlayerModel"></param>
        public void InitValue(Transform player, GameObject camera)
        {
            Awake(camera);
            this.mPlayerNav = new KeyValuePair<Transform, Transform>(player, player.transform.Find("CameraHand"));
            this.PlayerModel = player.gameObject;
            Camera.main.transform.position =GameObject.transform.position;
            CameraHand.transform.position = mPlayerNav.Value.position;
            CameraHandParent.transform.position = mPlayerNav.Key.position;
            Camera.main.transform.LookAt(CameraHand.transform);
            PlayerCamerahandInitPos = mPlayerNav.Value.localPosition;
            Offsetpos = GameObject.transform.position - CameraHand.transform.position;
         }
        /// <summary>
        /// 设置摄像机是否可以移动
        /// </summary>
        private void SetCameraMove()
        {
            IsMouse1 = Input.GetMouseButton(1);
        }
        /// <summary>
        /// 设置相机移动
        /// </summary>
        private void IsCameraMove()
        {
            //设置相机的移动bool
            SetCameraMove();
            //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}
            //相机缩小
            CalculateCameraCenter();
            //相机旋转
            CalculateCameraRotate();
        }
        //计算鼠标中键调整摄像机的远近
        public void CalculateCameraCenter()
        {
             if (Input.GetAxis("Mouse ScrollWheel") == 0)
            {
                return;
            }
            float distance = Offsetpos.magnitude;
            distance -= Input.GetAxis("Mouse ScrollWheel") * MouseCenterSpeed;
            distance = Mathf.Clamp(distance, CameraRotateMin, CameraRotateMax);
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Mouse2Distance = distance;
            }
            Offsetpos = Offsetpos.normalized * distance;
        }
        //计算摄像机在鼠标操作时的方法
        public void CalculateCameraMove()
        {
            //IsRoleMove = true;
            Jup = Input.GetAxis("Mouse Y") * MouseSpeedX;
            Jright = Input.GetAxis("Mouse X") * MouseSpeedY;
        }
        //计算摄像机的旋转
        public void CalculateCameraRotate()
        {
             if (IsMouse1)
            {
                Cursor.lockState = CursorLockMode.Confined;
                CalculateCameraMove();
                CameraHandParent.transform.Rotate(Vector3.up, Jright * horizontalSpeed * Time.fixedDeltaTime);
                tempEulerX -= Jup * verticalSpeed * Time.fixedDeltaTime;
                tempEulerX = Mathf.Clamp(tempEulerX, CameraXMin, CameraXMax);
                CameraHand.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
                Offsetpos =GameObject.transform.position - CameraHand.transform.position;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameObject.transform.position = Offsetpos + CameraHand.transform.position;
            }
        }
        Vector3 DiffPos;
        //发射射线计算摄像机与人物之间的距离
        private void CalculateCameraDistance()
        {
            Ray ray2 = new Ray(CameraHand.transform.position, (GameObject.transform.position - CameraHand.transform.position).normalized);
            RaycastHit hit2;
             if (Physics.Raycast(ray2, out hit2, 100, RoleCameraCanPenetrableLayers))
            {
                //Debug.DrawRay(ray2.origin, (this.transform.position - CameraHand.transform.position).normalized, Color.blue);
                if (hit2.distance < Mouse2Distance)
                {
                    if (hit2.distance < ColliderDistance)
                    {
                        mPlayerNav.Value.transform.localPosition = Vector3.SmoothDamp(CameraHand.transform.localPosition, CalculateCameraHandPos(hit2.distance), ref DiffPos, 0);

                        CrameraPosition(hit2.distance);
                        GameObject.transform.position = Offsetpos + CameraHand.transform.position;
                        return;
                    }
                    else
                    {
                        CrameraPosition(hit2.distance);
                    }
                }
                else
                {
                    CrameraPosition(Mouse2Distance);
                }
            }
            else if (hit2.distance == 0)
            {
                CrameraPosition(Mouse2Distance);
            }
            mPlayerNav.Value.transform.localPosition = Vector3.SmoothDamp(PlayerCamerahandInitPos, CalculateCameraHandPos(hit2.distance), ref DiffPos, 0);

            GameObject. transform.position = Offsetpos + CameraHand.transform.position;
        }
        Vector3 Section = new Vector3(0, 0, 0);
        private Vector3 CalculateCameraHandPos(float Distance)
        {
            Section.x = 0;
            Section.z = 0;
            Section.y = 0;
            if (Distance < 0.06f)
            {

                Section.y = ColliderDistance;
            }
            else
            {
                Section.y = ColliderDistance - Distance + 0.05f;
            }
            Section = PlayerCamerahandInitPos + Section;
            return Section;
        }
        //计算摄像机应该拥有的位置
        private void CrameraPosition(float theDistance)
        {
            //1.设置摄像机的位置
            this.Offsetpos = this.Offsetpos.normalized * theDistance;
        }
    }
}