using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    private Transform originalParent;  // 最初の親を保持する変数
    public static bool drag = false;

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        originalParent = transform.parent;
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (drag == false)
        {
            transform.SetParent(cardParent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        }
        else
            //最初にドラッグを始めた親に戻す
            transform.SetParent(originalParent, false);

        drag = false;
    }
}