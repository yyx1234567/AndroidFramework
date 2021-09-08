using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UITextMeshPro : TextMeshProUGUI
{
    [Sirenix.OdinInspector.ShowInInspector]
    public TMP_FontAsset fontAsset;
    protected override void Start()
    {
        base.Start();
        font = fontAsset;
    }
}
