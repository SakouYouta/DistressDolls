using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardId;
    public string name;
    public Sprite icon;

    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        cardId = cardEntity.cardId;
        name = cardEntity.name;
        icon = cardEntity.icon;
    }
}