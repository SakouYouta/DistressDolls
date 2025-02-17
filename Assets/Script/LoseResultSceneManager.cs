using UnityEngine;
using UnityEngine.UI;

public class LoseResultSceneManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText; // キャラのセリフを表示するUIのTextコンポーネント
    [SerializeField] private Image characterImage; // キャラクターの画像を表示するImage
    [SerializeField] private Sprite researcherSprite; // 研究者の画像
    [SerializeField] private Sprite angelSprite; // 天使の画像
    [SerializeField] private Sprite witchSprite; // 魔女の画像

    private enum CharacterType { Researcher, Angel, Witch } // キャラクターの種類
    [SerializeField] private CharacterType currentCharacter; // 現在のキャラクター

    void Start()
    {
        ShowLoseDialogue();
    }

    #region ShowLoseDialogue() - 敗北時のセリフを表示
    private void ShowLoseDialogue()
    {
        string dialogue = "";

        switch (currentCharacter)
        {
            case CharacterType.Researcher:
                dialogue = GetRandomDialogue(new string[]
                {
                    "くっ…理論だけでは勝てないのか…。",
                    "データを見直す必要があるな。",
                    "この敗北…次の勝利への糧にしよう。"
                });
                characterImage.sprite = researcherSprite;
                break;

            case CharacterType.Angel:
                dialogue = GetRandomDialogue(new string[]
                {
                    "負けちゃったね…でも次はきっと勝てるよ！",
                    "結果だけじゃないよ、大事なのは楽しむこと！",
                    "次はきっと天使の加護があるはず！"
                });
                characterImage.sprite = angelSprite;
                break;

            case CharacterType.Witch:
                dialogue = GetRandomDialogue(new string[]
                {
                    "あら…負けちゃった？悔しいわね。",
                    "この屈辱…必ず晴らしてみせるわ！",
                    "次は…私の魔法で完璧に勝つわよ。"
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
