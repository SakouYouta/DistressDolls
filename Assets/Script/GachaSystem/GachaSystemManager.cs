using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GachaSystemManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab; // カードプレハブ 

    // シングルトン化
    public static GachaSystemManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // カードを生成するメソッド（Public化した）
    public void OpenCardCreate(int cardId, Transform trans)
    {
        // cardPrefabをopenedCardTransに生成する
        CardController card = Instantiate(cardPrefab, trans);
        card.Init(cardId);
    }
}
