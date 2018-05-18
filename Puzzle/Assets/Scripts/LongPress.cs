using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonPressed = false;
    private bool coroutineStarted = false;


    private void Update()
    {
        if ((buttonPressed == true) && (coroutineStarted == false))
        {
            StartCoroutine("PressButton");
            coroutineStarted = true;
        }
        else if (buttonPressed == false)
        {
            StopCoroutine("PressButton");
            coroutineStarted = false;
        }
    }





    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        Controller.Instance.UndoTurn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    private IEnumerator PressButton()
    {
        float maxDelay = 0.28f;
        float minDelay = 0.12f;
        int count = 5;
        float delta = (maxDelay - minDelay) / count;

        while (true)
        {
            yield return new WaitForSeconds(maxDelay);

            if (maxDelay > minDelay)
            {
                maxDelay -= delta;
            }

            Controller.Instance.UndoTurn();
        }
    }
}
