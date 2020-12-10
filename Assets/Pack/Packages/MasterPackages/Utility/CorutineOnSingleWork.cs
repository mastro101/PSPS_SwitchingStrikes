using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutineOnSingleWork : MonoBehaviour
{
    IEnumerator corutine;

    public CorutineOnSingleWork SetCorutine(IEnumerator corutine)
    {
        this.corutine = corutine;
        return this;
    }

    IEnumerator workingCorutine;

    public void StartCo()
    {
        if (workingCorutine == null)
        {
            workingCorutine = corutine;
            StartCoroutine(workingCorutine);
        }
        else
        {
            StopCoroutine(workingCorutine);
            workingCorutine = corutine;
            StartCoroutine(workingCorutine);
        }
    }

    public void StopCo()
    {
        if (workingCorutine == null)
            return;

        StopCoroutine(workingCorutine);
        workingCorutine = null;
    }
}