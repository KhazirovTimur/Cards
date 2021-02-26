using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PointerReact : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameEventSO recalcPos;
    public CanvasGroup cg;
    public CardBehaviourScript card;
    bool CoroutineStarted = false;



    public void ChangeCorState() 
    {
        if(!CoroutineStarted && !card.wasPlayed)
        CoroutineStarted = true;
        else
        CoroutineStarted = false;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted && !card.wasPlayed)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 1);
            this.transform.SetSiblingIndex(this.transform.parent.transform.childCount-1);
            cg.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted && !card.wasPlayed)
            transform.position = pointerEventData.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (!CoroutineStarted && !card.wasPlayed)
        {
            cg.blocksRaycasts = true;
            recalcPos.Raise();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(!card.wasPlayed)
        card.borderImage.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!card.wasPlayed)
        card.borderImage.color = new Color(1, 1, 1, 0);
    }

  
}
