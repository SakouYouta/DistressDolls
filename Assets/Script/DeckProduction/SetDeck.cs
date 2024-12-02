using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetDeck : MonoBehaviour, IDropHandler
{
    private List<int> deck = new List<int>();

    public void OnDrop(PointerEventData eventData)
    {
        // ドラッグしているオブジェクトから CardMovement を取得
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null)
        {
            // ドロップされたカードの CardController を取得
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                // カードのモデル情報を取得
                CardModel cardModel = cardController.model;
                if (cardModel != null)
                {
                    // カードIDを取得
                    deck.Add(cardModel.cardId);
                    Debug.Log("デッキリスト：" + string.Join(", ", deck));
                }
                else
                    Debug.LogError("カードモデル情報の取得に失敗しました");
            }
            else
                Debug.LogError("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = this.transform;
        }
        else
            Debug.LogError("カードムーブ情報の取得に失敗しました");
    }
}
