using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectLeader : MonoBehaviour
{
    [SerializeField] private Image[] leaderImage;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    private int leader;

    private void Start()
    {
        leader = DataSaveManager.LoadDeckList().FirstOrDefault();
        button1.onClick.AddListener(() => ChangeLeader(1));
        button2.onClick.AddListener(() => ChangeLeader(2));
        button2.onClick.AddListener(() => ChangeLeader(3));
    }

    #region ChangeLeader()-ƒŠ[ƒ_[‚ÌØ‚è‘Ö‚¦
    private void ChangeLeader(int leader)
    {
        List<int> deck = DataSaveManager.LoadDeckList();
        switch (leader)
        {
            case 1 :
                if (deck[0] != 1)
                    deck[0] = 1;
                break;
            case 2 :
                if (deck[0] != 2)
                    deck[0] = 2;
                break;
            case 3 :
                if (deck[0] != 3)
                    deck[0] = 3;
                break;
            default : break;
        }
        DataSaveManager.SaveDeckList(deck);
    }
    #endregion
}