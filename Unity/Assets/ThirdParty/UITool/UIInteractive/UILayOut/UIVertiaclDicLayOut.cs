using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIVertiaclDicLayOut : Sirenix.OdinInspector.SerializedMonoBehaviour
{
    public Dictionary<Transform, float> SpaceDic = new Dictionary<Transform, float>();

 
    void Start()
    {
        UpdateLayOut();
    }

    private void UpdateLayOut()
    {
        foreach (var item in SpaceDic)
        {
 
        }

        float deltaHeight = 0;
        for (int i = 0; i < SpaceDic.Count; i++)
        {
            float height = 0;
            float speacing = SpaceDic.ElementAt(i).Value;
            if (i >= 1)
            {
                height = SpaceDic.ElementAt(i).Key.GetComponent<RectTransform>().rect.height;
            }
            else
            {
                speacing = 0;
            }
            deltaHeight += (height + speacing);
            SpaceDic.ElementAt(i).Key.localPosition = new Vector3(0, -deltaHeight, 0);
        }
     }
}
