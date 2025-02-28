using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText; // キャラのセリフを表示するUIのTextコンポーネント
    [SerializeField] private Image characterImage; // キャラクターの画像を表示するImage
    [SerializeField] private Sprite researcherSprite; // 研究者の画像
    [SerializeField] private Sprite angelSprite; // 天使の画像
    [SerializeField] private Sprite witchSprite; // 魔女の画像
    
    void Start()
    {
        ShowDialogue();
    }

    #region ShowDialogue() - キャラクターごとに異なるセリフを表示
    private void ShowDialogue()
    {
        string dialogue = "";

        switch (DataSaveManager.LoadLeader())
        {
            case 1:
                dialogue = GetRandomDialogue(new string[]
                {
                    "ふむ…このカードの組み合わせは興味深いな。",
                    "理論上、このデッキは最適解だが…実験が必要だな。",
                    "データを取るには、もっとバトルをしなければ！"
                });
                characterImage.sprite = researcherSprite;
                break;

            case 2:
                dialogue = GetRandomDialogue(new string[]
                {
                    "あなたの努力は、きっと報われるよ！",
                    "カードゲームって、信じる力が大事なんだよ。",
                    "今日も頑張ってね！天使の加護を感じて！"
                });
                characterImage.sprite = angelSprite;
                break;

            case 3:
                dialogue = GetRandomDialogue(new string[]
                {
                    "フフ…面白い実験台が来たわね。",
                    "魔法とカード、どちらが強いか試してみる？",
                    "このデッキ…なかなか良いわね。気に入ったわ。"
                });
                characterImage.sprite = witchSprite;
                break;

            default:
                dialogue = "キャラクターが選択されていません。";
                break;
        }

        if (dialogueText != null)
        {
            dialogueText.text = dialogue; // セリフを表示
        }
        else
        {
            Debug.LogError("dialogueTextが設定されていません！");
        }
    }
    #endregion

    #region GetRandomDialogue() - 指定されたセリフリストからランダムに選択する
    private string GetRandomDialogue(string[] dialogues)
    {
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    #endregion

    #region GachaCoinGet() - デバックでガチャ石を獲得
    public void GachaCoinGet()
    {
        DataSaveManager.AddSoul(120);
    }
    #endregion
}
