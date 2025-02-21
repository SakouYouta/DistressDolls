using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectLeader : MonoBehaviour
{
    [SerializeField] private Sprite[] leaderImage;
    [SerializeField] private Button changeButton;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private SetDeck setDeck;

    private void Start()
    {
        int leader = DataSaveManager.LoadLeader();
        ChangeLeader(leader);
        button1.onClick.AddListener(() => ChangeLeader(1));
        button2.onClick.AddListener(() => ChangeLeader(2));
        button3.onClick.AddListener(() => ChangeLeader(3));
    }

    #region ChangeLeader()-リーダーの切り替え
    private void ChangeLeader(int leader)
    {
        List<int> deck = DataSaveManager.LoadDeckList();
        switch (leader)
        {
            case 1 :
                if (deck[0] != 1)
                    deck[0] = 1;
                changeButton.image.sprite = leaderImage[0];
                break;
            case 2 :
                if (deck[0] != 2)
                    deck[0] = 2;
                changeButton.image.sprite = leaderImage[1];
                break;
            case 3 :
                if (deck[0] != 3)
                    deck[0] = 3;
                changeButton.image.sprite = leaderImage[2];
                break;
            default : break;
        }
        setDeck.deck = deck;
        selectPanel.SetActive(false);
    }
    #endregion

    #region ActivePanel()-セレクトパネルのアクティブ化
    public void ActivePanel()
    {
        selectPanel.SetActive(true);
    }
    #endregion
}