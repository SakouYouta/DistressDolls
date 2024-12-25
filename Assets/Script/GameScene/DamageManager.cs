using System.Collections;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    #region 変数定義
    public static DamageManager instance;
    private bool damageProcessActive = false; // ダメージプロセスが進行中かどうか
    private bool pendingDamageIsPlayer; // ダメージの対象がプレイヤーか敵か
    private int pendingDamage; // 現在のダメージ値
    private float responseTimer; // ガードカードを待つ時間
    private bool guardApplied; // ガードが適用されたかどうか
    private int damageReduction; // 現在のダメージ軽減値
    #endregion

    // Awake() - インスタンスの初期化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region StartDamageProcess() - ダメージカードが使われたらガードカードを待つタイマーをスタートさせる
    public void StartDamageProcess(bool isPlayerTarget, int damage, float waitTime = 5f)
    {
        if (damageProcessActive) return; // ダメージプロセスが進行中の場合は無視

        // 攻撃中のリーダーキャラクターを取得
        string attackingRole = isPlayerTarget ? "Enemy" : "Player";
        Character attackingLeader = DollSkillManager.instance.GetLeader(attackingRole);

        // 攻撃中のリーダーのスキルを適用
        if (attackingLeader != null)
        {
            Debug.Log($"攻撃中のリーダー: {attackingLeader.Name}");
            DollSkillManager.instance.ApplyLeaderSkill(attackingRole == "Player");
        }
        else
        {
            Debug.LogWarning("攻撃中のリーダーが設定されていません。");
        }

        // 次の攻撃ボーナスと軽減値を適用
        int bonusDamage = isPlayerTarget ? CardManager.instance.enemyNextAttackBonus : CardManager.instance.playerNextAttackBonus;
        int reduction = isPlayerTarget ? CardManager.instance.playerNextAttackBonusReduction : CardManager.instance.enemyNextAttackBonusReduction;
        int totalDamage = Mathf.Max(damage + bonusDamage - reduction, 0);

        // 初期化
        pendingDamageIsPlayer = isPlayerTarget;
        pendingDamage = totalDamage;
        responseTimer = waitTime;
        guardApplied = false;
        damageReduction = 0;
        damageProcessActive = true;

        Debug.Log($"ダメージプロセス開始: 対象は {(isPlayerTarget ? "プレイヤー" : "敵")}、ダメージ {totalDamage}、待機時間 {responseTimer}秒");

        // ボーナスと軽減値をリセット
        ResetNextAttackBonus(isPlayerTarget);
        ResetNextAttackReduction(isPlayerTarget);

        // 敵がガードカードをプレイする場合の処理
        if (!isPlayerTarget)
        {
            EnemyAiManager.instance.RespondToPlayerAttack();
        }

        StartCoroutine(DamageCountdown()); // タイマーの監視を開始
    }
    #endregion

    #region DamageCountdown() - ガード待機時間の監視
    private IEnumerator DamageCountdown()
    {
        while (responseTimer > 0)
        {
            responseTimer -= Time.deltaTime;
            yield return null;
        }

        ApplyDamage(); // タイマー終了後にダメージを適用
        damageProcessActive = false; // ダメージプロセス終了
    }
    #endregion

    #region ApplyDamage() - ダメージを適用
    private void ApplyDamage()
    {
        int finalDamage = Mathf.Max(pendingDamage - damageReduction, 0);

        if (pendingDamageIsPlayer)
        {
            GameManager.instance.playerHP -= finalDamage;
            Debug.Log($"プレイヤーに {finalDamage} ダメージが適用されました。残りHP: {GameManager.instance.playerHP}");
        }
        else
        {
            GameManager.instance.enemyHP -= finalDamage;
            Debug.Log($"敵に {finalDamage} ダメージが適用されました。残りHP: {GameManager.instance.enemyHP}");
        }
    }
    #endregion

    #region ResetNextAttackBonus() - 次の攻撃ボーナスをリセット
    private void ResetNextAttackBonus(bool isPlayerTarget)
    {
        if (isPlayerTarget)
        {
            CardManager.instance.enemyNextAttackBonus = 0;
        }
        else
        {
            CardManager.instance.playerNextAttackBonus = 0;
        }
    }
    #endregion

    #region ResetNextAttackReduction() - 次の攻撃軽減値をリセット
    private void ResetNextAttackReduction(bool isPlayerTarget)
    {
        if (isPlayerTarget)
        {
            CardManager.instance.playerNextAttackBonusReduction = 0;
        }
        else
        {
            CardManager.instance.enemyNextAttackBonusReduction = 0;
        }
    }
    #endregion

    #region UseProtectCard() - ガードカードを使用
    public void UseProtectCard(int reductionValue)
    {
        guardApplied = true;
        damageReduction += reductionValue;
        Debug.Log($"ガードカードが使用され、ダメージ軽減値が {reductionValue} 増加しました。");
    }
    #endregion
}
