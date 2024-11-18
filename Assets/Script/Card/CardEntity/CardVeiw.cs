using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, ablityText;
    [SerializeField] Image iconImage;

    public void Show(CardModel cardModel) // cardModel‚Ìƒf[ƒ^æ“¾‚Æ”½‰f
    {
        nameText.text = cardModel.name;
        ablityText.text = cardModel.ablity;
        iconImage.sprite = cardModel.icon;
    }
}