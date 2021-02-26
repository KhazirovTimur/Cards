using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;


public class TableBehavior : MonoBehaviour,  IDropHandler
{
    List<CardBehaviourScript> cards = new List<CardBehaviourScript>();
    List<CardBehaviourScript> cardsToDestroy = new List<CardBehaviourScript>();

    Dictionary<GameObject, CardBehaviourScript> cardsDict = new Dictionary<GameObject, CardBehaviourScript>();

    private PlayerHandBehavior playerHand;

    public float delta=150;
    // Start is called before the first frame update

    private void Start()
    {
        playerHand = FindObjectOfType<PlayerHandBehavior>();
        cards = FindObjectsOfType<CardBehaviourScript>().ToList();
        for (int i = 0; i < cards.Count; i++)
            cardsDict.Add(cards[i].gameObject, cards[i]);
        cards = new List<CardBehaviourScript>();
    }

    public void AddToDestroyList(CardBehaviourScript card) 
    {
        if (cards.Contains(card))
        {
            cardsToDestroy.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    public void DestroyCardsList() 
    {
        foreach (var i in cardsToDestroy)
        { 
            cards.Remove(i);
            Destroy(i.gameObject);
        }
        cardsToDestroy = new List<CardBehaviourScript>();   
    }

    public void calcCardsPos()
    {
        float chldCount = cards.Count;
        float startLine = delta/2 - (chldCount / 2) * delta;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Move(new Vector3(startLine, 0, 0));
            startLine += delta;
        }


    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        GameObject cardObj = pointerEventData.pointerDrag.gameObject;
        CardBehaviourScript card;
        if (cardsDict.TryGetValue(cardObj, out card))
        {
            cardObj.transform.SetParent(this.transform);
            playerHand.removeCardFromList(card);
            cards.Add(card);
            calcCardsPos();
            card.wasPlayed = true;
        }
    }

}
