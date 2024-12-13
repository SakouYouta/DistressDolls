using UnityEngine;

public class EnemyAiManager : MonoBehaviour
{
    public static EnemyAiManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // AIがカードを使用する処理
    public void PerformAiActions()
    {
        // AIがランダムでカードを選ぶ例
        if (GameManager.instance.enemyHand.childCount > 0)
        {
            int randomIndex = Random.Range(0, GameManager.instance.enemyHand.childCount);
            Transform cardToPlay = GameManager.instance.enemyHand.GetChild(randomIndex);

            CardController cardController = cardToPlay.GetComponent<CardController>();
            CardModel cardModel = cardController.model;

            // カードの効果を適用
            CardManager.instance.ApplyCardEffect(cardModel, false);

            // 使用したカードを場に出す
            PlayCardOnField(cardToPlay);
        }
        else
        {
            Debug.LogWarning("敵の手札にカードがありません");
        }
    }

    // カードを場に出す処理
    private void PlayCardOnField(Transform cardToPlay)
    {
        // カードを手札から場に移動
        cardToPlay.SetParent(GameManager.instance.enemyField); // enemyFieldは場のTransform

        // 場にカードが出たことを確認
        Debug.Log($"敵のカード「{cardToPlay.name}」が場に出されました");
    }
}
