using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] FloatData score = null;

    private void Awake()
    {
        text.text = 0.ToString();
        if (score)
        {
            SetText(score.value);
            score.OnChangeValue += SetText;
        }
    }



    void SetText(float f)
    {
        text.text = f.ToString();
    }
}