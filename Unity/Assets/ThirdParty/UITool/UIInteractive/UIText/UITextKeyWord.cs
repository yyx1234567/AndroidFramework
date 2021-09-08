using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
 using System.Collections.Generic;

public struct KeyWordFontStyle
{
     [Sirenix.OdinInspector.LabelText("加粗")]
    public bool Bold;
    [Sirenix.OdinInspector.LabelText("下划线")]
    public bool UnderLine;
    [Sirenix.OdinInspector.LabelText("字体大小")]
    public int FontSize;
    [Sirenix.OdinInspector.LabelText("选中颜色")]
    public Color SelectedColor;
}

public class UITextKeyWord : Sirenix.OdinInspector.SerializedMonoBehaviour, IPointerClickHandler
{
    [Sirenix.OdinInspector.LabelText("文本组件")]
    public TMP_Text TargetText;
    
    [Sirenix.OdinInspector.LabelText("关键词")]
    public Dictionary<string, bool> KeyWorks = new Dictionary<string, bool>();

    [Sirenix.OdinInspector.LabelText("已选择的关键词")]
    public List<string> SelectedKeyWorks = new List<string>();

    [Sirenix.OdinInspector.LabelText("关键词样式")]
    public KeyWordFontStyle KeyWordFontStyle;

    [Sirenix.OdinInspector.LabelText("关键词初始颜色")]
    public Color InitColor=Color.white;

    [Sirenix.OdinInspector.LabelText("取消选择")]
    public bool  Cancelable;

    [HideInInspector]
    public System.Action<KeyValuePair<string, bool>> OnClickKeyWord;

     void Awake()
     {
         Init();
     }

 

    private void Init()
    {
        TargetText = TargetText ?? GetComponent<TMPro.TMP_Text>();
        if (TargetText == null)
        {
            Debug.LogError("找不到TMP_Text组件");
            return;
        }
        foreach (var item in KeyWorks)
        {
            TargetText.text = TargetText.text.Replace(item.Key, $"<link=\"ID\"><color=#{ColorUtility.ToHtmlStringRGB(InitColor)}>{item.Key}</color></link>");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
         Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TargetText, pos, Camera.main);
        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = TargetText.textInfo.linkInfo[linkIndex];
            var linkText = linkInfo.GetLinkText();
            if (KeyWorks.TryGetValue(linkText, out bool selected))
            {
                if (!selected)
                {
                    KeyWorks[linkText] = true;
                    SetTextStyle(TargetText, linkText, InitColor, KeyWordFontStyle);
                    SelectedKeyWorks.Add(linkText);
                }
                else
                {
                    if (Cancelable)
                    {
                        KeyWorks[linkText] = false;
                        RecoverFontStyle(TargetText, linkText, KeyWordFontStyle.SelectedColor, KeyWordFontStyle);
                        SelectedKeyWorks.Remove(linkText);
                    }
                }
                OnClickKeyWord?.Invoke(new KeyValuePair<string, bool>(linkText, KeyWorks[linkText]));
            }
        }
    }

    private void SetTextStyle(TMPro.TMP_Text text, string value, Color form, KeyWordFontStyle fontStyle)
    {
        text.text = text.text.Replace($"<color=#{ColorUtility.ToHtmlStringRGB(form)}>{value}</color>", $"<color=#{ColorUtility.ToHtmlStringRGB(fontStyle.SelectedColor)}>{value}</color>");
        if (fontStyle.UnderLine)
        {
            text.text = text.text.Replace($"{value}", $"<u>{value}</u>");
        }
        if (fontStyle.Bold)
        {
            text.text = text.text.Replace($"{value}", $"<b>{value}</b>");
        }
        text.text = text.text.Replace($"{value}", $"<size={fontStyle.FontSize}>{value}</size>");
     }

    private void RecoverFontStyle(TMPro.TMP_Text text, string value, Color form, KeyWordFontStyle fontStyle)
    {
        text.text = text.text.Replace($"<size={fontStyle.FontSize}>{value}</size>", $"{value}");
 
        if (fontStyle.Bold)
        {
            text.text = text.text.Replace($"<b>{value}</b>", $"{value}");
        }
        if (fontStyle.UnderLine)
        {
            text.text = text.text.Replace($"<u>{value}</u>", $"{value}");
        }

        text.text = text.text.Replace($"<color=#{ColorUtility.ToHtmlStringRGB(form)}>{value}</color>", $"<color=#{ColorUtility.ToHtmlStringRGB(InitColor)}>{value}</color>");
    }
}
