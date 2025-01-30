using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private float time = 0.5f; // 回転にかかる時間（秒）
    private float fastTime = 0.15f; // 回転にかかる時間（秒）
    [SerializeField] private CardController cardPrefab;
    [SerializeField] private GameObject AnimationField;

    #region RotateCardAnimation-カードを回転
    public IEnumerator RotateCardAnimation(int cardId, Transform parent, Transform field)
    {
        float halfTime = time / 2f; // 縮むと広がるそれぞれの時間
        CardController card = Instantiate(cardPrefab, new Vector2(1000.0f, 350.0f), Quaternion.identity).GetComponent<CardController>();
        card.Init(cardId); // カードを初期化
        Vector2 originalScale = card.transform.localScale; // 元のスケールを保存
        Vector2 cardVector = new Vector2(2.0f, 2.0f);
        card.transform.SetParent(AnimationField.transform);

        // カードの横幅を徐々に短くしてゼロにする
        float timer = 0f;
        while (timer < halfTime)
        {
            float scaleX = Mathf.Lerp(cardVector.x, 0f, timer / halfTime);
            card.transform.localScale = new Vector2(scaleX, cardVector.y);
            timer += Time.deltaTime;
            yield return null;
        }

        // 完全にゼロにする
        card.transform.localScale = new Vector2(0f, cardVector.y);

        // 横幅を徐々に元の長さに戻す
        timer = 0f;
        while (timer < halfTime)
        {
            float scaleX = Mathf.Lerp(0f, cardVector.x, timer / halfTime);
            card.transform.localScale = new Vector2(scaleX, cardVector.y);
            timer += Time.deltaTime;
            yield return null;
        }

        // 0.5秒待つ
        yield return new WaitForSeconds(0.5f);

        // 元のスケールに戻し、親を変更
        card.transform.localScale = originalScale;
        card.transform.SetParent(field);
    }
    #endregion

    #region FastRotateCardAnimation-カードを回転（高速）
    public IEnumerator FastRotateCardAnimation(int cardId, Transform parent, Transform field)
    {
        float halfTime = fastTime / 2f; // 縮むと広がるそれぞれの時間
        CardController card = Instantiate(cardPrefab, new Vector2(1000.0f, 350.0f), Quaternion.identity).GetComponent<CardController>();
        card.Init(cardId); // カードを初期化
        Vector2 originalScale = card.transform.localScale; // 元のスケールを保存
        Vector2 cardVector = new Vector2(2.0f, 2.0f);
        card.transform.SetParent(AnimationField.transform);

        for (int i = 0; i < 3; i++)
        {
            halfTime = time / 2f;
            // カードの横幅を徐々に短くしてゼロにする
            float timer = 0f;
            while (timer < halfTime)
            {
                float scaleX = Mathf.Lerp(cardVector.x, 0f, timer / halfTime);
                card.transform.localScale = new Vector2(scaleX, cardVector.y);
                timer += Time.deltaTime;
                yield return null;
            }

            // 完全にゼロにする
            card.transform.localScale = new Vector2(0f, cardVector.y);

            // 横幅を徐々に元の長さに戻す
            timer = 0f;
            while (timer < halfTime)
            {
                float scaleX = Mathf.Lerp(0f, cardVector.x, timer / halfTime);
                card.transform.localScale = new Vector2(scaleX, cardVector.y);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // 0.5秒待つ
        yield return new WaitForSeconds(0.5f);

        // 元のスケールに戻し、親を変更
        card.transform.localScale = originalScale;
        card.transform.SetParent(field);
    }
    #endregion
}
