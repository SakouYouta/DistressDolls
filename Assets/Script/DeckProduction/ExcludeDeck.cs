using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
public class ExcludeDeck : MonoBehaviour, IDropHandler
{
    private List<int> deck = new List<int>();
    [SerializeField] private SetDeck setDeck;
    [SerializeField] private Transform deckPanel;
    [SerializeField] private Transform PossessionPanel;

    #region OnDrop() カードがドロップされた際に呼ばれる
    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null && cardMove.cardParent == deckPanel)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                CardModel cardModel = cardController.model;
                if (cardModel != null)// カードIDを削除
                {
                    deck = setDeck.deck;
                    deck.Remove(cardModel.cardId);
                    setDeck.deck = deck;
                    Debug.Log("デッキ削除" + string.Join(", ", deck));
                    Debug.Log("削除したかーどID" + cardModel.cardId);
                    Debug.Log("削除後のデッキの数" + deck.Count);
                }
                else
                    Debug.Log("カードモデル情報の取得に失敗しました");
            }
            else Debug.Log("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = PossessionPanel;
        }
        else if (cardMove == null)
            Debug.Log("カードムーブ情報の取得に失敗しました");
        else if (cardMove.cardParent == PossessionPanel)
            Debug.Log("カードの親情報の取得に失敗しました");
    }
    #endregion
}
