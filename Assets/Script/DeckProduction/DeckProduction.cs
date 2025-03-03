using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DeckProduction : MonoBehaviour
{
    [SerializeField] private DeckCardView cardView;
    [SerializeField] private SetDeck setDeck;
    [SerializeField] private Transform DeckPanel;
    [SerializeField] private Transform PossessionPanel;
    List<int> DeckRegister = new List<int>();         // デッキの保存用リスト
    List<int> PossessionCard = new List<int>();     // 自分の持っているカード
    List<int> ViewCard = new List<int>();              // デッキのカードを全体のカードから引いたリスト


    void Start()
    {
        DeckRegister = DataSaveManager.LoadDeckList(); // 現在のデッキデータを取得
        PossessionCard = DataSaveManager.LoadPossessionCard();// 現在の所持カードを取得
        ViewCard = SubtractList(PossessionCard, DeckRegister);// 使っていないカードをリストに格納

        // カードの表示
        cardView.DisplayCards(ViewCard, PossessionPanel);
        cardView.DisplayCards(DeckRegister, DeckPanel);
        //cardView.RemoveDisplayedCards(DeckPanel);
    }

    #region SaveDeck()　デッキを保存
    public void SaveDeck()
    {
        DeckRegister.Clear();
        List<int> deckList = setDeck.deck;
        deckList.Insert(0, setDeck.leader);
        foreach (int deck in deckList)
        {
            DeckRegister.Add(deck);
        }
        DataSaveManager.SaveDeckList(DeckRegister);
    }
    #endregion

    #region ReadDeck() デッキを読み込む
    public void ReadDeck()
    {
        List<int> readdeck = new List<int>();
        readdeck = DataSaveManager.LoadDeckList();
        Debug.Log("読み込んだデッキ：" + string.Join(", ", readdeck));
    }
    #endregion

    #region SubtractList() 使っていないカードを調べる
    private List<int> SubtractList(List<int> listA, List<int> listB)
    {
        List<int> listC = new List<int>(listA);
        foreach (int b in listB)
        {
            if (listC.Contains(b))
                listC.Remove(b);
        }
        return listC;
    }
    #endregion
}
