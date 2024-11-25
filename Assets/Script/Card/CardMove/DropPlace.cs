using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か

    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        CardMovement cardMovement = eventData.pointerDrag.GetComponent<CardMovement>(); // ドラッグしてきた情報からCardMovementを取得
        if (cardMovement != null) // もしカードがあれば
        {
            cardMovement.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする

            // カードの効果を取得
            CardModel cardModel = cardMovement.GetComponent<CardController>().model;

            // カード効果に基づく処理
            switch (cardModel.effectType)
            {
                case CardEffectType.Damage:
                    // ダメージ処理
                    int damage = cardModel.effectValue; // effectValue はダメージ量
                    if (isPlayerField)
                    {
                        // プレイヤーフィールドに配置された場合、エネミーにダメージ
                        GameManager.instance.DecreaseHP(false, damage);
                    }
                    else
                    {
                        // エネミーフィールドに配置された場合、プレイヤーにダメージ
                        GameManager.instance.DecreaseHP(true, damage);
                    }
                    break;

                case CardEffectType.Protect:
                    // 保護処理（仮の処理としてログ出力）
                    Debug.Log("カードの効果: ダメージをおさえる");
                    // ここに保護処理を追加（ダメージを軽減するなど）
                    break;

                case CardEffectType.DrawCard:
                    // カード引き処理
                    Debug.Log("カードの効果: カードを引く");
                    if (isPlayerField)
                    {
                        // プレイヤーフィールドに配置された場合、カードを引く
                        GameManager.instance.DrawCard(GameManager.instance.playerHand);
                    }
                    else
                    {
                        // エネミーフィールドに配置された場合、カードを引く
                        GameManager.instance.DrawCard(GameManager.instance.enemyHand);
                    }
                    break;

                default:
                    Debug.LogWarning("未対応のカード効果");
                    break;
            }

            // カードが使用された後、墓地に移動させる
            if (isPlayerField)
            {
                // プレイヤーフィールドに配置された場合、カードをプレイヤーの墓地に移動
                cardMovement.transform.SetParent(GameManager.instance.playerGraveyard);
            }
            else
            {
                // エネミーフィールドに配置された場合、カードをエネミーの墓地に移動
                cardMovement.transform.SetParent(GameManager.instance.enemyGraveyard);
            }

            // カードを削除する
            Destroy(cardMovement.gameObject);
        }
    }
}
