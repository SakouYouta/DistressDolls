using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPack : MonoBehaviour
{
    [SerializeField] GameObject cardPackPrefab; // カードパックプレハブ
    [SerializeField] Transform openedCardTrans; // 開封したカードの生成場所
    [SerializeField] Transform cardPackTrans; // カードパックの生成場所

    public List<int> packCardList1 = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // パックのカードリスト

    private void Start()
    {
        OpenPacks();
    }

    // パックを開封するメソッド
    public void OpenPacks()
    {

        string displayText = "Opened Cards: "; // 表示用文字列を初期化

        // 3枚カードを生成する
        for (int i = 0; i < 3; i++)
        {
            int cardId = decisionCardId();
            GachaSystemManager.instance.OpenCardCreate(cardId, openedCardTrans);

            // 取得したカードIDを文字列に追加
            displayText += cardId + (i < 2 ? ", " : "");
        }

        // デバッグログに表示
        Debug.Log(displayText);
    }

    // カードのIDをランダムに決めるメソッド
    int decisionCardId()
    {
        int randomCardId = packCardList1[Random.Range(0, packCardList1.Count)];
        return randomCardId;
    }
}