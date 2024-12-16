using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class DeckCardView : MonoBehaviour
{
    [SerializeField] private CardController cardPrefab;

    #region DisplayCards() カードを表示する
    public void DisplayCards(List<int> cardIds, Transform panel)
    {
        foreach (int cardID in cardIds)
        {
            CardController card = Instantiate(cardPrefab, panel);
            card.Init(cardID);// カードを表示
        }
    }
    #endregion
}
