using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] public Transform playerHand, enemyHand, playerField, enemyField, playerGraveyard, enemyGraveyard;
    [SerializeField] Text playerHPText, enemyHPText;
    public int playerHP, enemyHP;

    //ターン管理フラグ
    public bool isPlayerTurn = true;
    // Damageカード使用フラグ
    private bool canUseDamageCard = true;

    //デッキリスト
    List<int> deck = new List<int>() {  43, 43, 43,
                                        44, 44, 44,
                                        45, 45, 45,
                                        46, 46, 46,
                                        47, 47, 47,
                                        48, 48, 48,
                                        49, 49, 49,
                                        50, 50, 50,
                                        51, 51, 51,
                                        52, 52, 52,
                                        53, 53, 53,
                                        54, 54, 54};  //proto

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

        //デッキをシャッフル
        Shuffle();

        // 初期手札を配る
        SetStartHand();

        // ターンの決定
        TurnCalc();
    }

    //シャッフル
    void Shuffle()
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

    public void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);

        // カードがどちらの所有者かを設定
        bool isPlayer = (place == playerHand); // プレイヤーの手札に追加する場合はtrue
        card.model.isPlayerCard = isPlayer;
    }


    public void DrawCard(Transform hand, int drawAmount = 1) // カードを引く（引く枚数を指定可能）
    {
        // デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }

        // 手札のカード数が9未満の場合にカードを引く
        CardController[] handCardList = hand.GetComponentsInChildren<CardController>();
        if (handCardList.Length < 9)
        {
            // 引く枚数分カードを引く
            for (int i = 0; i < drawAmount; i++)
            {
                if (deck.Count == 0) break; // デッキが空ならそれ以上引かない

                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = deck[0];
                deck.RemoveAt(0);
                CreateCard(cardID, hand);
            }
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
        // 現在のターンのカードを墓地に移動
        if (isPlayerTurn)
        {
            EndTurnForAllCards(playerField, playerGraveyard);  // プレイヤーのフィールドからカードを墓地に移動
            EndTurnForAllCards(enemyField, enemyGraveyard);  // エネミーのフィールドからカードを墓地に移動
        }
        else
        {
            EndTurnForAllCards(playerField, playerGraveyard);  // プレイヤーのフィールドからカードを墓地に移動
            EndTurnForAllCards(enemyField, enemyGraveyard);  // エネミーのフィールドからカードを墓地に移動
        }

        // ターンを逆にする
        isPlayerTurn = !isPlayerTurn;

        // ターンの処理を行う
        TurnCalc();
    }

    // プレイヤーまたはエネミーのフィールド上の全カードを墓地に移動させるメソッド
    private void EndTurnForAllCards(Transform field, Transform graveyard)
    {
        // フィールド上の全てのカードを取得
        CardController[] cardsOnField = field.GetComponentsInChildren<CardController>();

        foreach (CardController card in cardsOnField)
        {
            // 各カードを墓地に移動させ、再生成する
            CreateCard(card.model.cardId, graveyard);
            Destroy(card.gameObject);  // 元のカードを削除
        }
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        // ターン開始時にDamageカード使用可フラグをリセット
        canUseDamageCard = true;

        DrawCard(playerHand); // 手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        // ターン開始時にDamageカード使用可フラグをリセット
        canUseDamageCard = true;

        // 手札を補充
        DrawCard(enemyHand);

        // 敵手札からカードを1枚フィールドにプレイ
        //CardController[] enemyHandCards = enemyHand.GetComponentsInChildren<CardController>();

        //if (enemyHandCards.Length > 0 && enemyField.childCount < 5)
        //    // 最初のカードをフィールドに移動
        //    CardController cardToPlay = enemyHandCards[0];
        //    cardToPlay.transform.SetParent(enemyField);
        //    // カード効果を適用
        //    ApplyCardEffect(cardToPlay.model, false); // 敵のターンなのでfalseを渡す
        //    Debug.Log($"敵がカード {cardToPlay.model.name} をプレイしました");
        //}

    }

    /*void ApplyCardEffect(CardModel card, bool isPlayerTurn)
    {
        
        // 敵ターンの場合
        switch (card.effectType)
        {
            case CardEffectType.Damage:
                DecreaseHP(false, 2); // ダメージ量は仮
                break;

           case CardEffectType.Protect:
                Debug.Log("エネミー: プロテクト効果: 次のダメージを軽減します");
                // 防御のロジックを追加
                break;

            case CardEffectType.DrawCard:
                DrawCard(enemyHand);
                break;

            default:
               Debug.Log("エネミー: 未知の効果です");
                break;
        }
    }
    */

    public void DecreaseHP(bool isPlayer, int damage)
    {
        // プレイヤーまたはエネミーの手札を取得
        Transform hand = isPlayer ? playerHand : enemyHand;

        // Protectカードによるダメージ軽減
        int damageReduction = ApplyProtectCard(hand);

        // 最終的なダメージを計算（軽減後）
        int finalDamage = Mathf.Max(0, damage - damageReduction); // ダメージが0未満にならないようにする

        // スイッチ文内でHPの減少を処理
        switch (isPlayer)
        {
            case true:
                playerHP -= finalDamage;
                Debug.Log($"プレイヤーのHPが {finalDamage} 減少しました: 残りHP {playerHP}");
                if (playerHP <= 0)
                {
                    EndGame(false);
                }
                break;

            case false:
                enemyHP -= finalDamage;
                Debug.Log($"エネミーのHPが {finalDamage} 減少しました: 残りHP {enemyHP}");
                if (enemyHP <= 0)
                {
                    EndGame(true);
                }
                break;
        }

        // HPのUIを更新
        ShowLeaderHP();
    }

    // Protectカードを適用するメソッド
    private int ApplyProtectCard(Transform hand)
    {
        int damageReduction = 0;
        bool protectCardUsed = false; // Protectカードが使用されたかを追跡

        // 手札にあるカードを確認
        CardController[] handCards = hand.GetComponentsInChildren<CardController>();


        foreach (CardController card in handCards)
        {
            // 最初のProtectカードが見つかればその効果を適用
            if (card.model.effectType == CardEffectType.Protect && !protectCardUsed)
            {

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    damageReduction += card.model.effectValue; // Protectカードの効果値を軽減に使用
                    protectCardUsed = true; // Protectカードが使用されたとマーク
                    Destroy(card.gameObject); // Protectカードを消費
                }
                else if(Input.GetKeyDown(KeyCode.N))
                {
                    // ガードカードを使用しない
                    Debug.Log("Protectカードは使用されませんでした");
                    protectCardUsed = true; // 使わないときもループを終了
                }
            }
        }

        return damageReduction; // 軽減されたダメージ値を返す
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