using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputControl 
{
    bool TouchInput();
    bool MouseInput();
    void UpdatInput(Transform camera,Transform target,Collider bounds);

}
