using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using DG.Tweening;


public class CardBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Text cardNameText = default;
    [SerializeField]
    private Text descriptionText = default;
    [SerializeField]
    private Text damageText = default;
    [SerializeField]
    private Text healthText = default;
    [SerializeField]
    private Text manaText = default;
    public Image image = default;
    public Image borderImage = default;
    public Text counterAnim;

    public float deltaWhileChange = 350f;
    public float cardMovingTime = 2f;
    [HideInInspector]
    public bool wasPlayed = false;
    private int damageValue;
    private int manaValue;
    private int healthValue;


    public Action<CardBehaviourScript> OnDestroy = delegate { };

    public List<CardScriptableObject> CardInfoList;
    private CardScriptableObject CardInfo;
    public int CardInfoChoose = 0;

    public GameEventSO RecalcPos;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(LoadTextureFromServer("https://picsum.photos/250/200"));

        if (CardInfoChoose == -1)
            CardInfoChoose = UnityEngine.Random.Range(0,5);
        CardInfo = CardInfoList[CardInfoChoose];
        if (CardInfo)
        {
            if (cardNameText)
                cardNameText.text = CardInfo.cardName;
            if (descriptionText)
                descriptionText.text = CardInfo.description;
            if (damageText)
            { 
                damageText.text = CardInfo.damage.ToString();
                damageValue = CardInfo.damage;
            }
            if (healthText)
            { 
                healthText.text = CardInfo.health.ToString();
                healthValue = CardInfo.health;
            }
            if (manaText)
            { 
                manaText.text = CardInfo.mana.ToString();
                manaValue = CardInfo.mana;
            }
            //if (image)
              //  image.sprite = CardInfo.image;
        }
    }


    public void UpdateCardInfo() 
    {
        damageText.text = damageValue.ToString();
        healthText.text = healthValue.ToString();
        manaText.text = manaValue.ToString();
    }

    IEnumerator LoadTextureFromServer(string url)
    {
        var request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (!request.isHttpError && !request.isNetworkError)
        {
           var response = DownloadHandlerTexture.GetContent(request);
           Texture2D tex = response;
           image.sprite = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        else
        {
            Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);

        }

        request.Dispose();
    }

    //Pos Setting////////////////

    public void Move(Vector3 pos)
    {
        transform.DOLocalMove(pos, cardMovingTime);
    }

    public void Rotate(float ang) 
    {
        transform.DORotate(new Vector3(0,0,-ang), cardMovingTime);
    }



    //Changing Value////////////////

    public void ChangeValue(int value, int type)
    {
        StartCoroutine(ChangingValue(value, type));
     
    }

    IEnumerator ChangingValue(int value, int type) 
    {
        this.transform.rotation = new Quaternion(0, 0, 0, 1);
        Move(new Vector3 (this.transform.localPosition.x, deltaWhileChange, 0));
        yield return new WaitForSeconds(0.5f);
        switch (type)
        {
            case 1:
                int diff = value - healthValue;
                healthValue = value;
                InitAnimCounter(diff, 4, new Color(0.8f,0.1f,0.1f));
                break;
            case 2:
                diff = value - damageValue;
                damageValue = value;
                InitAnimCounter(diff, 5, new Color(0.8f, 0.8f, 0.8f));
                break;
            case 3:
                diff = value - manaValue;
                manaValue = value;
                InitAnimCounter(diff, 3, new Color(0.1f, 0.1f, 0.8f));
                break;

        }
        UpdateCardInfo();
        yield return new WaitForSeconds(0.5f);
        if (healthValue <= 0)
        {
            DestroyThisCard();
        }
        if (this.isActiveAndEnabled)
            RecalcPos.Raise();
    }


    void InitAnimCounter(int tmp, int index, Color col) 
    {
        string tmpText = tmp.ToString();
        if (tmp > 0)
            tmpText = "+" + tmp.ToString();
        Text textValue = (Text)Instantiate(counterAnim, this.transform.GetChild(index).transform.GetChild(0).transform.position, this.transform.GetChild(index).transform.rotation);
        textValue.transform.SetParent(this.transform);
        textValue.text = tmpText;
        textValue.transform.localScale = this.transform.GetChild(index).transform.GetChild(0).transform.localScale;
        textValue.color = col;
    }



    //Destroy/////////////////////


    void DestroyThisCard() 
    {
        StopAllCoroutines();
        OnDestroy.Invoke(this);
    }



}
