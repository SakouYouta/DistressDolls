using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPack : MonoBehaviour
{
    [SerializeField] Transform openedCardTrans; // 開封したカードの生成場所
    [SerializeField] Button openButton;         // 開封ボタン

    private bool isPackOpened;  // パックが開封されたかどうかのフラグ

    private void Start()
    {
        // 開封ボタンを非表示にする
        openButton.gameObject.SetActive(false);

        //パック開封昨日をリセット
        isPackOpened = false;
    }

    #region EnableOpenButton() - 開封ボタンを有効化するメソッド
    public void EnableOpenButton()
    {
        Debug.Log("開封ボタンを有効化します");

        // すでに開封済みなら処理しない
        if (isPackOpened) return;

        // ボタンを有効化
        openButton.gameObject.SetActive(true);
        Debug.Log("開封ボタンを表示しました");

        // リスナーが重複しないようにクリア
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(OpenPacks);
        Debug.Log("開封ボタンにクリックイベントを登録しました");
    }
    #endregion

    #region OpenPacks() - パックを開封するメソッド
    public void OpenPacks()
    {
        if (isPackOpened) return;  // すでに開封済みの場合は処理しない

        Debug.Log("パックを開封します");

        // GachaSystemManagerから選択されたカードリストを取得
        List<int> selectedPackList = GachaSystemManager.instance.GetSelectedPackList();

        if (selectedPackList == null || selectedPackList.Count == 0)
        {
            Debug.LogError("選択されたパックがない、またはパックが空です");
            return;
        }

        string displayText = "開封したカード: "; // 表示用文字列を初期化

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
        Debug.Log("開封ボタンを無効化しました");

        // 開封フラグを立てて、再度開封できないようにする
        isPackOpened = true;
        Debug.Log("パック開封フラグを設定しました");

        // 確定ボタンを表示
        GachaSystemManager.instance.confirmButton.gameObject.SetActive(true);
        Debug.Log("確認ボタンを表示しました");
    }
    #endregion

    #region DecisionCardId() - 選択されたリストからランダムにカードIDを決定するメソッド
    private int DecisionCardId(List<int> cardList)
    {
        return cardList[Random.Range(0, cardList.Count)];
    }
    #endregion
}

