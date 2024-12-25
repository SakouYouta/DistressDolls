using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;

    private int pendingDamage = 0; // ダメージ値
    private int damageReduction = 0; // 保護カードによる軽減値
    private bool pendingDamageIsPlayer; // ダメージ対象がプレイヤーかエネミーか
    private float responseTimer = 0f; // ガードカード応答タイマー
    private readonly float guardResponseTime = 3f; // ガードカード応答時間
    private bool guardApplied = false; // ガードが適用されたかどうかのフラグ
    private bool damageProcessActive = false; // ダメージプロセスが進行中かどうか

    void Awake()
    {
        instance = this;
    }

    #region StartDamageProcess() - ダメージカードが使われたらガードカードを待つタイマーをスタートさせる
    public void StartDamageProcess(bool isPlayerTarget, int damage, float waitTime = 5f)
    {
        // すでにダメージプロセスが進行中の場合は無視
        if (damageProcessActive) return;

        // 次の攻撃ボーナスを適用
        int bonusDamage = isPlayerTarget ? CardManager.instance.enemyNextAttackBonus : CardManager.instance.playerNextAttackBonus;
        int totalDamage = damage + bonusDamage;

        // 次の攻撃軽減を適用
        int reduction = isPlayerTarget ? CardManager.instance.playerNextAttackBonusReduction : CardManager.instance.enemyNextAttackBonusReduction;
        totalDamage = Mathf.Max(totalDamage - reduction, 0); // ダメージが0未満にならないよう調整

        // 初期化
        pendingDamageIsPlayer = isPlayerTarget;
        pendingDamage = totalDamage;
        responseTimer = waitTime; // 引数で渡された待機時間を設定
        guardApplied = false; // ガード未適用に設定
        damageReduction = 0; // 軽減値もリセット
        damageProcessActive = true; // ダメージプロセスをアクティブに設定

        Debug.Log($"ダメージプロセス開始: 対象は {(isPlayerTarget ? "プレイヤー" : "敵")}、ダメージ {totalDamage}、待機時間 {responseTimer}秒");

        // 次の攻撃ボーナスと軽減値をリセット
        ResetNextAttackBonus(isPlayerTarget);
        ResetNextAttackReduction(isPlayerTarget);

        // 敵がガードカードをプレイする処理を追加
        if (!isPlayerTarget) // 攻撃対象が敵の場合
        {
            EnemyAiManager.instance.RespondToPlayerAttack();
        }

        // タイマーの監視を開始
        StartCoroutine(DamageCountdown());
    }
    #endregion

    #region UseProtectCard() - 相手がガードカードを使うかの処理
    public void UseProtectCard(int protectValue)
    {
        // ダメージプロセスが進行中かつ応答時間内の場合のみ適用可能
        if (damageProcessActive && responseTimer > 0)
        {
            damageReduction = protectValue; // ガード効果を設定
            guardApplied = true; // ガード適用をマーク
            Debug.Log($"ガードカード適用: {protectValue} ポイント軽減");
        }
        else
        {
            Debug.LogWarning("ガードカードの応答時間を超過しました。軽減は適用されません。");
        }
    }
    #endregion

    #region DamageCountdown() - ガードカードが待機時間が終了したのちダメージ処理に移動
    private IEnumerator DamageCountdown()
    {
        while (responseTimer > 0)
        {
            responseTimer -= Time.deltaTime; // タイマーを減少
            yield return null;
        }

        // タイマー終了後にダメージ適用
        ApplyDamage();
        damageProcessActive = false; // ダメージプロセス終了
    }
    #endregion

    #region ApplyDamage() - ダメージの適用
    private void ApplyDamage()
    {
        int finalDamage = Mathf.Max(pendingDamage - damageReduction, 0); // 最終ダメージ計算

        // ガード適用状況をログ出力
        if (guardApplied)
        {
            Debug.Log("ガードが成功し、ダメージが軽減されました。");
        }
        else
        {
            Debug.Log("ガードが適用されず、ダメージがそのまま適用されます。");
        }

        Debug.Log($"最終ダメージ計算: 元のダメージ {pendingDamage}, 軽減値 {damageReduction}, 最終ダメージ {finalDamage}");

        if (pendingDamageIsPlayer)
        {
            GameManager.instance.DecreaseHP(true, finalDamage);
        }
        else
        {
            GameManager.instance.DecreaseHP(false, finalDamage);
        }

        // HPの表示を更新
        GameManager.instance.ShowLeaderHP();
    }
    #endregion

    #region ResetNextAttackBonus() - 次の攻撃ボーナスをリセット
    private void ResetNextAttackBonus(bool isPlayerTarget)
    {
        if (isPlayerTarget)
        {
            CardManager.instance.enemyNextAttackBonus = 0;
            Debug.Log("敵の次の攻撃ボーナスがリセットされました");
        }
        else
        {
            CardManager.instance.playerNextAttackBonus = 0;
            Debug.Log("プレイヤーの次の攻撃ボーナスがリセットされました");
        }
    }
    #endregion

    #region ResetNextAttackReduction() - 次の攻撃軽減をリセット
    private void ResetNextAttackReduction(bool isPlayerTarget)
    {
        if (isPlayerTarget)
        {
            CardManager.instance.playerNextAttackBonusReduction = 0;
            Debug.Log("プレイヤーの次の攻撃軽減がリセットされました");
        }
        else
        {
            CardManager.instance.enemyNextAttackBonusReduction = 0;
            Debug.Log("敵の次の攻撃軽減がリセットされました");
        }
    }
    #endregion
}

