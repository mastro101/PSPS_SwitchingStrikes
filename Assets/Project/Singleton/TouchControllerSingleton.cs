using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TouchControllerSingleton : SingletonProject<TouchControllerSingleton>
{
    Touch[] touches;
    Touch touch;

    //Vector3[] touchPositions;
    Vector3 touchPosition;

    TouchBehaviour touchBehaviour;
    //TouchBehaviour[] StartTouchBehaviours;

    //RaycastHit[] hits;
    RaycastHit hit;

    override protected void Awake()
    {
        touches = new Touch[10];
        //touchPositions = new Vector3[touches.Length];
        //StartTouchBehaviours = new TouchBehaviour[touches.Length];
    }

    private void Update()
    {
        int i, l;
        for (i = 0, l = Input.touchCount; i < l; i++)
        {
            if (i > touches.Length)
                break;

            touches[i] = Input.GetTouch(i);
            touch = touches[i];

            //touchPositions[i] = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);//touchPositions[i];

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hit))//out hits[i]))
            {
                //hit = hits[i];
                touchBehaviour = hit.transform.GetComponent<TouchBehaviour>();
                if (touchBehaviour != null)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchBehaviour.OnStartTouch(i);
                        touchBehaviour.startTouch = true;
                        Debug.LogFormat("touch %0 is Started", i);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (touchBehaviour.startTouch)
                        {
                            touchBehaviour.OnTap(i);
                            touchBehaviour.startTouch = false;
                        }
                        touchBehaviour.OnEndTouch(i);
                        Debug.LogFormat("touch %0 is Ended", i);
                    }

                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touchBehaviour.oldOverlapTouch == false)
                    {
                        touchBehaviour.OnStartOverlapTouch(i);
                        Debug.LogFormat("overlap %0 is started", i);
                    }
                    
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        touchBehaviour.OnOverlapTouch(i);
                        if (touchBehaviour.overlapTouch == false)
                            touchBehaviour.overlapTouch = true;
                    }

                    touchBehaviour.oldOverlapTouch = touchBehaviour.overlapTouch;
                }
            }
        }
    }
}

interface TouchBehaviour
{
    GameObject gameObject { get; }
    Collider2D collider { get; }

    bool startTouch { get; set; }
    bool oldOverlapTouch { get; set; }
    bool overlapTouch { get; set; }

    void OnStartTouch(int _touchIndex);
    void OnTap(int _touchIndex);
    void OnEndTouch(int _touchIndex);
    void OnStartOverlapTouch(int _touchIndex);
    void OnEndOverlapTouch(int _touchIndex);
    void OnOverlapTouch(int _touchIndex);
}