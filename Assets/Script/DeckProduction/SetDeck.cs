using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SetDeck : MonoBehaviour, IDropHandler
{
    public List<int> deck = new List<int>();
    public int leader = 1;
    public GameObject warningPanel;
    public GameObject blackOut;
    public Text warningText;
    public bool active = true;
    [SerializeField] private Transform deckPanel;
    [SerializeField] private Transform possessionPanel;

    //外部から変更を加えられないように
    public ReadOnlyCollection<int> Deck => deck.AsReadOnly();

    void Start()
    {
        warningPanel.SetActive(active);
        blackOut.SetActive(active);

        // デッキデータ読み込み   
        deck = DataSaveManager.LoadDeckList();
        leader = DataSaveManager.LoadLeader();
    }

    #region OnDrop() カードがドロップされた際に呼ばれる
    public void OnDrop(PointerEventData eventData)
    {
        CardMovement cardMove = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardMove != null && cardMove.cardParent == possessionPanel)
        {
            CardController cardController = cardMove.GetComponent<CardController>();
            if (cardController != null)
            {
                CardModel cardModel = cardController.model;
                if (cardModel != null)
                {// カードIDを登録
                    if (deck.Count < 30)
                    {// デッキのカードが30枚までになるように
                        cardMove.drag = false;
                        deck.Add(cardModel.cardId); // カードをデッキに追加
                        Debug.Log("デッキ追加" + string.Join(", ", deck));
                        Debug.Log("追加したかーどID" + cardModel.cardId);
                        Debug.Log("追加後のデッキの数" + deck.Count);
                    }
                    else
                    {
                        cardMove.drag = true;
                        Debug.Log("カードが30枚以上です");
                        OpenWarningPanel(true);
                    }
                }
                else
                    Debug.LogError("カードモデル情報の取得に失敗しました");
            }
            else Debug.LogError("カードコントローラー情報の取得に失敗しました");
            cardMove.cardParent = deckPanel;
        }
        else if (cardMove == null)
            Debug.Log("カードムーブ情報の取得に失敗しました");
        else if (cardMove.cardParent == possessionPanel)
            Debug.Log("カードの親情報の取得に失敗しました");
    }
    #endregion

    #region OpenWarningPanel() warningPanelを表示、非表示
    public void OpenWarningPanel(bool checkDeck)
    {
        if (active == false)
            active = true;
        else
            active = false;
        if (checkDeck)
            warningText.text = "カードが30枚以上です";
        else
            warningText.text = "カードが30枚以下です";
        warningPanel.SetActive(active);
        blackOut.SetActive(active);
    }
    #endregion
}