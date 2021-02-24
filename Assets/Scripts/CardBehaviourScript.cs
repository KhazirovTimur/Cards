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
    private Text cardNameText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Text damageText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text manaText;
    public Image image;
    public Image borderImage;
    public GameObject counterAnim;

    [SerializeField]
    private float cardSpeed = 0.3f;
    
    public float cardMovingTime = 2f;
    private bool wasMoved = false;
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

    // Update is called once per frame
    void Update()
    {
        
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

    public void ChangeValue(int value, int type)
    {
        StartCoroutine(ChangingValue(value, type));
     
    }

    IEnumerator ChangingValue(int value, int type) 
    {
        Move(this.transform.localPosition+new Vector3(0,300,0));
        yield return new WaitForSeconds(0.7f);
        switch (type)
        {
            case 1:
                int tmp = value - healthValue;
                string tmpText = tmp.ToString();
                if (tmp > 0)
                    tmpText = "+" + tmp.ToString();
                healthValue = value;
                GameObject textValue = (GameObject)Instantiate(counterAnim, this.transform.GetChild(4).transform.GetChild(0).transform.position, this.transform.GetChild(4).transform.rotation);
                textValue.transform.SetParent(this.transform);
                textValue.GetComponent<Text>().text = tmpText;
                textValue.transform.localScale = this.transform.GetChild(4).transform.GetChild(0).transform.localScale;
                break;
            case 2:
                tmp = value - damageValue;
                tmpText = tmp.ToString();
                if (tmp > 0)
                    tmpText = "+" + tmp.ToString();
                damageValue = value;
                textValue = (GameObject)Instantiate(counterAnim, this.transform.GetChild(5).transform.GetChild(0).transform.position, this.transform.GetChild(4).transform.rotation);
                textValue.transform.SetParent(this.transform);
                textValue.GetComponent<Text>().text = tmpText;
                textValue.GetComponent<Text>().color = new Color(0.8f, 0.8f, 0.8f);
                textValue.transform.localScale = this.transform.GetChild(5).transform.GetChild(0).transform.localScale;
                break;
            case 3:
                tmp = value - manaValue;
                tmpText = tmp.ToString();
                if (tmp > 0)
                    tmpText = "+" + tmp.ToString();
                manaValue = value;
                textValue = (GameObject)Instantiate(counterAnim, this.transform.GetChild(3).transform.GetChild(0).transform.position, this.transform.GetChild(4).transform.rotation);
                textValue.transform.SetParent(this.transform);
                textValue.GetComponent<Text>().text = tmpText;
                textValue.GetComponent<Text>().color = new Color(0.1f,0.1f,0.8f);
                textValue.transform.localScale = this.transform.GetChild(3).transform.GetChild(0).transform.localScale;
                break;

        }
        UpdateCardInfo();
        yield return new WaitForSeconds(0.7f);
        if (healthValue <= 0)
        {
            DestroyThisCard();
        }
        if (this.isActiveAndEnabled)
            RecalcPos.Raise();
    }


    void DestroyThisCard() 
    {
        StopAllCoroutines();
        OnDestroy.Invoke(this);
    }


    //Help func//////////////////////////////

    public float distanceCount(Vector3 a, Vector3 b)
    {
        Vector3 vec = a;
        vec -= b;
        return vec.magnitude;
    }

    public float VectorAngleSigned(Vector3 a, Vector3 b)
    {
        Vector3 tmp = new Vector3(Mathf.Abs(Vector3.Cross(a, b).x), Mathf.Abs(Vector3.Cross(a, b).y), Mathf.Abs(Vector3.Cross(a, b).z));

        return Vector3.Angle(a, b) * (Vector3.Cross(a, b).z / Mathf.Abs(tmp.z));
    }
}
