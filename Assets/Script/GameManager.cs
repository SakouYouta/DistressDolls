using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand,enemyHand, playerField, enemyField;

    void Start()
    {
        for (int i = 0; i < 5; i++) // ŽèŽD‚ð1–‡”z‚éˆ—‚ð5‰ñŒJ‚è•Ô‚·iŽ©•ªj
        {
            Instantiate(cardPrefab, playerHand);
            Instantiate(cardPrefab, enemyHand);
        }
    }
}