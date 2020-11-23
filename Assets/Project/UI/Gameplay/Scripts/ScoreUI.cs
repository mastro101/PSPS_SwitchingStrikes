using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScoreText = null;
    [SerializeField] TextMeshProUGUI actualScoretext = null;
    [SerializeField] FloatData score = null;
    [SerializeField] FloatData bestScore = null;

    private void Awake()
    {
        actualScoretext.text = 0.ToString();
        bestScoreText.text = bestScore.GetValue().ToString();
        if (score)
        {
            SetText(score.value);
            score.OnChangeValue += SetText;
        }
    }

    void SetText(float f)
    {
        actualScoretext.text = f.ToString();
        if (f > bestScore.value)
        {
            bestScore.SetValue(f);
            bestScoreText.text = bestScore.value.ToString();
        }
    }
}