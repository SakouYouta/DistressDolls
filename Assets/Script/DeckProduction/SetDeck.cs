using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class SetDeck : MonoBehaviour, IDropHandler
{
    private List<int> deck = new List<int>();
    [SerializeField] private Transform deckPanel;
    [SerializeField] private Transform PossessionPanel;

    void Start()
    {// デッキデータ読み込み
        deck = DataSaveManager.LoadDeckList();
    }

    #region OnDrop() デッキにカードを加えた際に呼ばれる
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("呼ばれた");
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null && cardMove.cardParent == PossessionPanel)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                CardModel cardModel = cardController.model;
                if (cardModel != null)
                {// カードIDを登録
                    if (deck.Count < 30)
                    {// デッキのカードが30枚までになるように
                        CardMovement.drag = false;
                        deck.Add(cardModel.cardId);
                        Debug.Log(deck.Count);
                    }
                    else
                    {
                        CardMovement.drag = true;
                        Debug.LogError("カードが30枚以上です");
                    }
                }
                else
                    Debug.LogError("カードモデル情報の取得に失敗しました");
            }
            else Debug.LogError("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = deckPanel;
        }
        else if(cardMove != null && cardMove.cardParent == deckPanel)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                CardModel cardModel = cardController.model;
                if (cardModel != null)// カードIDを削除
                {
                    deck.Remove(cardModel.cardId);
                    Debug.Log(deck.Count);
                }
                else
                    Debug.LogError("カードモデル情報の取得に失敗しました");
            }
            else Debug.LogError("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = PossessionPanel;
        }
        else if (cardMove == null)
            Debug.Log("カードムーブ情報の取得に失敗しました");
        else if (cardMove.cardParent == PossessionPanel)
            Debug.Log("カードの親情報の取得に失敗しました");
    }
    #endregion

    #region GetDeck() デッキを渡す
    public List<int> GetDeck()
    {
        return new List<int>(deck);
    }
    #endregion
}
