using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel; // ポップアップ全体のパネル
    [SerializeField] private Transform cardButtonContainer; // Protectカードボタンを配置するコンテナ
    [SerializeField] private GameObject cardButtonPrefab; // カード選択ボタンのプレハブ

    private List<CardController> currentProtectCards; // 現在選択可能なProtectカード
    private System.Action<CardController> onCardSelected; // カード選択時のコールバック

    // ポップアップを開く
    public void ShowPopup(List<CardController> protectCards, System.Action<CardController> onSelect)
    {
        // 初期化
        popupPanel.SetActive(true);
        currentProtectCards = protectCards;
        onCardSelected = onSelect;

        // 古いボタンを削除
        foreach (Transform child in cardButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Protectカードごとにボタンを生成
        foreach (CardController card in protectCards)
        {
            GameObject buttonObj = Instantiate(cardButtonPrefab, cardButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            buttonText.text = $"{card.model.name}\n軽減値: {card.model.effectValue}";

            // ボタンがクリックされたときの処理
            button.onClick.AddListener(() =>
            {
                OnCardSelected(card);
            });
        }
    }

    // カードが選択されたとき
    private void OnCardSelected(CardController selectedCard)
    {
        popupPanel.SetActive(false); // ポップアップを閉じる
        onCardSelected?.Invoke(selectedCard); // コールバックを呼び出す
    }

    // キャンセル処理（例: Closeボタンに付与）
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        onCardSelected?.Invoke(null); // キャンセルとしてnullを返す
    }
}
