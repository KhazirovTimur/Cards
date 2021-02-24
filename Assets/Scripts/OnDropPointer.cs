using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDropPointer : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData pointerEventData)
    {
        PointerReact card = pointerEventData.pointerDrag.GetComponent<PointerReact>();

        if (card)
            card.defaulParent = this.transform;
    }
}
