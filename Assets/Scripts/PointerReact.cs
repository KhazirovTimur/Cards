using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PointerReact : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public Transform defaulParent;
    public GameEventSO recalcPos;
    bool CoroutineStarted = false;

    public void ChangeCorState() 
    {
        CoroutineStarted = true;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted)
        {
            defaulParent = this.transform.parent;
            this.transform.SetParent(defaulParent.parent);
            if (this.TryGetComponent<CanvasGroup>(out CanvasGroup i))
                i.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted)
            transform.position = pointerEventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted)
        {
            CardBehaviourScript card = pointerEventData.pointerDrag.GetComponent<CardBehaviourScript>();
            this.transform.SetParent(defaulParent);
            if (this.TryGetComponent<CanvasGroup>(out CanvasGroup i))
                i.blocksRaycasts = true;
            recalcPos.Raise();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        CardBehaviourScript card = pointerEventData.pointerEnter.GetComponent<CardBehaviourScript>();
        card.borderImage.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        CardBehaviourScript card = pointerEventData.pointerEnter.GetComponent<CardBehaviourScript>();
        card.borderImage.color = new Color(1, 1, 1, 0);
    }

  
}
