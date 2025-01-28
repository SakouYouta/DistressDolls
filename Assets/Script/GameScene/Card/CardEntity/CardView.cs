using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, ablityText;
    [SerializeField] Image iconImage;
    [SerializeField] Image BackImage;

    #region Show() カードの見た目を保存
    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        nameText.text = cardModel.name;
        ablityText.text = cardModel.ablity;
        iconImage.sprite = cardModel.icon;
    }
    #endregion

    #region InvisibleHand() - 背景画像の表示優先度を切り替えるメソッド
    public void InvisibleHand(bool isVisible)
    {
        BackImage.gameObject.SetActive(isVisible); // 背景画像を有効化または無効化
    }
    #endregion
}