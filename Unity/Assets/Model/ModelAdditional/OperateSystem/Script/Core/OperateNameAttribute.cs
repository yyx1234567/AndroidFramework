using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateNameAttribute : Attribute
{
    public OperateNameAttribute(string name)
    {
        OperateName = name;
    }
    public string OperateName;
}
