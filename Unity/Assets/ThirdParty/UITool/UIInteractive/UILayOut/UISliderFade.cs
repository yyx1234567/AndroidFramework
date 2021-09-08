using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderFade : MonoBehaviour
{
    private Scrollbar m_slider;

    public Image Target;
    // Start is called before the first frame update
    void Start()
    {
        m_slider = GetComponent<Scrollbar>();
        m_slider.onValueChanged.AddListener(SliderHandle);
     }

    private void SliderHandle(float arg0)
    {
        Target.color = new Color(Target.color.r, Target.color.g, Target.color.b, 1 - arg0);
    }

    public void Reflash()
    {
        if (m_slider != null)
        {
             SliderHandle(m_slider.value);
        }
    }

    private void OnEnable()
    {
        Reflash();
    }
}
