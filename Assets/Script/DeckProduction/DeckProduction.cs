using System.Collections.Generic;
using UnityEngine;

public class DeckProduction : MonoBehaviour
{
    [SerializeField] private MyCardView cardView;
    [SerializeField] private SetDeck setDeck;
    List<int> DebugCardID = new List<int> { 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53 };// 仮のリスト
    List<int> DeckRegister = new List<int>();// デッキの保存用リスト

    void Start()
    {
        cardView.DisplayCards(DebugCardID);    
    }

    void Update()
    {

    }

    // デッキを保存する関数
    public void SaveDeck()
    {
        DeckRegister = setDeck.GetDeck();
        Debug.Log("デッキリスト：" + string.Join(", ", DeckRegister));
        JsonSaveManager.SaveDeckList(DeckRegister);
    }

    public void ReadDeck()
    {
        JsonSaveManager.LoadDeckList();
    }
}
