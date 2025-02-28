using UnityEngine;
using UnityEngine.UI;

public class WinResultSceneManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText; // キャラのセリフを表示するUIのTextコンポーネント
    [SerializeField] private Image characterImage; // キャラクターの画像を表示するImage
    [SerializeField] private Sprite researcherSprite; // 研究者の画像
    [SerializeField] private Sprite angelSprite; // 天使の画像
    [SerializeField] private Sprite witchSprite; // 魔女の画像

    void Start()
    {
        ShowWinDialogue();
    }

    #region ShowWinDialogue() - 勝利時のセリフを表示
    private void ShowWinDialogue()
    {
        string dialogue = "";

        switch (DataSaveManager.LoadLeader())
        {
            case 1:
                dialogue = GetRandomDialogue(new string[]
                {
                    "ふむ…やはり私の理論は正しかったな。",
                    "実験は成功だな！この戦いのデータを記録しよう。",
                    "フフ、もっと強い相手が現れることを期待しよう。"
                });
                characterImage.sprite = researcherSprite;
                break;

            case 2:
                dialogue = GetRandomDialogue(new string[]
                {
                    "やったね！信じる力が勝利を呼び込んだよ！",
                    "最高の試合だったね！私も嬉しい！",
                    "この勝利、あなたの努力の賜物だよ！"
                });
                characterImage.sprite = angelSprite;
                break;

            case 3:
                dialogue = GetRandomDialogue(new string[]
                {
                    "フフ…やっぱり私の力は絶対ね。",
                    "勝つのは当然よ。もっと強い相手はいないの？",
                    "まあまあ…悪くない試合だったわ。"
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

    #region GetRandomDialogue() - ランダムなセリフを取得する
    private string GetRandomDialogue(string[] dialogues)
    {
        return dialogues[Random.Range(0, dialogues.Length)];
    }
    #endregion
}
