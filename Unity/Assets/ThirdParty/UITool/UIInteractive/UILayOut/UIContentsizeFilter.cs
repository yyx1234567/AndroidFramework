using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIContentsizeFilter : MonoBehaviour
{
    public Image Content;
    public Text TextTarget;
    public TMPro.TMP_Text TMP_Target;

    public Vector2 Padding;
    private void OnEnable()
    {
        UpdateSize();
    }

    [Sirenix.OdinInspector.Button("测试", Sirenix.OdinInspector.ButtonSizes.Large)]
    public void UpdateSize()
    {
        if (Content == null)
            Content = GetComponentInChildren<Image>();
        if (Content == null)
            return;
        if (TMP_Target == null && TextTarget == null)
            return;
        var height = TextTarget == null ? TMP_Target.preferredHeight:TextTarget.preferredHeight;
        var endvalue = new Vector2(Content.rectTransform.sizeDelta.x, height);
        if (TextTarget != null&& TextTarget.text==string.Empty|| TMP_Target != null && TMP_Target.text==string.Empty)
        {
            Content.rectTransform.DOSizeDelta(new Vector2(Content.rectTransform.sizeDelta.x, 0), 0.3f);
         }
         else 
        {
            Content.rectTransform.DOSizeDelta(endvalue + Padding, 0.3f);
         }
    }
}
