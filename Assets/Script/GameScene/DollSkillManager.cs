using System.Collections.Generic;
using UnityEngine;

public class DollSkillManager : MonoBehaviour
{
    #region 変数定義
    // リーダーキャラクター
    public Character playerLeaderCharacter; // プレイヤーのリーダー
    public Character enemyLeaderCharacter;  // 敵のリーダー
    public static DollSkillManager instance;
    #endregion

    // Awake() - インスタンスの初期化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #region SetLeaders() - 初期化処理
    public void SetLeaders(Character playerLeader, Character enemyLeader)
    {
        playerLeaderCharacter = playerLeader; // プレイヤーのリーダーを設定
        enemyLeaderCharacter = enemyLeader;  // 敵のリーダーを設定

        Debug.Log($"プレイヤーリーダーが設定されました: {playerLeaderCharacter.Name}");
        Debug.Log($"敵リーダーが設定されました: {enemyLeaderCharacter.Name}");
    }
    #endregion

    #region GetLeader() - リーダーを動的に取得
    // "Player" または "Enemy" に応じてリーダーを取得
    public Character GetLeader(string role)
    {
        if (role == "Player")
        {
            return playerLeaderCharacter;
        }
        else if (role == "Enemy")
        {
            return enemyLeaderCharacter;
        }
        else
        {
            Debug.LogWarning("不正なリーダー取得リクエストです。roleは 'Player' または 'Enemy' でなければなりません。");
            return null;
        }
    }
    #endregion

    #region ApplyLeaderSkill() - リーダーに応じたスキルの適用
    // スキルを適用する対象がプレイヤーか敵かで分岐
    public void ApplyLeaderSkill(bool isPlayer)
    {
        Character targetLeader = isPlayer ? playerLeaderCharacter : enemyLeaderCharacter;

        if (targetLeader == null)
        {
            Debug.LogError("リーダーキャラクターが設定されていません！");
            return;
        }

        switch (targetLeader.Name)
        {
            case "神秘への探究者 エレミネ":
                ApplySeekerOfMysteryEffect(targetLeader);
                break;

            case "無垢な歌姫 ドロシー・レイン":
                ApplyPureSongstressEffect(targetLeader);
                break;

            case "幼魔女 アイネ・ヴァンデンベルグ":
                ApplyYoungWitchEffect(targetLeader);
                break;

            case "守護の壁":
                ApplyProtectEffect(targetLeader);
                break;

            default:
                Debug.LogWarning($"リーダー『{targetLeader.Name}』に対応するスキルが見つかりません。");
                break;
        }
    }
    #endregion

    #region　ApplySeekerOfMysteryEffect() - エレミネの固有能力
    public void ApplySeekerOfMysteryEffect(Character targetLeader)
    {
        int currentHP = targetLeader.IsPlayer ? GameManager.instance.playerHP : GameManager.instance.enemyHP;
        if (currentHP < 7)
        {
            Debug.Log("神秘への探究者 エレミネ: ダメージが増加しました！");
            if (targetLeader.IsPlayer)
            {
                CardManager.instance.playerNextAttackBonus += 5; // 攻撃ボーナスを追加
            }
            else
            {
                CardManager.instance.enemyNextAttackBonus += 5;
            }
        }
    }
    #endregion

    #region　ApplyPureSongstressEffect() - ドロシーの固有能力
    public void ApplyPureSongstressEffect(Character targetLeader)
    {
        //if (Random.value < 0.5f)
        //{
            Debug.Log("無垢な歌姫 ドロシー・レイン: カードを1枚引きました！");
            if (targetLeader.IsPlayer)
            {
                GameManager.instance.DrawCard(GameManager.instance.playerHand, GameManager.instance.playerDeck); // プレイヤーのデッキからドロー
            }
            else
            {
                GameManager.instance.DrawCard(GameManager.instance.enemyHand, GameManager.instance.enemyDeck); // 敵のデッキからドロー
            }
       // }
    }
    #endregion

    #region　ApplyYoungWitchEffect() - アイネの固有能力
    public void ApplyYoungWitchEffect(Character targetLeader)
    {
        Debug.Log("幼魔女 アイネ・ヴァンデンベルグ: 追加で1ダメージを与えました！");
        DamageManager.instance.StartDamageProcess(!targetLeader.IsPlayer, 1, 1); // 対象に追加ダメージを与える
    }
    #endregion

    #region
    private void ApplyProtectEffect(Character targetLeader)
    {
        Debug.Log("守護の壁: ダメージを軽減します！");
        DamageManager.instance.UseProtectCard(4); // ダメージを15軽減
    }
    #endregion
}

#region キャラクタークラス
// キャラクタークラス
public class Character
{
    public string Name { get; private set; }
    public bool IsPlayer { get; private set; } // trueならプレイヤー、falseなら敵

    public Character(string name, bool isPlayer)
    {
        Name = name;
        IsPlayer = isPlayer;
    }
}
#endregion
