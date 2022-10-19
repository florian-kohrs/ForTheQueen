using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MultiClickButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public UnityEvent rightClick;

    public UnityEvent leftClick;

    public UnityEvent onBeginHover;

    public UnityEvent onEndHover;

    public bool interactable;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if(eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;

        onBeginHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable)
            return;

        onEndHover.Invoke();
    }
}
