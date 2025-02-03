using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPack : MonoBehaviour
{
    [SerializeField] Transform openedCardTrans; // 開封したカードの生成場所
    [SerializeField] Button openButton;         // 開封ボタン

    private bool isPackOpened;                       // パックが開封されたかどうかのフラグ
    public List<int> GachaResult = null; 

    private void Start()
    {
        // 開封ボタンを非表示にする
        openButton.gameObject.SetActive(false);

        //パック開封機能をリセット
        isPackOpened = false;

        //ガチャで出たカードのIDを保存する関数をリセット
        GachaResult = new List<int>();
    }

    #region EnableOpenButton() - 開封ボタンを有効化するメソッド
    public void EnableOpenButton()
    {
        // すでに開封済みなら処理しない
        if (isPackOpened) return;

        // ボタンを有効化
        openButton.gameObject.SetActive(true);

        // リスナーが重複しないようにクリア
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(OpenPacks);
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

        // 3枚カードを生成する
        for (int i = 0; i < 3; i++)
        {
            int cardId = DecisionCardId(selectedPackList); // リストに基づいてランダムにカードIDを取得
            GachaSystemManager.instance.OpenCardCreate(cardId, openedCardTrans); // カード生成
            
            GachaResult.Add(cardId);

            // デバッグログを追加（現在のリストの状態を表示）
            Debug.Log($"カードID {cardId} をGachaResultに追加。現在のリスト: [{string.Join(", ", GachaResult)}]");
        }

        //ガチャで獲得したCardIDをセーブデータに引き渡す
        DataSaveManager.SavePossessionCard(GachaResult);

        // 開封後、ボタンを無効化して再度押せないようにする
        openButton.gameObject.SetActive(false);

        // 開封フラグを立てて、再度開封できないようにする
        isPackOpened = true;

        // 確定ボタンを表示
        GachaSystemManager.instance.confirmButton.gameObject.SetActive(true);
    }
    #endregion

    #region DecisionCardId() - 選択されたリストからランダムにカードIDを決定するメソッド
    private int DecisionCardId(List<int> cardList)
    {
        return cardList[Random.Range(0, cardList.Count)];
    }
    #endregion
}

