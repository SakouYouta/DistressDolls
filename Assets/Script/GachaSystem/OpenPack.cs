using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPack : MonoBehaviour
{
    [SerializeField] Transform openedCardTrans; // 開封したカードの生成場所
    [SerializeField] Button openButton;         // 開封ボタン

    private bool isPackOpened = false;  // パックが開封されたかどうかのフラグ

    private void Start()
    {
        // 開封ボタンを非表示にする
        openButton.gameObject.SetActive(false);
    }

    // 開封ボタンを有効化するメソッド
    public void EnableOpenButton()
    {
        // ボタンを有効化して、リスナーを追加
        openButton.gameObject.SetActive(true);

        // ボタンが再表示されるたびにリスナーを1回だけ追加
        if (!isPackOpened)
        {
            openButton.onClick.AddListener(OpenPacks);
        }
    }

    // パックを開封するメソッド
    public void OpenPacks()
    {
        if (isPackOpened) return;  // すでに開封済みの場合は処理しない

        // GachaSystemManagerから選択されたカードリストを取得
        List<int> selectedPackList = GachaSystemManager.instance.GetSelectedPackList();

        if (selectedPackList == null || selectedPackList.Count == 0)
        {
            Debug.LogError("No pack selected or pack is empty.");
            return;
        }

        string displayText = "Opened Cards: "; // 表示用文字列を初期化

        // 3枚カードを生成する
        for (int i = 0; i < 3; i++)
        {
            int cardId = DecisionCardId(selectedPackList); // リストに基づいてランダムにカードIDを取得
            GachaSystemManager.instance.OpenCardCreate(cardId, openedCardTrans); // カード生成

            // ログ用文字列を更新
            displayText += cardId + (i < 2 ? ", " : "");
        }

        Debug.Log(displayText); // 開封結果をログに表示

        // 開封後、ボタンを無効化して再度押せないようにする
        openButton.gameObject.SetActive(false);

        // 開封フラグを立てて、再度開封できないようにする
        isPackOpened = true;
        
        GachaSystemManager.instance.confirmButton.gameObject.SetActive(true);
    }

    // 選択されたリストからランダムにカードIDを決定するメソッド
    private int DecisionCardId(List<int> cardList)
    {
        return cardList[Random.Range(0, cardList.Count)];
    }
}

