using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isPlayerField; // このフィールドがプレイヤー用かエネミー用か

    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>(); // ドラッグしてきた情報からCardMovementを取得
        if (card != null) // もしカードがあれば、
        {
            card.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする
        }

        // ダメージ処理を呼び出す
        int damage = 1; // 仮に固定ダメージ1
        if (isPlayerField)
        {
            // プレイヤーフィールドに配置された場合、エネミーにダメージ
            GameManager.instance.DecreaseHP(false, damage);
        }
        else
        {
            // エネミーフィールドに配置された場合、プレイヤーにダメージ
            GameManager.instance.DecreaseHP(true, damage);
        }
    }
}