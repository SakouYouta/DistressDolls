using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand,enemyHand, playerField, enemyField;
    [SerializeField] Text playerHPText, enemyHPText;

    public int playerHP, enemyHP;

    public bool isPlayerTurn = true; //
    List<int> deck = new List<int>() { 1, 2, 3, 1, 1, 2, 2, 3, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };  //

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartGame();
    }

    void StartGame() // 初期値の設定 
    {
        playerHP = 20;
        enemyHP = 20;

        Shuffle();

        // 初期手札を配る
        SetStartHand();

        // ターンの決定
        TurnCalc();
    }

    void Shuffle()
    {
        int n = deck.Count;

        while(n > 1)
        {
            n--;

            int k = UnityEngine.Random.Range(0, n + 1);

            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    void DrawCard(Transform hand) // カードを引く
    {
        // デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 9)
        {
            // デッキの一番上のカードを抜き取り、手札に加える
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }
    }

    void SetStartHand() // 手札を3枚配る
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
            DrawCard(enemyHand);
        }
    }

    void TurnCalc() // ターンを管理する
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }

    public void ChangeTurn() // ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn; // ターンを逆にする
        TurnCalc(); // ターンを相手に回す
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        DrawCard(playerHand); // 手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        // 敵手札のカードを取得
        CardController[] enemyHandCardList = enemyHand.GetComponentsInChildren<CardController>();

        // 手札にカードがある場合、1枚をフィールドに移動
        if (enemyHandCardList.Length > 0)
        {
            // 最初のカードを選択（またはランダムなカードでも可）
            CardController cardToPlay = enemyHandCardList[0];

            // フィールドのスロットが空いているかチェック
            if (enemyField.childCount < 5) // 最大フィールド数を5と仮定
            {
                cardToPlay.transform.SetParent(enemyField); // カードをフィールドに移動
                //Debug.Log($"敵がカード {cardToPlay.cardID} をフィールドに出しました");
            }
            else
            {
                Debug.Log("敵フィールドが満杯のためカードをプレイできません");
            }
        }

        // 新しいカードを敵の手札に追加（最大手札数9を超えないようにする）
        if (enemyHand.childCount < 9)
        {
            DrawCard(enemyHand);
        }

        // ターン終了
        ChangeTurn();
    }


    public void DecreaseHP(bool isPlayer, int damage)
    {
        if (isPlayer)
        {
            playerHP -= damage;
            Debug.Log($"プレイヤーのHPが {damage} 減少しました: 残りHP {playerHP}");
            if (playerHP <= 0)
            {
                EndGame(false);
            }
        }
        else
        {
            enemyHP -= damage;
            Debug.Log($"エネミーのHPが {damage} 減少しました: 残りHP {enemyHP}");
            if (enemyHP <= 0)
            {
                EndGame(true);
            }
        }

        // HPのUIを更新
        ShowLeaderHP();
    }

    public void ShowLeaderHP()
    {
        if (playerHP <= 0)
        {
            playerHP = 0;
        }
        if (enemyHP <= 0)
        {
            enemyHP = 0;
        }

        playerHPText.text = playerHP.ToString();
        enemyHPText.text = enemyHP.ToString();
    }

    private void EndGame(bool isPlayerWinner)
    {
        if (isPlayerWinner)
        {
            Debug.Log("ゲーム終了: プレイヤーの勝利！");
        }
        else
        {
            Debug.Log("ゲーム終了: エネミーの勝利！");
        }

        // 必要ならリスタートやシーン遷移処理を追加
    }
}