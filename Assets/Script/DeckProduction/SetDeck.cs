using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetDeck : MonoBehaviour, IDropHandler
{
    private List<int> deck = new List<int>();

    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                CardModel cardModel = cardController.model;
                if (cardModel != null)// カードIDを登録
                    deck.Add(cardModel.cardId);
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

    public List<int> GetDeck()
    {
        return new List<int>(deck);
    }
}
