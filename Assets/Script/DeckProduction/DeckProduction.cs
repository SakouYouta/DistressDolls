using System.Collections.Generic;
using UnityEngine;

public class DeckProduction : MonoBehaviour
{
    [SerializeField] private MyCardView cardView;
    List<int> DebugCardID = new List<int> { 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 };
    List<int> deck1 = new List<int>();// デッキ1を格納するリスト

    void Start()
    {
        cardView.DisplayCards(DebugCardID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
