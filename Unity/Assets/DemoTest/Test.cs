using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
     public UnityEngine.UI.InputField input;

    public int MaxLength;

    private string lastdata;
    private void Awake()
    {
        input.onValueChanged.AddListener(UpdateText);
    }

    public void UpdateText(string text)
    {
        float length = 0;
        foreach (char item in text)
        {
            var bytes=  System.Text.Encoding.UTF8.GetBytes(item.ToString());
            switch (bytes.Length)
            {
                case 1:
                    length += 0.5f;
                    break;
                case 3:
                    length += 1;
                    break;
            }
        }
        if (length > MaxLength)
        {
            input.text = lastdata;
            return;
        }
        lastdata = text;
    }
 
}
