using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance; // Singleton パターンの適用
    // 次の攻撃ボーナスの情報を保持
    public int playerNextAttackBonus = 0;
    public int enemyNextAttackBonus = 0;

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

            //ガードカード処理
            case CardEffectType.Protect:
                ApplyProtect(cardModel.effectValue, isPlayerField);
                break;

            //ドローカード処理
            case CardEffectType.DrawCard:
                ApplyDrawCard(cardModel.effectValue, isPlayerField);
                break;

            //固有能力処理
            case CardEffectType.Researcher:
                ApplyResearcher(cardModel.effectValue, isPlayerField);
                break;

            //固有能力処理
            case CardEffectType.Angel:
                ApplyAngel(cardModel.effectValue, isPlayerField);
                break;

            //固有能力処理
            case CardEffectType.Witch:
                ApplyWitch(cardModel.effectValue, isPlayerField);
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
    private void ApplyResearcher(int effectValue, bool isPlayerField)
    {
        Debug.Log($"研究者の効果: {effectValue} ダメージを自身に与える");

        // 自身にダメージを与える
        DamageManager.instance.StartDamageProcess(isPlayerField, effectValue);

        // 次の攻撃のダメージを増加させる
        Debug.Log($"次の攻撃のダメージが {effectValue} 増加します");
        IncreaseNextAttackDamage(isPlayerField, effectValue);
    }
    #endregion

    #region ApplyAngel() - 天使の固有能力の処理
    private void ApplyAngel(int effectValue, bool isPlayerField)
    {
        Debug.Log($"天使の効果: {effectValue} ダメージを与え、次の攻撃を軽減する");

        // 敵にダメージを与える
        DamageManager.instance.StartDamageProcess(!isPlayerField, effectValue);

        // 次の攻撃のダメージを軽減する
        Debug.Log($"次の攻撃が {effectValue} ポイント軽減されます");
        ReduceNextAttackDamage(isPlayerField, effectValue);
    }
    #endregion

    #region ApplyWitch() - 魔女の固有能力の処理
    private void ApplyWitch(int effectValue, bool isPlayerField)
    {
        Debug.Log($"魔女の効果: {effectValue} に基づく特殊な処理を実行");

        // 現時点では処理を定義する必要があるため、ロジックを追加してください
        // TODO: 魔女の能力ロジックを実装
    }
    #endregion

    #region IncreaseNextAttackDamage() - 次の攻撃のダメージを増加させる
    private void IncreaseNextAttackDamage(bool isPlayerField, int value)
    {
        if (isPlayerField)
        {
            playerNextAttackBonus += value;
            Debug.Log($"プレイヤーの次の攻撃ダメージが {value} 増加しました (合計: {playerNextAttackBonus})");
        }
        else
        {
            enemyNextAttackBonus += value;
            Debug.Log($"敵の次の攻撃ダメージが {value} 増加しました (合計: {enemyNextAttackBonus})");
        }
    }
    #endregion

    #region ReduceNextAttackDamage() - 次の攻撃のダメージを軽減する
    private void ReduceNextAttackDamage(bool isPlayerField, int value)
    {
        if (isPlayerField)
        {
            playerNextAttackBonus -= value;
            Debug.Log($"プレイヤーの次の攻撃ダメージが {value} 軽減されました (合計: {playerNextAttackBonus})");
        }
        else
        {
            enemyNextAttackBonus -= value;
            Debug.Log($"敵の次の攻撃ダメージが {value} 軽減されました (合計: {enemyNextAttackBonus})");
        }
    }
    #endregion

    #region ResetNextAttackBonus() - 次の攻撃ボーナスをリセット
    private void ResetNextAttackBonus(bool isPlayerField)
    {
        if (isPlayerField)
        {
            playerNextAttackBonus = 0;
            Debug.Log("プレイヤーの次の攻撃ボーナスがリセットされました");
        }
        else
        {
            enemyNextAttackBonus = 0;
            Debug.Log("敵の次の攻撃ボーナスがリセットされました");
        }
    }
    #endregion
}
