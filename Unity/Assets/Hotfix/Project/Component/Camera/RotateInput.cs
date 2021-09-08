using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// 旋转
    /// </summary>
    public class RotateInput : IInputControl
    {
        float previewSummationX;
        float previewSummationY;
 
        float _deltaX, _deltaY;
        public bool MouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastRotatePos = Vector2.zero;
            }
            if (Input.GetMouseButton(0))
            {
                RotateOperate(Input.mousePosition, 1);
                return true;
            }
            else if (_deltaX == 0 && _deltaY == 0)
            {
                _lastRotatePos = Vector2.zero;
                return false;
            }
            return true;
        }

        public bool TouchInput()
        {
            if (Input.touchCount == 1)
            {
                RotateOperate(Input.GetTouch(0).position, 2);
                return true;
            }
            else if (_deltaX == 0 && _deltaY == 0)
            {
                _lastRotatePos = Vector2.zero;
                return false;
            }
            return true;
        }

        private Vector2 _lastRotatePos;
        private void RotateOperate(Vector2 position, float power)
        {
            previewSummationX = InputParameter.SummationX;
            previewSummationY = InputParameter.SummationY;
            _lastRotatePos = _lastRotatePos == Vector2.zero ? position : _lastRotatePos;
            Vector2 pos = _lastRotatePos - position;
            _deltaX = pos.y * 0.15f * power;
            _deltaY = -pos.x * 0.12f * power;
            _lastRotatePos = position;
        }

         public void UpdatInput(Transform camera, Transform target, Collider collider)
        {
            //_deltaX = Mathf.Lerp(_deltaX, 0, Time.deltaTime * InputParameter.RotateSpeed);
            //_deltaY = Mathf.Lerp(_deltaY, 0, Time.deltaTime * InputParameter.RotateSpeed);
             InputParameter.SummationX += _deltaY;
            InputParameter.SummationY += _deltaX;
            _deltaX = 0;
            _deltaY = 0;
             InputParameter.SummationY = Mathf.Clamp(InputParameter.SummationY, InputParameter.ClampMinY, InputParameter.ClampMaxY);
            //InputParameter.SummationY = Mathf.Clamp(InputParameter.SummationY, InputParameter.ClampMinX, InputParameter.ClampMaxX);

            var previewLocalEuler = camera.localEulerAngles;
            var previewPos = camera.position;
            camera.localEulerAngles = new Vector3(InputParameter.SummationY, InputParameter.SummationX);
            camera.position = camera.rotation * new Vector3(0.0f, 0.0f, -InputParameter.SummationZ) + target.transform.position;
 
            if (collider != null && !collider.bounds.Contains(camera.position))
            {
                InputParameter.SummationX = previewSummationX;
                InputParameter.SummationY = previewSummationY;
                camera.localEulerAngles = previewLocalEuler;
                camera.position = previewPos;
                _deltaY = 0;
                _deltaX = 0;
            }
        }

        public void Reset()
        {
            _lastRotatePos = Vector2.zero;
        }
    }
}