using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private float time = 0.75f; // 回転にかかる時間（秒）
    [SerializeField] private CardController cardPrefab;
    [SerializeField] private GameObject AnimationField;

    #region RotateCard-カードを回転
    public IEnumerator RotateCard(int cardId, Transform parent, Transform field)
    {
        float halfTime = time / 2f; // 縮むと広がるそれぞれの時間
        CardController card = Instantiate(cardPrefab, new Vector2(1000.0f, 350.0f), Quaternion.identity).GetComponent<CardController>();
        card.Init(cardId); // カードを初期化
        Vector2 originalScale = card.transform.localScale; // 元のスケールを保存
        Vector2 cardVector = new Vector2(2.0f, 2.0f);
        card.transform.SetParent(AnimationField.transform);

        // カードの横幅を徐々に短くしてゼロにする
        float elapsedTime = 0f;
        while (elapsedTime < halfTime)
        {
            float scaleX = Mathf.Lerp(cardVector.x, 0f, elapsedTime / halfTime);
            card.transform.localScale = new Vector2(scaleX, cardVector.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全にゼロにする
        card.transform.localScale = new Vector2(0f, cardVector.y);

        // 横幅を徐々に元の長さに戻す
        elapsedTime = 0f;
        while (elapsedTime < halfTime)
        {
            float scaleX = Mathf.Lerp(0f, cardVector.x, elapsedTime / halfTime);
            card.transform.localScale = new Vector2(scaleX, cardVector.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 元のスケールに戻し、親を変更
        card.transform.localScale = originalScale;
        card.transform.SetParent(field);
    }
    #endregion
}
