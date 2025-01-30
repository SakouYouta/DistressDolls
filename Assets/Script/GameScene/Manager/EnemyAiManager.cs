using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class EnemyAiManager : MonoBehaviour
{
    [SerializeField] private CardAnimation cardAnimation;
    [SerializeField] private GameObject AnimationField;

    public static EnemyAiManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region PerformAiActions() - 敵AIが動的な優先順位に基づいてカードを使用する処理
    public void PerformAiActions()
    {
        Transform cardToPlay = null;
        bool onlyProtectCardFound = true; // Protectカードしかない場合を記録するフラグ

        // 1. 手札が空でないか確認
        if (GameManager.instance.enemyHand.childCount > 0)
        {
            // 優先度リストを定義（動的に変更可能）
            CardEffectType[] priorityList = {
            CardEffectType.DrawCard,    // 最優先: ドローカード
            CardEffectType.Researcher,  // 2: 研究者固有カード
            CardEffectType.Angel,       // 3: 天使固有カード
            CardEffectType.Witch,       // 4: 魔女の固有カード
            CardEffectType.Damage,      // ラスト: アタックカード
            CardEffectType.Protect,     // Protectカード（最低優先度）
        };

            // 優先度リストに基づいてカードを探す
            foreach (CardEffectType effectType in priorityList)
            {
                cardToPlay = FindCardByType(effectType);
                if (cardToPlay != null)
                {
                    // Protect以外のカードが見つかった場合、フラグを変更
                    if (effectType != CardEffectType.Protect)
                    {
                        onlyProtectCardFound = false;
                    }

                    Debug.Log($"優先度 {effectType} のカードが選ばれました");
                    break; // 見つかった時点でループを終了
                }
            }

            // 全ての優先カードが見つからなかった場合は先頭のカードを使用
            if (cardToPlay == null)
            {
                cardToPlay = GameManager.instance.enemyHand.GetChild(0); // デフォルトのカード
                onlyProtectCardFound = false; // 手札の最初のカードがProtectでない可能性もあるため
            }

            // 最適なカードをプレイ
            if (cardToPlay != null)
            {
                CardController cardController = cardToPlay.GetComponent<CardController>();
                CardModel cardModel = cardController.model;

                // Protectカードのみの場合はターンを強制終了
                if (onlyProtectCardFound && cardModel.effectType == CardEffectType.Protect)
                {
                    Debug.Log("Protectカードしかないため、ターンを終了します");
                    GameManager.instance.TurnEnd = true; // ターンを終了
                    return; // メソッドを終了
                }

                // 使用したカードを場に出す
                // 敵手札の場合はカード裏面を表示
                cardController.view.InvisibleHand(false); // カードの裏面を表示
                PlayCardOnField(cardToPlay);
                Debug.Log($"敵がカード「{cardModel.name}」を使用しました（効果タイプ: {cardModel.effectType}）");

                // カードの効果を適用
                CardManager.instance.ApplyCardEffect(cardModel, false);
            }
        }
        else
        {
            Debug.LogWarning("敵の手札にカードがありません");
        }
    }
    #endregion

    #region FindCardByType() - 特定の効果タイプのカードを手札から探す
    private Transform FindCardByType(CardEffectType effectType)
    {
        for (int i = 0; i < GameManager.instance.enemyHand.childCount; i++)
        {
            Transform card = GameManager.instance.enemyHand.GetChild(i);
            CardController cardController = card.GetComponent<CardController>();
            CardModel cardModel = cardController.model;

            if (cardModel.effectType == effectType)
            {
                return card; // 指定した効果タイプのカードを見つけたら返す
            }
        }
        return null; // 該当するカードがない場合
    }
    #endregion

    #region PlayCardOnField() - 使用したカードを場に出す処理
    private async void PlayCardOnField(Transform cardToPlay)
    {
        CardController cardController = cardToPlay.GetComponent<CardController>();
        CardModel cardModel = cardController.model;

        await Task.Delay(1000); // 1秒待つ

        // カードを手札から場に移動
        cardToPlay.SetParent(GameManager.instance.enemyField);
        //StartCoroutine(Animation(cardToPlay, cardModel));

        // 場にカードが出たことを確認
        Debug.Log($"敵のカード「{cardToPlay.name}」が場に出されました");
    }
    #endregion

    #region RespondToPlayerAttack() - プレイヤーの攻撃に応じてガードカードを使用する処理
    public void RespondToPlayerAttack()
    {
        // ガードカードを探索
        Transform guardCard = FindGuardCardInHand();

        if (guardCard != null)
        {
            CardController cardController = guardCard.GetComponent<CardController>();
            CardModel cardModel = cardController.model;

            // ガードカードの効果を適用
            DamageManager.instance.UseProtectCard(cardModel.effectValue);
            Debug.Log($"敵がガードカード「{guardCard.name}」を使用: 軽減値 {cardModel.effectValue}");

            // 敵リーダーが「無垢な歌姫 ドロシー」の場合、固有スキルを発動
            if (DollSkillManager.instance.enemyLeaderCharacter.Name == "無垢な歌姫 ドロシー")
            {
                DollSkillManager.instance.ApplyPureSongstressEffect(DollSkillManager.instance.enemyLeaderCharacter);
                Debug.Log("敵リーダー「ドロシー」の固有スキルが発動しました！");
            }

            // 使用したカードを場に出す
            cardController.view.InvisibleHand(false);
            PlayCardOnField(guardCard);
        }
        else
        {
            Debug.Log("敵の手札にガードカードがありません");
        }
    }
    #endregion

    #region FindGuardCardInHand() - 手札からガードカードを見つける
    private Transform FindGuardCardInHand()
    {
        for (int i = 0; i < GameManager.instance.enemyHand.childCount; i++)
        {
            Transform card = GameManager.instance.enemyHand.GetChild(i);
            CardController cardController = card.GetComponent<CardController>();
            CardModel cardModel = cardController.model;

            if (cardModel.effectType == CardEffectType.Protect)
            {
                return card; // ガードカードを見つけたら返す
            }
        }
        return null; // ガードカードがない場合
    }
    #endregion

    #region Animation()-アニメーション用のコルーチン
    private IEnumerator Animation(Transform cardToPlay, CardModel cardModel)
    {
        if (cardToPlay != null)
            yield return StartCoroutine(cardAnimation.RotateCardAnimation(cardModel.cardId, GameManager.instance.enemyField, AnimationField));
        else
            yield break;

        if (cardToPlay != null)
            Destroy(cardToPlay.gameObject);
    }
    #endregion
}
