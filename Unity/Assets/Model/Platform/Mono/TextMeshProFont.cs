using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProFont : MonoBehaviour
{
    public TMP_FontAsset tMP_Font;
    private void Awake()
    {
        GetComponent<TMPro.TMP_Text>().font = tMP_Font;
    }
}
