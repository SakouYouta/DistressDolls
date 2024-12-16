using UnityEngine;
using System.Threading.Tasks;

public class EnemyAiManager : MonoBehaviour
{
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

        // 1. 手札が空でないか確認
        if (GameManager.instance.enemyHand.childCount > 0)
        {
            // 優先度リストを定義（動的に変更可能）
            CardEffectType[] priorityList = {
            CardEffectType.DrawCard,    //最優先:ドローカード
            CardEffectType.Researcher,  //2:研究者固有カード
            CardEffectType.Angel,       //3:天使固有カード
            CardEffectType.Witch,       //4:魔女の固有カード
            CardEffectType.Damage,      //ラスト: アタックカード
            CardEffectType.Protect,
        };

            // 優先度リストに基づいてカードを探す
            for (int i = 0; i < priorityList.Length; i++)
            {
                cardToPlay = FindCardByType(priorityList[i]);
                if (cardToPlay != null)
                {
                    Debug.Log($"優先度 {priorityList[i]} のカードが選ばれました");
                    break; // 見つかった時点でループを終了

                }
            }

            // 4. 全ての優先カードが見つからなかった場合は先頭のカードを使用
            if (cardToPlay == null)
            {
                cardToPlay = GameManager.instance.enemyHand.GetChild(0); // デフォルトのカード
            }

            // 最適なカードをプレイ
            if (cardToPlay != null)
            {
                CardController cardController = cardToPlay.GetComponent<CardController>();
                CardModel cardModel = cardController.model;

                // 使用したカードを場に出す
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
    private void PlayCardOnField(Transform cardToPlay)
    {
        // カードを手札から場に移動
        cardToPlay.SetParent(GameManager.instance.enemyField); // enemyFieldは場のTransform

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

            // 使用したカードを場に出す
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
}
