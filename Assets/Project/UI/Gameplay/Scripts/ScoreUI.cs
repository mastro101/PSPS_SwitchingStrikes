using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] FloatData score;

    private void Awake()
    {
        if (score)
        {
            score.OnChangeValue += SetText;
        }
    }

    void SetText(float f)
    {
        text.text = f.ToString();
    }
}