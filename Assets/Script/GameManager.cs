using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] public Transform playerHand,enemyHand, playerField, enemyField, playerGraveyard, enemyGraveyard;
    [SerializeField] Text playerHPText, enemyHPText;

    [SerializeField] private PopupManager popupManager;

    public int playerHP, enemyHP;
    public bool isPlayerTurn = true; 

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
            int k = UnityEngine.Random.Range(0, n + 1); // UnityEngine.Random を明示
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }


    void CreateCard(int cardID, Transform place)
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
        // 手札を補充
        DrawCard(enemyHand);
    }

    void ApplyCardEffect(CardModel card, bool isPlayerTurn)
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

    public void DecreaseHP(bool isPlayer, int damage)
    {
        // Protectカードが手札にあるか確認
        List<CardController> protectCards = GetProtectCards(isPlayer);
        if (protectCards.Count > 0)
        {
            // PopupManagerを使って選択処理を依頼
            popupManager.ShowPopup(protectCards, (selectedCard) =>
            {
                // ユーザーがProtectカードを選んだ場合
                int damageReduction = 0;
                if (selectedCard != null)
                {
                    damageReduction = selectedCard.model.effectValue;
                    Destroy(selectedCard.gameObject); // 使用したカードを破壊
                    Debug.Log($"カード「{selectedCard.model.name}」を使用してダメージを軽減しました！");
                }

                // 軽減後のダメージを計算
                int finalDamage = Mathf.Max(0, damage - damageReduction);
                ApplyDamage(isPlayer, finalDamage); // ダメージを適用
            });
        }
        else
        {
            // Protectカードがない場合、そのままダメージ適用
            ApplyDamage(isPlayer, damage);
        }
    }

    private List<CardController> GetProtectCards(bool isPlayer)
    {
        Transform hand = isPlayer ? playerHand : enemyHand;
        List<CardController> protectCards = new List<CardController>();
        foreach (CardController card in hand.GetComponentsInChildren<CardController>())
        {
            if (card.model.effectType == CardEffectType.Protect)
            {
                protectCards.Add(card);
            }
        }
        return protectCards;
    }

    private void ApplyDamage(bool isPlayer, int finalDamage)
    {
        if (isPlayer)
        {
            playerHP -= finalDamage;
            Debug.Log($"プレイヤーのHPが {finalDamage} 減少しました: 残りHP {playerHP}");
            if (playerHP <= 0) EndGame(false);
        }
        else
        {
            enemyHP -= finalDamage;
            Debug.Log($"エネミーのHPが {finalDamage} 減少しました: 残りHP {enemyHP}");
            if (enemyHP <= 0) EndGame(true);
        }
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