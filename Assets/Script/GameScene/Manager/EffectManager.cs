using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject damageEffect; // ダメージの際表示するオブジェクト
    private float damageTime = 0.5f;                        // ダメージ演出を表示する時間
    private float distance = 150.0f;                            // 移動させる距離

    #region ShowDamage()-ダメージ用の演出
    public IEnumerator ShowDamage(bool bPlayer, int damage, Transform Life)
    {
        if (damageEffect == null || damage <= 0)// damegaEffectがnullなら処理を飛ばす
            yield break;

        damageEffect.SetActive(true);
        if(damageEffect)
        if (damageEffect.transform.childCount > 0)// 子オブジェクトがあるのか確認
        {
            GameObject child = damageEffect.transform.GetChild(0).gameObject;
            Text text = child.GetComponent<Text>();
            if (text != null)
                text.text = damage.ToString();
        }

        damageEffect.transform.position = Life.position;

        float timer = 0;
        if (bPlayer)// プレイヤーかどうか判定
        {
            while (timer < damageTime)
            {
                // 時間経過に応じてY軸の位置を増加
                damageEffect.transform.position += new Vector3(0, (distance / damageTime) * Time.deltaTime, 0);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (timer < damageTime)
            {
                // 時間経過に応じてY軸の位置を増加
                damageEffect.transform.position -= new Vector3(0, (distance / damageTime) * Time.deltaTime, 0);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        damageEffect.SetActive(false);
    }
    #endregion
}
