using UnityEngine;
using UnityEngine.EventSystems;

public class SetDeck : MonoBehaviour, IDropHandler
{
    private int cardNum;// カードのID情報を格納
    void Start() { }
    void Update() { }

    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null)
        {
            CardModel cardModel = cardMove.GetComponent<CardController>().model;

            cardMove.cardParent = this.transform;

            // カードのID情報を取得
        }
    }
}
