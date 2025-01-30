using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private GameObject AnimationField;

    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null)
        {
            CardModel cardModel = cardMove.GetComponent<CardController>().model;

            // 所有者チェック: プレイヤーフィールドにはプレイヤーのカードのみ、エネミーフィールドにはエネミーのカードのみ配置できる
            if (isPlayerField && cardModel.isPlayerCard || !isPlayerField && !cardModel.isPlayerCard)
            {
                if (cardMove.cardParent == GameManager.instance.playerField)
                {
                    cardMove.drag = true;   //再配置できないように
                }
                else
                {
                    // 正しいフィールドにカードを配置する
                    StartCoroutine(Animation(cardMove, cardModel));
                    // カード効果を適用 (CardManagerに委譲)
                    CardManager.instance.ApplyCardEffect(cardModel, isPlayerField);
                }
            }
        }
    }

    #region Animation()-アニメーション用のコルーチン
    private IEnumerator Animation(CardMovement cardMove, CardModel cardModel)
    {
        yield return StartCoroutine(cardAnimation.RotateCardAnimation(cardModel.cardId, this.transform, AnimationField));
        Destroy(cardMove.gameObject);
    }
    #endregion
}
