using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か

    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMovement = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            CardModel cardModel = cardMovement.GetComponent<CardController>().model;

            // 所有者チェック: プレイヤーフィールドにはプレイヤーのカードのみ、エネミーフィールドにはエネミーのカードのみ配置できる
            if (isPlayerField && cardModel.isPlayerCard || !isPlayerField && !cardModel.isPlayerCard)
            {
                // 正しいフィールドにカードを配置する
                cardMovement.cardParent = this.transform;  // ドラッグ元からドロップ先に親を変更

                ApplyCardEffect(cardModel);
            }
        }
    }

    // カード効果の適用
    void ApplyCardEffect(CardModel cardModel)
    {
        switch (cardModel.effectType)
        {
            case CardEffectType.Damage:
                // ダメージカードの場合の処理
                ApplyDamage(cardModel.effectValue);
                break;

            case CardEffectType.Protect:
                // ガードカードの場合の処理
                ApplyProtect(cardModel.effectValue);
                break;

            case CardEffectType.DrawCard:
                // カード引きの処理
                DrawCard(cardModel.effectValue);
                break;

            default:
                Debug.LogWarning("未対応のカード効果");
                break;
        }
    }

    // ダメージカードの処理
    private void ApplyDamage(int damage)
    {
        // ダメージカードが使用された場合の処理
        if (GameManager.instance.canUseDamageCardThisTurn)
        {
            // ダメージ処理（DamageManagerに委譲する場合）
            DamageManager.instance.StartDamageProcess(!isPlayerField, damage);
            GameManager.instance.canUseDamageCardThisTurn = false; // 1ターンで1回のみ使用可能
        }
        else
        {
            Debug.LogWarning("ダメージカードは1ターンに1回しか使用できません");
        }
    }

    // ガードカードの処理
    private void ApplyProtect(int protectValue)
    {
        // Protectカードの処理
        Debug.Log($"ガードカードの効果: {protectValue} ポイントを軽減");

        // ここで実際にProtectカードを適用する処理が入る（例えば、DamageManagerに委譲）
        DamageManager.instance.UseProtectCard(protectValue);
    }

    // カード引きの処理
    private void DrawCard(int drawAmount)
    {
        // カード引き処理
        Debug.Log($"カードの効果: {drawAmount} 枚のカードを引く");

        if (isPlayerField)
        {
             GameManager.instance.DrawCard(GameManager.instance.playerHand,GameManager.instance.playerDeck, drawAmount);
            GameManager.instance.EndTurnForAllCards(GameManager.instance.playerField, GameManager.instance.playerGraveyard);
        }
        else
        {
            GameManager.instance.DrawCard(GameManager.instance.enemyHand, GameManager.instance.enemyDeck, drawAmount);
            GameManager.instance.EndTurnForAllCards(GameManager.instance.enemyField, GameManager.instance.enemyGraveyard);

        }
    }
}