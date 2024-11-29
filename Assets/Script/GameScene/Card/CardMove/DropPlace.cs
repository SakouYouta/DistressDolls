using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か
    private bool canUseDamageCardThisTurn = true; // 1ターンでダメージカードを1回のみ使用可能
    private bool isDamageDelayed = false; // ダメージ遅延中かどうか
    private float damageDelayTime = 3f; // ダメージ遅延時間（3秒）
    private float damageDelayTimer = 0f; // 遅延タイマー

    private int damageToApply = 0; // 実際に適用するダメージ
    private bool isDamageCardUsed = false; // ダメージカードが使われたかどうか

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

                // ダメージカードの処理
                if (cardModel.effectType == CardEffectType.Damage && canUseDamageCardThisTurn)
                {
                    // ダメージカードが使われた
                    isDamageCardUsed = true;

                    // ダメージ発生を3秒遅延させる
                    isDamageDelayed = true;
                    damageDelayTimer = damageDelayTime;

                    // カードに設定されたダメージ値を取得
                    damageToApply = cardModel.effectValue;

                    // ダメージカード使用フラグを1ターン内で1回しか使えないように
                    canUseDamageCardThisTurn = false;

                    // カードを墓地に移動（使用された後）
                    if (isPlayerField)
                    {
                        cardMovement.transform.SetParent(GameManager.instance.playerGraveyard);
                    }
                    else
                    {
                        cardMovement.transform.SetParent(GameManager.instance.enemyGraveyard);
                    }
                }
                else
                {
                    // 他のカード効果の処理
                    ApplyCardEffect(cardModel);
                }
            }
            else
            {
                Debug.LogWarning("このフィールドにはカードを配置できません。");
            }
        }
    }

    void Update()
    {
        // ダメージの遅延処理（3秒経過を待つ）
        if (isDamageDelayed)
        {
            damageDelayTimer -= Time.deltaTime; // タイマーを減らしていく

            if (damageDelayTimer <= 0f)
            {
                // ダメージが遅延後に適用される
                ApplyDelayedDamage();
                isDamageDelayed = false; // ダメージ遅延終了
            }
        }
    }

    // ダメージカードの遅延後に適用される処理
    void ApplyDelayedDamage()
    {
        // 手札のガードカードを確認して、ダメージ軽減処理を行う
        Transform hand = isPlayerField ? GameManager.instance.playerHand : GameManager.instance.enemyHand;
        int damageReduction = ApplyProtectCard(hand);

        // 最終的なダメージを計算（軽減後）
        int finalDamage = damageToApply - damageReduction; // カードに設定されたダメージ値を使用

        // 最終的なダメージが0未満になるのを防ぐ
        if (finalDamage < 0)
        {
            finalDamage = 0; // ダメージが0未満にならないように設定
        }

        // 軽減されたダメージをログに表示
        Debug.Log("元のダメージ: " + damageToApply + ", ガードカードで軽減されたダメージ: " + damageReduction + ", 最終ダメージ: " + finalDamage);

        if (isPlayerField)
        {
            // プレイヤーのターンの場合
            GameManager.instance.DecreaseHP(false, finalDamage);
        }
        else
        {
            // エネミーのターンの場合
            GameManager.instance.DecreaseHP(true, finalDamage);
        }
    }

    // ガードカードを適用するメソッド
    private int ApplyProtectCard(Transform hand)
    {
        int damageReduction = 0;
        bool protectCardUsed = false; // Protectカードが使用されたかを追跡

        // 手札にあるカードを確認
        CardController[] handCards = hand.GetComponentsInChildren<CardController>();

        foreach (CardController card in handCards)
        {
            // 最初のProtectカードが見つかればその効果を適用
            if (card.model.effectType == CardEffectType.Protect && !protectCardUsed)
            {
                // Protectカードを使う場合
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    damageReduction += card.model.effectValue; // Protectカードの効果値を軽減に使用
                    protectCardUsed = true; // Protectカードが使用されたとマーク
                    Destroy(card.gameObject); // Protectカードを消費
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    // ガードカードを使用しない
                    Debug.Log("Protectカードは使用されませんでした");
                    protectCardUsed = true; // 使わないときもループを終了
                }
            }
        }

        return damageReduction; // 軽減されたダメージ値を返す
    }

    // カード効果の処理（ダメージ以外）
    void ApplyCardEffect(CardModel cardModel)
    {
        switch (cardModel.effectType)
        {
            case CardEffectType.Protect:
                Debug.Log("カードの効果: ダメージをおさえる");
                break;

            case CardEffectType.DrawCard:
                // カード引き処理
                Debug.Log("カードの効果: カードを引く");
                int drawAmount = cardModel.effectValue; // カード効果の値を引く枚数として使用
                if (isPlayerField)
                {
                    GameManager.instance.DrawCard(GameManager.instance.playerHand, drawAmount);
                }
                else
                {
                    GameManager.instance.DrawCard(GameManager.instance.enemyHand, drawAmount);
                }
                break;

            default:
                Debug.LogWarning("未対応のカード効果");
                break;
        }
    }
}

