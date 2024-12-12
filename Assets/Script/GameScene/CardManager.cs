using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance; // Singleton パターンの適用

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region ApplyCardEffect() - カード効果を適用する関数
    public void ApplyCardEffect(CardModel cardModel, bool isPlayerField)
    {
        switch (cardModel.effectType)
        {
            //ダメージカード処理
            case CardEffectType.Damage:
                ApplyDamage(cardModel.effectValue, isPlayerField);
                break;

                //
            case CardEffectType.Protect:
                ApplyProtect(cardModel.effectValue, isPlayerField);
                break;

                //
            case CardEffectType.DrawCard:
                ApplyDrawCard(cardModel.effectValue, isPlayerField);
                break;

                //
            case CardEffectType.Researcher:
                ApplyResearcher(cardModel.effectValue, isPlayerField);
                break;

            default:
                Debug.LogWarning("未対応のカード効果: " + cardModel.effectType);
                break;
        }
    }
    #endregion

    #region ApplyDamage() - ダメージカードの処理
    private void ApplyDamage(int damage, bool isPlayerField)
    {
        if (GameManager.instance.canUseDamageCardThisTurn)
        {
            DamageManager.instance.StartDamageProcess(!isPlayerField, damage);
            GameManager.instance.canUseDamageCardThisTurn = false; // 1ターン1回のみ使用可能
        }
        else
        {
            Debug.LogWarning("ダメージカードは1ターンに1回しか使用できません");
        }
    }
    #endregion

    #region ApplyProtect() - ガードカードの処理
    private void ApplyProtect(int protectValue, bool isPlayerField)
    {
        Debug.Log($"ガードカードの効果: {protectValue} ポイントを軽減");
        DamageManager.instance.UseProtectCard(protectValue);
    }
    #endregion

    #region ApplyDrawCard() - カード引きの処理
    private void ApplyDrawCard(int drawAmount, bool isPlayerField)
    {
        Debug.Log($"カードの効果: {drawAmount} 枚のカードを引く");
        if (isPlayerField)
        {
            GameManager.instance.DrawCard(GameManager.instance.playerHand, GameManager.instance.playerDeck, drawAmount);
            GameManager.instance.EndTurnForAllCards(GameManager.instance.playerField, GameManager.instance.playerGraveyard);
        }
        else
        {
            GameManager.instance.DrawCard(GameManager.instance.enemyHand, GameManager.instance.enemyDeck, drawAmount);
            GameManager.instance.EndTurnForAllCards(GameManager.instance.enemyField, GameManager.instance.enemyGraveyard);
        }
    }
    #endregion

    #region ApplyResearcher() - 研究者の固有能力の処理
    private void ApplyResearcher(int drawAmount, bool isPlayerField)
    {

    }
    #endregion
}
