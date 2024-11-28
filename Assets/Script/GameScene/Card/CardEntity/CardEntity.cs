using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public int cardId;
    public new string name;
    public string abilty;
    public Sprite icon;

    public CardEffectType effectType;   // カード効果の種類（ダメージ、保護、カードを引く）
    public int effectValue;             // 効果の値（例: ダメージ量、保護量、引くカード枚数）

}