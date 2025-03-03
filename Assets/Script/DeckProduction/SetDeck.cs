using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class SetDeck : MonoBehaviour, IDropHandler
{
    public List<int> deck = new List<int>();
    public int leader = 0;
    [SerializeField] private Transform deckPanel;
    [SerializeField] private Transform PossessionPanel;

    //外部から変更を加えられないように
    public ReadOnlyCollection<int> Deck => deck.AsReadOnly();

    void Start()
    {// デッキデータ読み込み
        deck = DataSaveManager.LoadDeckList();
    }

    #region OnDrop() カードがドロップされた際に呼ばれる
    public void OnDrop(PointerEventData eventData)
    {
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
                        cardMove.drag = false;
                        deck.Add(cardModel.cardId); // カードをデッキに追加
                        Debug.Log("デッキ追加" + string.Join(", ", deck));
                        Debug.Log("追加したかーどID" + cardModel.cardId);
                        Debug.Log("追加後のデッキの数" + deck.Count);
                    }
                    else
                    {
                        cardMove.drag = true;
                        Debug.Log("カードが30枚以上です");
                    }
                }
                else
                    Debug.LogError("カードモデル情報の取得に失敗しました");
            }
            else Debug.LogError("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = deckPanel;
        }
        else if (cardMove == null)
            Debug.Log("カードムーブ情報の取得に失敗しました");
        else if (cardMove.cardParent == PossessionPanel)
            Debug.Log("カードの親情報の取得に失敗しました");
    }
    #endregion
}
