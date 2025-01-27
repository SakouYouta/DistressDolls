using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private float time = 0.75f; // 回転にかかる時間（秒）

    #region RotateCard-カードを回転
    public IEnumerator RotateCard(Transform card)
    {
        float halfTime = time / 2f; // 縮むと広がるそれぞれの時間
        Vector3 originalScale = card.localScale; // 元のスケールを保存

        // カードの横幅を徐々に短くしてゼロにする
        float elapsedTime = 0f;
        while (elapsedTime < halfTime)
        {
            float scaleX = Mathf.Lerp(originalScale.x, 0f, elapsedTime / halfTime);
            card.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全にゼロにする
        card.localScale = new Vector3(0f, originalScale.y, originalScale.z);

        // 横幅を徐々に元の長さに戻す
        elapsedTime = 0f;
        while (elapsedTime < halfTime)
        {
            float scaleX = Mathf.Lerp(0f, originalScale.x, elapsedTime / halfTime);
            card.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 元のスケールに戻す
        card.localScale = originalScale;
    }
    #endregion
}
