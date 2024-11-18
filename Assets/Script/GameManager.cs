using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand,enemyHand, playerField, enemyField;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        CardController card = Instantiate(cardPrefab, playerHand);
        card.Init(1);
    }
}