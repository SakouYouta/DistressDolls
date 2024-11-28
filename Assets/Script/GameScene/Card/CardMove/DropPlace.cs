using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か

    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMovement = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            CardModel cardModel = cardMovement.GetComponent<CardController>().model;

            // 所有者チェック: プレイヤーフィールドならプレイヤーのカードのみ、エネミーフィールドならエネミーのカードのみ
            if (isPlayerField && cardModel.isPlayerCard || !isPlayerField && !cardModel.isPlayerCard)
            {
                // 正しいフィールドにカードを配置
                cardMovement.cardParent = this.transform;

                // カード効果の適用処理（既存のコード）
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
                        // カード引き処理
                        Debug.Log("カードの効果: カードを引く");
                        int drawAmount = cardModel.effectValue; // カード効果の値を引く枚数として使用
                        if (isPlayerField)
                        {
                            // プレイヤーフィールドに配置された場合、カードを引く
                            GameManager.instance.DrawCard(GameManager.instance.playerHand, drawAmount);
                        }
                        else
                        {
                            // エネミーフィールドに配置された場合、カードを引く
                            GameManager.instance.DrawCard(GameManager.instance.enemyHand, drawAmount);
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
                //Destroy(cardMovement.gameObject);

            }
            else
            {
                Debug.LogWarning("このフィールドにはカードを配置できません。");
            }
        }

    }
}
