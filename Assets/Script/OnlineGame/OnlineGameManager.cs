using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class OnlineGameManager : MonoBehaviourPunCallbacks
{
    // フィールドの宣言
    [SerializeField] CardController cardPrefab;
    public Transform playerHand, enemyHand, playerField, enemyField, playerGraveyard, enemyGraveyard;
    [SerializeField] Text playerHPText, enemyHPText;
    public int playerHP = 10, enemyHP = 20;

    public bool isMyTurn = false; // 自分のターンかどうか
    private bool isGameOver = false;

    public List<int> playerDeck;

    public static OnlineGameManager instance;

    // Awake() - インスタンスの初期化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region StartGame() - ゲーム開始時の初期設定
    public void StartGame()
    {
        Debug.Log("ゲーム開始");

        // ホストが先攻
        if (PhotonNetwork.IsMasterClient)
        {
            isMyTurn = true;
            photonView.RPC("SyncTurn", RpcTarget.Others, true); // 相手にターン情報を通知
        }
        else
        {
            isMyTurn = false;
        }

        // HPの初期値設定
        playerHP = 10;
        enemyHP = 20;

        // デッキの初期化
        playerDeck = DataSaveManager.LoadDeckList();
        Shuffle(playerDeck);

        // 初期手札を配布
        SetStartHand();
        ShowLeaderHP();
    }
    #endregion

    #region Shuffle() - デッキをシャッフル
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
    #endregion

    #region SetStartHand() - 初期手札を配布
    void SetStartHand()
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand, playerDeck);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("DrawCardRPC", RpcTarget.Others, i);
            }
        }
    }
    #endregion

    #region DrawCard() - 手札にカードを引く処理
    public void DrawCard(Transform hand, List<int> deck, int drawAmount = 1)
    {
        if (deck.Count == 0) return; // デッキが空なら引かない

        CardController[] handCardList = hand.GetComponentsInChildren<CardController>();
        if (handCardList.Length < 9) // 手札が9枚未満の場合に引く
        {
            for (int i = 0; i < drawAmount; i++)
            {
                if (deck.Count == 0) break;

                int cardID = deck[0];
                deck.RemoveAt(0);
                CreateCard(cardID, hand);
            }
        }
    }

    [PunRPC]
    private void DrawCardRPC(int index)
    {
        DrawCard(enemyHand, playerDeck); // 敵もカードを引く（サーバー同期）
    }
    #endregion

    #region CreateCard() - カードを生成して指定された場所に配置
    public void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place); // カードをインスタンス化
        card.Init(cardID);

        bool isPlayer = (place == playerHand); // プレイヤーかどうかを判定
        card.model.isPlayerCard = isPlayer;
    }
    #endregion

    #region PlayerTurn() - プレイヤーのターンを開始
    public void PlayerTurn()
    {
        if (!isMyTurn) return; // 自分のターンでない場合は無視

        Debug.Log("プレイヤーのターン開始");
        DrawCard(playerHand, playerDeck); // カードを1枚引く
    }
    #endregion

    #region EndTurn() - ターンを終了
    public void EndTurn()
    {
        if (!isMyTurn || isGameOver) return;

        // 自分のフィールド上のカードを墓地に移動
        EndTurnForAllCards(playerField, playerGraveyard);

        // ターンを相手に渡す
        isMyTurn = false;
        photonView.RPC("SyncTurn", RpcTarget.Others, true);
    }
    #endregion

    #region EndTurnForAllCards() - フィールド上のカードを全て墓地に移動
    public void EndTurnForAllCards(Transform field, Transform graveyard)
    {
        CardController[] cardsOnField = field.GetComponentsInChildren<CardController>();

        foreach (CardController card in cardsOnField)
        {
            CreateCard(card.model.cardId, graveyard); // カードを墓地に移動
            Destroy(card.gameObject); // 元のカードを削除
        }
    }
    #endregion

    #region SyncTurn() - ターン情報を同期
    [PunRPC]
    private void SyncTurn(bool turn)
    {
        isMyTurn = turn; // ターンフラグを更新
        if (isMyTurn)
        {
            Debug.Log("自分のターン開始");
            PlayerTurn(); // プレイヤーのターンを開始
        }
        else
        {
            Debug.Log("相手のターン待機中...");
        }
    }
    #endregion

    #region DecreaseHP() - HPを減少させる
    public void DecreaseHP(bool isPlayer, int damage)
    {
        if (isPlayer)
        {
            playerHP -= damage;
        }
        else
        {
            enemyHP -= damage;
        }

        ShowLeaderHP();

        // HPがゼロ以下の場合、ゲーム終了
        if (playerHP <= 0 || enemyHP <= 0)
        {
            photonView.RPC("EndGame", RpcTarget.All, playerHP > 0);
        }
    }
    #endregion

    #region ShowLeaderHP() - プレイヤーと敵のHPをUIに表示する
    public void ShowLeaderHP()
    {
        if (playerHP <= 0) playerHP = 0;
        if (enemyHP <= 0) enemyHP = 0;

        playerHPText.text = playerHP.ToString();
        enemyHPText.text = enemyHP.ToString();
    }
    #endregion

    #region EndGame() - ゲーム終了処理
    [PunRPC]
    private void EndGame(bool isPlayerWinner)
    {
        isGameOver = true;

        if (isPlayerWinner)
        {
            Debug.Log("ゲーム終了: プレイヤーの勝利！");
        }
        else
        {
            Debug.Log("ゲーム終了: 相手の勝利！");
        }

        // 必要ならリスタートやシーン遷移処理を追加
    }
    #endregion
}
