using UnityEngine;

// カード効果の種類を表す列挙型
public enum CardEffectType
{
    Damage,     // ダメージを与える
    Protect,    // ダメージをおさえる
    DrawCard,   // カードを引く
    Doll,       //人形の固有所持能力
    Researcher, //研究者の固有能力
    Angel,      //天使の固有能力
    Witch,      //魔女の固有能力
}
