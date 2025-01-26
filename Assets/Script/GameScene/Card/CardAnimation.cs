using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private float rotationDuration = 0.75f; // 回転にかかる時間（秒）

    #region RotateCard-カードを回転
    public IEnumerator RotateCard(Transform card)
    {
        float elapsedTime = 0.0f; // 経過時間を記録する変数
        float startRotation = card.eulerAngles.z;
        float endRotation = startRotation + 360f; // 一周（360度）後の回転角度

        while (elapsedTime < rotationDuration)
        {
            // 現在の回転角度を線形補間で計算
            float currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
            card.eulerAngles = new Vector3(0, 0, currentRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 回転が終了した後、最終的に初期の向きにリセット（0度の状態に戻す）
        card.eulerAngles = new Vector3(0, 0, startRotation);
    }
    #endregion
}
