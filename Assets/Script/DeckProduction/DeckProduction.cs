using System.Collections.Generic;
using UnityEngine;

public class DeckProduction : MonoBehaviour
{
    [SerializeField] private MyCardView cardView;
    List<int> DebugCardID = new List<int> { 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53, 53 };// ‰¼‚ÌƒŠƒXƒg

    void Start()
    {
        cardView.DisplayCards(DebugCardID);
    }

    void Update()
    {

    }
}
