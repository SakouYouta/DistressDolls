using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    //フィールドの宣言
    [SerializeField] CardController cardPrefab;
    public Transform playerHand, enemyHand, playerField, enemyField, playerGraveyard, enemyGraveyard;
    [SerializeField] Text playerHPText, enemyHPText;
    public int playerHP, enemyHP;
    public bool isPlayerTurn = true;
    public bool TurnEnd = false;
    public List<int> playerDeck, enemyDeck;
    public static GameManager instance;

    // Awake() - インスタンスの初期化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region Start() - ゲーム開始時の初期設定を行う
    void Start()
    {
        StartGame();
    }
    #endregion

    #region StartGame() - ゲーム開始時の初期設定
    void StartGame()
    {

        // リーダーキャラクターを設定（DollSkillManagerにアクセスしてリーダーを設定）

        //神秘への探索者 エレミネ  HPが６以下になったら攻撃力が5増加
        //無垢な歌姫 ドロシー      ガードした時５０％の確率でカード引く
        //幼魔女 アイネ            攻撃した時ガードされなかったら3ダメージ

        Character playerLeader = new Character("神秘への探索者 エレミネ", true);  // プレイヤーリーダー
        Character enemyLeader = new Character("幼魔女 アイネ", false);  // 敵リーダー

        // DollSkillManager のインスタンスを取得して SetLeaders を呼び出す
        DollSkillManager.instance.SetLeaders(playerLeader, enemyLeader); // ここで SetLeaders を呼び出しているか確認

        // デッキの初期化
        playerDeck = DataSaveManager.LoadDeckList();
        enemyDeck = new List<int>() { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };

        // プレイヤーと敵のHP初期値
        playerHP = 10;
        enemyHP = 20;

        // デッキシャッフル
        Shuffle(playerDeck);
        Shuffle(enemyDeck);

        // 初期手札を配布
        SetStartHand();

        // ターン計算を開始
        TurnCalc();
    }
    #endregion

    #region Shuffle() - デッキをシャッフルする
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

    #region  CreateCard() - カードを生成して指定された場所に配置する
    public void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place); // カードをインスタンス化
        card.Init(cardID);

        bool isPlayer = (place == playerHand); // プレイヤーかどうかを判定
        card.model.isPlayerCard = isPlayer;
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
                CreateCard(cardID, hand); // 新しいカードを生成して手札に追加
            }
        }
    }
    #endregion

    #region SetStartHand() - 初期手札を3枚配布する
    void SetStartHand()
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand, playerDeck);
            DrawCard(enemyHand, enemyDeck);
        }
    }
    #endregion

    #region  TurnCalc() - プレイヤーまたは敵のターンを計算し、それぞれのターンを実行する
    void TurnCalc()
    {
        if (isPlayerTurn)
        {
            PlayerTurn(); // プレイヤーのターンを開始
        }
        else
        {
            EnemyTurn(); // 敵のターンを開始
        }
    }
    #endregion

    #region ChangeTurn() - ターンを終了し、次のターンに切り替える
    public void ChangeTurn()
    {
        EndTurnForAllCards(playerField, playerGraveyard);  // プレイヤーのフィールドからカードを墓地に移動
        EndTurnForAllCards(enemyField, enemyGraveyard);  // エネミーのフィールドからカードを墓地に移動

        // ターンを逆にする
        isPlayerTurn = !isPlayerTurn;

        //ターンエンドのフラグを初期化
        TurnEnd = false;

        // 次のターンの処理を実行
        TurnCalc();
    }
    #endregion

    #region EndTurnForAllCards() - フィールド上のカードを全て墓地に移動させる
    public void EndTurnForAllCards(Transform field, Transform graveyard)
    {
        CardController[] cardsOnField = field.GetComponentsInChildren<CardController>();

        foreach (CardController card in cardsOnField)
        {
            CreateCard(card.model.cardId, graveyard); // カードを墓地に移動
            Destroy(card.gameObject);  // 元のカードを削除
        }
    }
    #endregion

    #region  PlayerTurn() - プレイヤーのターンを開始する
    private void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        DrawCard(playerHand, playerDeck); // 手札を1枚加える

        if (TurnEnd)
        {
            ChangeTurn();
        }
    }
    #endregion

    #region EnemyTurn() - 敵のターンを開始する
    private async void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        // 1. 敵がカードを引く
        DrawCard(enemyHand, enemyDeck);
        await Task.Delay(1000); // 1秒待つ

        // 2. 敵がカードを使用する
        while (TurnEnd == false)
        {
            EnemyAiManager.instance.PerformAiActions();
            await Task.Delay(1500); 
        }

        // 3. 敵がターンを終了する条件
        await Task.Delay(5000); // 待機後ターン終了
        ChangeTurn();
    }
    #endregion

    #region DecreaseHP() - ダメージを受けた場合のHPを減らす処理
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

    #region  EndGame() - ゲーム終了処理
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
    #endregion
}
