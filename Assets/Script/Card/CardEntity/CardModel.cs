using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardId;
    public string name;
    public string ablity;
    public Sprite icon;
    public CardEffectType effectType;   // カード効果の種類
    public int effectValue;             // 効果の値
    public bool isPlayerCard; // true: プレイヤーのカード, false: エネミーのカード


    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);

        cardId = cardEntity.cardId;
        name = cardEntity.name;
        ablity = cardEntity.abilty;
        icon = cardEntity.icon;
        effectType = cardEntity.effectType;
        effectValue = cardEntity.effectValue;
    }
}