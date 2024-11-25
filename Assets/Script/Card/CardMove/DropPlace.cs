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
            CardModel cardModel = cardMovement.GetComponent<CardController>().model;

            // フィールドの種類とカードの所有者を確認
            if (isPlayerField && cardModel.isPlayerCard || !isPlayerField && !cardModel.isPlayerCard)
            {
                // 正しいフィールドにカードを配置
                cardMovement.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする

                // カード効果に基づく処理
                switch (cardModel.effectType)
                {
                    case CardEffectType.Damage:
                        int damage = cardModel.effectValue;
                        if (isPlayerField)
                        {
                            GameManager.instance.DecreaseHP(false, damage);
                        }
                        else
                        {
                            GameManager.instance.DecreaseHP(true, damage);
                        }
                        break;

                    case CardEffectType.Protect:
                        Debug.Log("カードの効果: ダメージをおさえる");
                        break;

                    case CardEffectType.DrawCard:
                        Debug.Log("カードの効果: カードを引く");
                        if (isPlayerField)
                        {
                            GameManager.instance.DrawCard(GameManager.instance.playerHand);
                        }
                        else
                        {
                            GameManager.instance.DrawCard(GameManager.instance.enemyHand);
                        }
                        break;

                    default:
                        Debug.LogWarning("未対応のカード効果");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("このフィールドには配置できません。");
            }
        }
    }

}
