using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHandBehavior : MonoBehaviour
{
    List<CardBehaviourScript> cards = new List<CardBehaviourScript>();
    List<CardBehaviourScript> cardsToDestroy = new List<CardBehaviourScript>();

    public GameEventSO CorStarted;
    public float delta=150;
    private bool corStart = false;

    public GameObject cardExample;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        int i = UnityEngine.Random.Range(4, 6);
        while(i>=0)
        {
            GameObject card = Instantiate(cardExample);
            card.transform.SetParent(this.transform);
            card.GetComponent<CardBehaviourScript>().OnDestroy += AddToDestroyList;
            cards.Add(card.GetComponent<CardBehaviourScript>());
            i--;
        }
        cardsToDestroy = new List<CardBehaviourScript>();
        calcCardsPosNoAnim();
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
        float chldCount = transform.childCount;
        float startLine = delta/2 - (chldCount / 2) * delta;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<CardBehaviourScript>(out CardBehaviourScript j))
                j.Move(new Vector3(startLine, 0, 0));
            startLine += delta;
        }


    }

    public void calcCardsPosNoAnim()
    {
        float chldCount = transform.childCount;
        float startLine = delta / 2 - (chldCount / 2) * delta;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<CardBehaviourScript>(out CardBehaviourScript j))
                j.transform.localPosition = new Vector3(startLine, 0, 0);
            startLine += delta;
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
        while (this.transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<CardBehaviourScript>(out CardBehaviourScript j))
                    j.ChangeValue(Random.Range(-2,9), Random.Range(1,4));
                yield return new WaitForSeconds(2);
            }
            DestroyCardsList();
            yield return new WaitForSeconds(0.5f);
            calcCardsPos();
            yield return new WaitForSeconds(2f);
        }
    }

}