using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;

public class TableBehavior : MonoBehaviour
{
    List<CardBehaviourScript> cards = new List<CardBehaviourScript>();
    List<CardBehaviourScript> cardsToDestroy = new List<CardBehaviourScript>();

    public float delta=150;
    // Start is called before the first frame update


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
        float chldCount = transform.childCount;
        float startLine = delta/2 - (chldCount / 2) * delta;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<CardBehaviourScript>(out CardBehaviourScript j))
                j.Move(new Vector3(startLine, 0, 0));
            startLine += delta;
        }


    }

}
