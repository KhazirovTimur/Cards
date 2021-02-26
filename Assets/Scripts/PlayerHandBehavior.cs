using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHandBehavior : MonoBehaviour
{
    List<CardBehaviourScript> cards = new List<CardBehaviourScript>();
    List<CardBehaviourScript> cardsToDestroy = new List<CardBehaviourScript>();

    public float deltaAngle = 3.0f;
    public float circleRadius = 2000;
    private bool corStart = false;

    public GameEventSO CorStarted;
    public GameEventSO CorEnded;
    public CardBehaviourScript cardExample;
    // Start is called before the first frame update

    private void Awake()
    {
        int i = UnityEngine.Random.Range(4, 6);
        while (i >= 0)
        {
            CardBehaviourScript card = Instantiate(cardExample);
            card.transform.SetParent(this.transform);
            card.OnDestroy += AddToDestroyList;
            cards.Add(card);
            i--;
        }
        cardsToDestroy = new List<CardBehaviourScript>();
        calcCardsPosNoAnim();
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
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
        float startAng = (deltaAngle /2.0f) - (chldCount / 2.0f) * deltaAngle;
        Vector3 circleCenter =  new Vector3(0, -circleRadius, 0);
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 pos = calcCardsAngle(startAng, circleCenter);
            cards[i].Move(pos);
            cards[i].Rotate(startAng);
            cards[i].transform.SetSiblingIndex(i);
            startAng += deltaAngle;
        }
    }

    Vector3 calcCardsAngle(float ang, Vector3 center) {
        Vector3 pos;
        pos.x = center.x + (circleRadius * Mathf.Sin(ang * Mathf.Deg2Rad));
        pos.y = center.y + (circleRadius * Mathf.Cos(ang * Mathf.Deg2Rad));
        pos.z = 0;
        return pos;
    }

    public void calcCardsPosNoAnim()
    {
        float chldCount = transform.childCount;
        float startAng = (deltaAngle / 2.0f) - (chldCount / 2.0f) * deltaAngle;
        Vector3 circleCenter = new Vector3(0, -circleRadius, 0);
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 pos = calcCardsAngle(startAng, circleCenter);
            cards[i].transform.localPosition = pos;
            cards[i].transform.rotation = Quaternion.Euler(0, 0, -startAng);
            startAng += deltaAngle;
        }
    }


    public void ChangeCardValue() 
    {
        if(!corStart)
            StartCoroutine(ChangingCardValue());
        corStart = true;
    }

    IEnumerator ChangingCardValue()
    {
        CorStarted.Raise();
        cards[Random.Range(0, cards.Count-1)].ChangeValue(Random.Range(-2, 9), Random.Range(1, 3));
        yield return new WaitForSeconds(1);
        DestroyCardsList();
        yield return new WaitForSeconds(0.5f);
        calcCardsPos();
        CorEnded.Raise();
        corStart = false;
    }


    public void removeCardFromList(CardBehaviourScript cd) 
    {
        cards.Remove(cd);
        calcCardsPos();
    }
}