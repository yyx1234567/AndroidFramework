using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 平移
/// </summary>
public class MoveInput : IInputControl
{
    private GameObject _dragTarget;

    private float _NewPosX, _NewPosY;
    public bool MouseInput()
    {
        if (Input.GetMouseButton(2))
        {
            MoveOperate(Input.mousePosition, 1);
            return true;
        }
        else
        {
            _lastPos = Vector2.zero;
            return false;
        }
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
            _lastPos = position1;
        }
        Vector2 pos = position1 - _lastPos;
        _NewPosX = pos.x * 0.0025f * power;
        _NewPosY = pos.y * 0.0025f * power;
        _lastPos = position1;
    }

    private Vector3 _lastCameraLocalAngle;
    public void UpdatInput(Transform camera, Transform target, Collider collider)
    {
        if (_lastPos == Vector2.zero)
            return;
        if (_dragTarget == null)
        {
            _dragTarget = new GameObject("[MoveRoot]");
            _dragTarget.transform.SetParent(camera.transform.parent.parent);
            _dragTarget.transform.position = camera.position;
            _dragTarget.transform.localEulerAngles = new Vector3(0, camera.transform.localEulerAngles.y, 0);
            camera.parent.SetParent(_dragTarget.transform);
        }

        if (camera.localEulerAngles != _lastCameraLocalAngle)
        {
            camera.parent.SetParent(null);
            _dragTarget.transform.position = camera.position;
            _dragTarget.transform.localEulerAngles = new Vector3(0, camera.transform.localEulerAngles.y, 0);
            camera.parent.SetParent(_dragTarget.transform);
        }

        _lastCameraLocalAngle = camera.localEulerAngles;


        var delta = new Vector3(-_NewPosX, 0, -_NewPosY);

        var previewDragTargetPos = _dragTarget.transform.position;
        var previewargetPos = target.position;
        target.transform.localEulerAngles = new Vector3(0, camera.localEulerAngles.y, 0);


        _dragTarget.transform.position += _dragTarget.transform.rotation * delta;

        target.position += target.transform.rotation * delta;

 
        if (collider != null && !collider.bounds.Contains(_dragTarget.transform.position))
        {
            _dragTarget.transform.position = previewDragTargetPos;
            target.position = previewargetPos;
            _NewPosX = 0;
            _NewPosY = 0;
        }

    }

}
