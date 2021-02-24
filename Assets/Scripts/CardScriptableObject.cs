using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card", order = 51)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName = "Name";
    public string description = "this is a card";

    
    public int health = 5;
    public int damage = 5;
    public int mana = 5;

    public Sprite image;


}
