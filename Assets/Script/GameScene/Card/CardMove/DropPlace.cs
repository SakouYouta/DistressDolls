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
                if (cardMovement.cardParent == GameManager.instance.playerField)
                {
                    CardMovement.drag = true;   //再配置できないように
                }
                else
                {
                    // 正しいフィールドにカードを配置する
                    cardMovement.cardParent = this.transform; // ドラッグ元からドロップ先に親を変更

                    // カード効果を適用 (CardManagerに委譲)
                    CardManager.instance.ApplyCardEffect(cardModel, isPlayerField);
                }

            }
        }
    }
}
