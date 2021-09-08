using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVertiaclLayOut : MonoBehaviour
{
    public float Speacing;
    void OnTransformChildrenChanged()
    {
        UpdateLayOut();
    }

    private void UpdateLayOut()
    {
        float deltaHeight = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            float height = 0;
            float speacing = Speacing;
            if (i >= 1)
            {
                height = transform.GetChild(i).GetComponent<RectTransform>().rect.height;
            }
            else
            {
                speacing = 0;
            }
            transform.GetChild(i).transform.localPosition = new Vector3(0, -deltaHeight,0);
            deltaHeight += (height + speacing);
        }
        //GetComponent<RectTransform>().rect = new Rect(0,deltaHeight);
    }
}
