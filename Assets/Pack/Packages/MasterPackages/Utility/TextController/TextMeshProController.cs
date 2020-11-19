using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    public void SetText(string s)
    {
        text.text = s;
    }
}