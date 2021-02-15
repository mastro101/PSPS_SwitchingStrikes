using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScoreText = null;
    [SerializeField] TextMeshProUGUI actualScoretext = null;
    [SerializeField] FloatData score = null;
    [SerializeField] FloatData bestScore = null;
    [SerializeField] Image bestScoreImage = null;

    private void Awake()
    {
        actualScoretext.text = 0.ToString();
        if (score)
        {
            SetText(score.value);
            score.OnChangeValue += SetText;
        }

        if (bestScore && bestScoreText)
        {
            bestScore.value = 0;
            bestScore.value = PlayerPrefs.GetFloat("BestScore", bestScore.value);
            bestScoreText.text = bestScore.GetValue().ToString();
        }

        if(bestScore && bestScoreImage)
        {
            if (score.value == bestScore.value)
            {
                bestScoreImage.gameObject.SetActive(true);
            }
        }
    }

    void SetText(float f)
    {
        actualScoretext.text = f.ToString();
        if (bestScore)
        {
            if (f > bestScore.value)
            {
                bestScore.value = f;
                bestScoreText.text = bestScore.value.ToString();
                PlayerPrefs.SetFloat("BestScore", bestScore.value);
            }
        }
    }
}