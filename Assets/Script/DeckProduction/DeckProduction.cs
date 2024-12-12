using System.Collections.Generic;
using UnityEngine;

public class DeckProduction : MonoBehaviour
{
    [SerializeField] private MyCardView cardView;
    [SerializeField] private SetDeck setDeck;
    List<int> DeckRegister = new List<int>();// デッキの保存用リスト
    List<int> PossessionCard = new List<int>();// 自分の持っているカード

    void Start()
    {
        PossessionCard = DataSaveManager.LoadPossessionCard();
        cardView.DisplayCards(PossessionCard);    
    }

    void Update()
    {

    }

    // デッキを保存する関数
    public void SaveDeck()
    {
        DeckRegister = setDeck.GetDeck();// 仮登録のデッキを持ってくる
        Debug.Log("デッキリスト：" + string.Join(", ", DeckRegister));
        DataSaveManager.SaveDeckList(DeckRegister);
    }

    public void ReadDeck()
    {
        DataSaveManager.LoadDeckList();
    }
}
