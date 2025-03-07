using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
public class ExcludeDeck : MonoBehaviour, IDropHandler
{
    private List<int> deck = new List<int>();
    [SerializeField] private SetDeck setDeck;
    [SerializeField] private Transform deckPanel;
    [SerializeField] private Transform PossessionPanel;
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] public GameObject AnimationField;
    private CardModel cardModel;

    void start()
    {

    }

    #region OnDrop() カードがドロップされた際に呼ばれる
    public void OnDrop(PointerEventData eventData)
    {
        Transform card = eventData.pointerDrag.transform;
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        deck = setDeck.deck;
        if (cardMove != null && cardMove.cardParent == deckPanel)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null && deck.Count > 1)
            {
                cardModel = cardController.model;
                if (cardModel != null && deck.Count <= 30)// カードIDを削除
                {
                    deck.Remove(cardModel.cardId);
                    setDeck.deck = deck;
                    Debug.Log("デッキ削除" + string.Join(", ", deck));
                    Debug.Log("削除したかーどID" + cardModel.cardId);
                    Debug.Log("削除後のデッキの数" + deck.Count);
                    StartCoroutine(Animation(cardMove, cardModel));
                }
                else
                {
                    Debug.Log("カードモデル情報の取得に失敗しました");
                    Debug.Log("カードが30枚以下になってしまいました!");
                    //SetDeck.OpenWarningPanel();
                }
            }
            else Debug.Log("カードコントローラー情報の取得に失敗しました");
        }
        else if (cardMove == null)
            Debug.Log("カードムーブ情報の取得に失敗しました");
        else if (cardMove.cardParent == PossessionPanel)
            Debug.Log("カードの親情報の取得に失敗しました");
    }
    #endregion

    private IEnumerator Animation(CardMovement cardMove, CardModel cardModel)
    {
        yield return StartCoroutine(cardAnimation.RotateCardAnimation(cardModel.cardId, PossessionPanel, AnimationField));
        Destroy(cardMove.gameObject);
    }
}