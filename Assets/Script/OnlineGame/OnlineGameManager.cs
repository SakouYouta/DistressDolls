using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnlineGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CardController cardPrefab;
    public Transform player1Hand, player2Hand, player1Field, player2Field, player1Graveyard, player2Graveyard;
    [SerializeField] Text player1HPText, player2HPText;
    public int player1HP = 10;
    public int player2HP = 10;
    public bool isPlayer1Turn = true;
    public bool turnEnd = false;

    public List<int> player1Deck, player2Deck;
    public static OnlineGameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // デバッグ用ログ出力
        Debug.Log($"player1Hand: {player1Hand}");
        Debug.Log($"player2Hand: {player2Hand}");
        Debug.Log($"player1Deck: {player1Deck?.Count}");
        Debug.Log($"player2Deck: {player2Deck?.Count}");

        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    // ゲームの初期化処理
    public void StartGame()
    {
        // プレイヤー1とプレイヤー2のデッキを設定
        if (PhotonNetwork.IsMasterClient)  // ホスト（マスタークライアント）側
        {
            player1Deck = DataSaveManager.LoadDeckList();  // プレイヤー1のデッキ
            player2Deck = new List<int>() { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };  // プレイヤー2のデッキ
        }
        else  // ゲスト（接続したプレイヤー）側
        {
            player1Deck = new List<int>() { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };  // プレイヤー2のデッキ
            player2Deck = DataSaveManager.LoadDeckList();  // プレイヤー1のデッキ
        }

        Shuffle(player1Deck);
        Shuffle(player2Deck);

        SetStartHand();
        UpdateHPUI();
        TurnCalc();
    }

    // デッキをシャッフルする
    void Shuffle(List<int> deck)
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    // カードを生成して手札に追加 (同期付き)
    [PunRPC]
    public void CreateCard(int cardID, string targetHand)
    {
        Transform hand = targetHand == "player1" ? player1Hand : player2Hand;

        if (hand == null)
        {
            Debug.LogError($"CreateCard failed: Hand is null. Target hand: {targetHand}");
            return;
        }

        CardController card = Instantiate(cardPrefab, hand);
        card.Init(cardID);
        card.model.isPlayerCard = hand == player1Hand;
    }

    // 手札にカードを引く (同期付き)
    public void DrawCard(Transform hand, List<int> deck, int drawAmount = 1)
    {
        if (hand == null)
        {
            Debug.LogError("DrawCard failed: Hand is null.");
            return;
        }

        if (deck == null)
        {
            Debug.LogError("DrawCard failed: Deck is null.");
            return;
        }

        if (deck.Count == 0)
        {
            Debug.LogWarning("DrawCard failed: Deck is empty.");
            return;
        }

        CardController[] handCardList = hand.GetComponentsInChildren<CardController>();
        if (handCardList.Length >= 9)
        {
            Debug.LogWarning("DrawCard skipped: Hand already has 9 cards.");
            return;
        }

        for (int i = 0; i < drawAmount; i++)
        {
            if (deck.Count == 0) break;

            int cardID = deck[0];
            deck.RemoveAt(0);

            string targetHand = hand == player1Hand ? "player1" : "player2";
            photonView.RPC("CreateCard", RpcTarget.All, cardID, targetHand);
        }
    }

    // 初期手札の配布
    void SetStartHand()
    {
        if (player1Hand == null || player2Hand == null)
        {
            Debug.LogError("Player hands are not initialized.");
            return;
        }

        // プレイヤー1とプレイヤー2にカードを引かせる
        if (PhotonNetwork.IsMasterClient)  // プレイヤー1はホスト（マスタークライアント）
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCard(player2Hand, player2Deck);
                DrawCard(player1Hand, player1Deck); 
            }
        }
        else  // プレイヤー2はゲスト
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCard(player1Hand, player1Deck);
                DrawCard(player2Hand, player2Deck);
            }
        }
    }

    // ターン計算と進行
    void TurnCalc()
    {
        if (isPlayer1Turn)
        {
            if (PhotonNetwork.IsMasterClient)  // ホスト（マスタークライアント）がプレイヤー1
            {
                Player1Turn();
            }
            Debug.Log("Player 1's Turn");
            Debug.Log("Player 2's Turn is inactive");
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)  // ゲストがプレイヤー2
            {
                Player2Turn();
            }
            Debug.Log("Player 2's Turn");
            Debug.Log("Player 1's Turn is inactive");
        }
    }

    // プレイヤー1のターン
    private void Player1Turn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        DrawCard(player1Hand, player1Deck);

        if (turnEnd)
        {
            photonView.RPC("ChangeTurn", RpcTarget.All);
        }
    }

    // プレイヤー2のターン
    private void Player2Turn()
    {
        if (PhotonNetwork.IsMasterClient) return;

        DrawCard(player2Hand, player2Deck);

        if (turnEnd)
        {
            photonView.RPC("ChangeTurn", RpcTarget.All);
        }
    }

    // ターン切り替え (同期付き)
    [PunRPC]
    public void ChangeTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        turnEnd = false;
        TurnCalc();
    }

    // HP減少処理 (同期付き)
    [PunRPC]
    public void DecreaseHP(bool isPlayer1, int damage)
    {
        if (isPlayer1)
        {
            player1HP -= damage;
        }
        else
        {
            player2HP -= damage;
        }
        UpdateHPUI();
    }

    // UIのHP更新
    public void UpdateHPUI()
    {
        player1HPText.text = player1HP.ToString();
        player2HPText.text = player2HP.ToString();

        if (player1HP <= 0 || player2HP <= 0)
        {
            EndGame(player1HP > 0);
        }
    }

    // ゲーム終了処理
    private void EndGame(bool isPlayer1Winner)
    {
        Debug.Log(isPlayer1Winner ? "Player1 Wins!" : "Player2 Wins!");
    }
}
