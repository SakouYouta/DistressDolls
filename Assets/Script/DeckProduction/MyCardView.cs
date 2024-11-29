using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MyCardView : MonoBehaviour
{
    [SerializeField] private CardController cardPrefab;
    [SerializeField] private Transform panel; // Panel

    public void DisplayCards(List<int> cardIds)
    {
        foreach (int cardID in cardIds)
        {
            CardController card = Instantiate(cardPrefab, panel);
            card.Init(cardID);
        }
    }
}
