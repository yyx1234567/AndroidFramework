using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 平移
/// </summary>
namespace ETHotfix
{
    public class MoveInput : IInputControl
    {
        private float _NewPosX, _NewPosY,_NewPosZ;
        public bool MouseInput()
        {
            if (Input.GetMouseButton(2))
            {
                MoveOperate(Input.mousePosition, 1);
                return true;
            }
            else if (Input.GetMouseButton(1))
            {
                MoveOperateUpDown(Input.mousePosition, 1);
                return true;
            }
            else
            {
                _lastPos = Vector2.zero;
                return false;
            }
        }

        private void MoveOperateUpDown(Vector2 position1, float power)
        {
            if (_lastPos == Vector2.zero)
            {
                _lastPos = Input.mousePosition;
            }
            Vector2 pos = position1 - _lastPos;
          //  _NewPosX = pos.x * 0.0025f * power;
            _NewPosZ = pos.y * 0.0013f * power;
            _lastPos = position1;
        }

        public bool TouchInput()
        {
            if (Input.touchCount == 3)
            {
                MoveOperate(Input.GetTouch(0).position, 2);
                return true;
            }
            else
            {
                _lastPos = Vector2.zero;
                return false;
            }
        }

        private Vector2 _lastPos;
        /// <summary>
        /// 平移
        /// </summary>
        /// <param name="position1"></param>
        private void MoveOperate(Vector2 position1, float power)
        {
            if (_lastPos == Vector2.zero)
            {
                _lastPos = Input.mousePosition;
            }
            Vector2 pos = position1 - _lastPos;
            _NewPosX = pos.x * 0.005f * power;
            _NewPosY = pos.y * 0.005f * power;
            _lastPos = position1;
        }
        public void UpdatInput(Transform camera, Transform target, Collider collider)
        {
            var preeuler = target.transform.position;
            target.transform.eulerAngles = new Vector3(0, camera.eulerAngles.y, camera.eulerAngles.z);
            var pos2 = target.transform.rotation * new Vector3(_NewPosX, -_NewPosZ, _NewPosY);
            target.transform.position -= pos2;

           if (collider != null && !collider.bounds.Contains(target.transform.position))
           {
                 target.position = preeuler;
            }
            _NewPosX = 0;
            _NewPosY = 0;
            _NewPosZ = 0;

        }

        public void Reset()
        {
            _lastPos = Vector2.zero;
        }
    }
}