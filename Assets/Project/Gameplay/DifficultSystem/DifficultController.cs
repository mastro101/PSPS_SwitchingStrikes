using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultController : MonoBehaviour
{
    [SerializeField] IntData difficultLevel = null;
    [SerializeField] float secondToChange = 30f;
    [SerializeField] int changeValue = 1;

    CorutineOnSingleWork changeDifficultCorutine;

    private void Awake()
    {
        difficultLevel.value = 1;
        changeDifficultCorutine = gameObject.AddComponent<CorutineOnSingleWork>().SetCorutine(ChangeDifficult());
        changeDifficultCorutine.SetCorutine(ChangeDifficult());
    }

    private void OnDisable()
    {
        changeDifficultCorutine.StopCo();
    }

    IEnumerator ChangeDifficult()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondToChange);
            difficultLevel.value += changeValue;
        }
    }
}