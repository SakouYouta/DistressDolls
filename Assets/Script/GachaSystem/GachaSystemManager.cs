using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSystemManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab; // カードプレハブ
    [SerializeField] Button pack1Button;        // Pack 1 ボタン
    [SerializeField] Button pack2Button;        // Pack 2 ボタン
    [SerializeField] Button pack3Button;        // Pack 3 ボタン
    [SerializeField] Text jueruText;            // 何個ジュエルを持ているか
    public Button confirmButton;                // 確認ボタン（追加）
    public Button resetButton;                  //ガチャリセットボタン
    private const int Gachacost = 120;          // ガチャの必要ソウル数

    // シングルトン化
    public static GachaSystemManager instance;

    // 各パックのカードリスト
    private List<int> packCardList1 = new List<int>() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
    private List<int> packCardList2 = new List<int>() { 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 };
    private List<int> packCardList3 = new List<int>() { 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };
    private List<int> selectedPackList = null; // 選択されたパックのリスト

    private List<GameObject> spawnedCards = new List<GameObject>(); // 生成されたカードを管理するリスト

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // 各パック選択ボタンにリスナーを追加
        pack1Button.onClick.AddListener(() => SelectPack(1));
        pack2Button.onClick.AddListener(() => SelectPack(2));
        pack3Button.onClick.AddListener(() => SelectPack(3));
        ResetGacha(); // シーン開始時にガチャ状態をリセット
        UpdateJueruText(); // 初期ジュエル表示
        DataSaveManager.AddSoul(120);
    }

    #region ResetGacha() - ガチャの状態をリセットするメソッド
    public void ResetGacha()
    {
        Debug.Log("ガチャの状態をリセットします");

        //選択されたパックをリセット
        selectedPackList = null;

        //ボタンの状態を初期化
        pack1Button.gameObject.SetActive(true);
        pack2Button.gameObject.SetActive(true);
        pack3Button.gameObject.SetActive(true);
        confirmButton.gameObject.SetActive(true);
        resetButton.gameObject.SetActive(true);

        // 生成されたカードを削除
        foreach (GameObject card in spawnedCards)
        {
            Destroy(card);
        }
        spawnedCards.Clear(); // リストもクリア

        // OpenPackの状態もリセット
        OpenPack openPack = FindAnyObjectByType<OpenPack>();
        if (openPack != null)
        {
            openPack.ResetPackOpening(); // 開封状態をリセット
        }
    }
    #endregion

    #region SelectPack() - パックを選択するメソッド
    public void SelectPack(int packNumber)
    {
        Debug.Log($"パック{packNumber}の選択処理を開始します");

        // ソウルが不足している場合は選択不可
        if (!DataSaveManager.SubtractionSoul(Gachacost))
        {
            Debug.LogError("ソウルが不足しています！");
            return;
        }

        // ソウル使用後にジュエル表示を更新
        UpdateJueruText();

        switch (packNumber)
        {
            case 1:
                selectedPackList = packCardList1;
                break;
            case 2:
                selectedPackList = packCardList2;
                break;
            case 3:
                selectedPackList = packCardList3;
                break;
            default:
                Debug.LogError("無効なパック番号が指定されました");
                return;
        }

        // 選択ボタンを非表示にする
        pack1Button.gameObject.SetActive(false);
        pack2Button.gameObject.SetActive(false);
        pack3Button.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);

        Debug.Log("選択前のパックボタン表示状態: " +
                  $"Pack1: {pack1Button.gameObject.activeSelf}, " +
                  $"Pack2: {pack2Button.gameObject.activeSelf}, " +
                  $"Pack3: {pack3Button.gameObject.activeSelf}");

        // OpenPackスクリプトに通知して開封ボタンを有効化
        OpenPack openPack = FindAnyObjectByType<OpenPack>();
        if (openPack != null)
        {
            Debug.Log("OpenPackスクリプトが見つかりました。開封ボタンを有効化します");
            openPack.EnableOpenButton();
        }
        else
        {
            Debug.LogError("OpenPackスクリプトが見つかりませんでした");
        }
    }
    #endregion

    #region GetSelectedPackList() - 現在の選択パックリストを取得する
    public List<int> GetSelectedPackList()
    {
        return selectedPackList;
    }
    #endregion

    #region OpenCardCreate() - カードを生成するメソッド（OpenPackで利用される）
    public void OpenCardCreate(int cardId, Transform trans)
    {
        if (selectedPackList == null)
        {
            Debug.LogError("パックが選択されていません。カードを生成できません");
            return;
        }

        CardController card = Instantiate(cardPrefab, trans);
        card.Init(cardId);

        // 生成されたカードをリストに追加
        spawnedCards.Add(card.gameObject);
    }
    #endregion

    #region UpdateJueruText() - ジュエルの量を更新するメソッド
    private void UpdateJueruText()
    {
        int currentJueru = DataSaveManager.GetSoul(); // 現在のソウル数を取得
        jueruText.text = $"{currentJueru}";
    }
    #endregion
}
