using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DamageManager.cs
public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;

    private int pendingDamage = 0; // ダメージ値
    private int damageReduction = 0; // 保護カードによる軽減値
    private bool pendingDamageIsPlayer; // ダメージ対象がプレイヤーかエネミーか

    void Awake()
    {
        instance = this;
    }

    // ダメージ処理の開始
    public void StartDamageProcess(bool isPlayerTarget, int damage)
    {
        pendingDamageIsPlayer = isPlayerTarget;
        pendingDamage = damage;

        // ダメージの軽減を適用（プロテクトカードがある場合）
        ApplyDamage();
    }

    // ダメージの適用
    private void ApplyDamage()
    {
        // ダメージ軽減があれば適用
        int finalDamage = Mathf.Max(pendingDamage - damageReduction, 0); // ダメージが負にならないように調整

        // デバッグ: 軽減値が適用されているかを確認
        Debug.Log($"適用前のダメージ: {pendingDamage}, 軽減値: {damageReduction}, 最終ダメージ: {finalDamage}");

        // ダメージ処理
        if (pendingDamageIsPlayer)
        {
            // プレイヤーのHPを減少
            GameManager.instance.DecreaseHP(true, finalDamage);
        }
        else
        {
            // 敵のHPを減少
            GameManager.instance.DecreaseHP(false, finalDamage);
        }

        // HPの表示を更新
        GameManager.instance.ShowLeaderHP();

        // 軽減値をリセット（次のターンで再度設定されるまで無効化）
        damageReduction = 0;
    }

    // 保護カードの適用（プロテクト効果を設定）
    public void UseProtectCard(int protectValue)
    {
        // 保護カードによる軽減
        damageReduction = protectValue;
        Debug.Log($"保護カードが適用され、ダメージが {protectValue} ポイント軽減されます。");
    }
}

