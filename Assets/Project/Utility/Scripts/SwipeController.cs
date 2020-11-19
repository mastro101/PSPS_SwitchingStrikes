using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SwipeController : MonoBehaviour
{
    [SerializeField] bool touch = true;
    [SerializeField] bool mouse = true;
    [Space]
    [SerializeField] bool onUpdate = false;
    [Space]
    [Header("Swipe")]
    [SerializeField] bool oneSwipeForTouch = true;
    [SerializeField] bool swipeOnRealese = false;
    [SerializeField] float minDistance = 0.2f;
    [SerializeField] UnityEvent<SwipeData> OnSwipeUE = null;
    [Space]
    [Header("Touch")]
    [SerializeField] float timeForTouchAndRealese = 0;
    [SerializeField] UnityEvent OnStartTouchUE = null;
    [SerializeField] UnityEvent OnTouchAndRealeseUE = null;
    [SerializeField] UnityEvent OnEndTouchUE = null;
    [SerializeField] TextMeshProUGUI swipeTextDebug;

    public Action OnStartTouch;
    public Action<SwipeData> OnSwipe;

    public Action OnTouchAndRealese;
    public Action OnEndTouch;

    bool swiped;
    float swipeMagnitude;

    bool canTap;

    Vector2 startTouchPos = Vector2.zero;
    Vector2 positionTouch = Vector2.zero;
    Vector2 oldTouchPos = Vector2.zero;

    Vector2 swipeVector = Vector2.zero;
    Vector2 swipeDirection = Vector2.zero;

    private void Update()
    {
        if (onUpdate)
        {
            if (touch)
                SwipeHandlerTouch();
            if (mouse)
                SwipeHandlerClick();
        }
    }

    public void SwipeHandlerTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            if (t.phase == TouchPhase.Began)
            {
                swiped = false;
            }

            if (oneSwipeForTouch && swiped)
                return;

            if (t.phase == TouchPhase.Began)
            {
                canTap = true;
                if (timeForTouchAndRealese > 0)
                {
                    CheckPressTime();
                }
                OnStartTouch?.Invoke();
                OnStartTouchUE?.Invoke();
                startTouchPos = t.position;
                oldTouchPos = startTouchPos;
                //CheckPressTime();
            }

            if ((t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) && swipeOnRealese)
            {
                positionTouch = t.position;
                DetectSwipe();
            }

            if (t.phase == TouchPhase.Ended)
            {
                DetectSwipe();
                OnEndTouchUE?.Invoke();
                OnEndTouch?.Invoke();
                if (!swiped)
                {
                    if (canTap)
                    {
                        OnTouchAndRealese?.Invoke();
                        OnTouchAndRealeseUE?.Invoke();
                    }
                }
            }

            oldTouchPos = t.position;
        }

        //foreach (Touch t in Input.touches)
        //{
        //    if (t.phase == TouchPhase.Began)
        //    {
        //        swiped = false;
        //    }

        //    if (oneSwipeForTouch && swiped)
        //        return;

        //    if (t.phase == TouchPhase.Began)
        //    {
        //        canTap = true;
        //        if (timeForTouchAndRealese > 0)
        //        {
        //            CheckPressTime();
        //        }
        //        OnStartTouch?.Invoke();
        //        OnStartTouchUE?.Invoke();
        //        startTouchPos = t.position;
        //        oldTouchPos = startTouchPos;
        //        //CheckPressTime();
        //    }

        //    if ((t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) && swipeOnRealese)
        //    {
        //        positionTouch = t.position;
        //        DetectSwipe();
        //    }

        //    if (t.phase == TouchPhase.Ended)
        //    {
        //        DetectSwipe();
        //        OnEndTouchUE?.Invoke();
        //        OnEndTouch?.Invoke();
        //        if (!swiped)
        //        {
        //            if (canTap)
        //            {
        //                OnTouchAndRealese?.Invoke();
        //                OnTouchAndRealeseUE?.Invoke(); 
        //            }
        //        }
        //    }

        //    oldTouchPos = t.position;
        //}

        swipeTextDebug.text = positionTouch.ToString();
    }
    
    public void SwipeHandlerClick()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            swiped = false;
        }

        if (oneSwipeForTouch && swiped)
            return;

        if (Input.GetMouseButtonDown(0) == true)
        {
            canTap = true;
            if (timeForTouchAndRealese > 0)
            {
                CheckPressTime();
            }
            OnStartTouch?.Invoke();
            OnStartTouchUE?.Invoke();
            startTouchPos = Input.mousePosition;
            oldTouchPos = startTouchPos;
            //CheckPressTime();
        }

        if (Input.GetMouseButton(0))
        {
            positionTouch = Input.mousePosition;
            DetectSwipe();
        }

        if (Input.GetMouseButtonUp(0) == true)
        {
            //StopCheckPressTime();
            DetectSwipe();
            OnEndTouchUE?.Invoke();
            OnEndTouch?.Invoke();
            if (!swiped)
            {
                if (canTap)
                {
                    OnTouchAndRealese?.Invoke();
                    OnTouchAndRealeseUE?.Invoke();
                }
            }
        }

        oldTouchPos = Input.mousePosition;
    }

    void DetectSwipe()
    {
        swipeVector = (positionTouch - oldTouchPos);
        swipeMagnitude = swipeVector.magnitude;
        if (swipeMagnitude > minDistance)
        {
            swipeDirection = swipeVector.normalized;
            swiped = true;
            OnSwipe?.Invoke(new SwipeData(swipeVector));
            OnSwipeUE?.Invoke(new SwipeData(swipeVector));
        }
    }

    IEnumerator checkPressTimeCorutine;
    IEnumerator CheckPressTimeCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeForTouchAndRealese);
            canTap = false;
        }
    }

    void CheckPressTime()
    {
        if (checkPressTimeCorutine != null)
            StopCheckPressTime();

        checkPressTimeCorutine = CheckPressTimeCorutine();
        StartCoroutine(checkPressTimeCorutine);
    }

    void StopCheckPressTime()
    {
        if (checkPressTimeCorutine != null)
            StopCoroutine(checkPressTimeCorutine);
        else
            StopCoroutine("CheckPressTimeCorutine");
    }
}

public struct SwipeData
{
    public Vector2 swipeVector { get; }
    public Vector2 swipeDirection { get; }
    public float angle { get; }

    public SwipeData(Vector2 swipeVector)
    {
        this.swipeVector = swipeVector;
        swipeDirection = swipeVector.normalized;
        angle = Vector2.SignedAngle(Vector2.up, swipeVector);
        if (angle < 0)
        {
            angle += 360;
        }
        //angle = Mathf.Abs(angle);
    }
}