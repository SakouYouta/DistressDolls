using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えを扱うための名前空間

public class SceneManagerScript : MonoBehaviour
{
    // メニューシーンやバトルシーンなどの名前を定数として管理します
    private const string TITLE_SCENE = "TitleScene";
    private const string MENU_SCENE = "MenuScene";
    private const string BATTLE_SCENE = "GameScene";
    private const string GACHA_SCENE = "gachaSystem";
    private const string DECK_SCENE = "DeckProduction";
    private const string WINRESULT_SCENE = "WinResultScene";
    private const string LOSERESULT_SCENE = "LoseResultScene";

    /// <summary>
    /// 指定されたシーンをロードします。
    /// </summary>
    /// <param name="sceneName">ロードしたいシーン名</param>
    public void LoadScene(string sceneName)
    {
        // シーン名が正しいか確認しながら、指定されたシーンをロードします
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) != -1)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("シーン名が無効です: " + sceneName);
        }
    }

    /// <summary>
    /// タイトルシーンに移動します。
    /// </summary>
    public void LoadTitleScene()
    {
        LoadScene(TITLE_SCENE);
    }

    /// <summary>
    /// メニューシーンに移動します。
    /// </summary>
    public void LoadMenuScene()
    {
        LoadScene(MENU_SCENE);
    }

    /// <summary>
    /// バトルシーンに移動します。
    /// </summary>
    public void LoadBattleScene()
    {
        LoadScene(BATTLE_SCENE);
    }

    /// <summary>
    /// ガチャシーンに移動します。
    /// </summary>
    public void LoadGachaScene()
    {
        LoadScene(GACHA_SCENE);
    }

    /// <summary>
    /// デッキ生成シーンに移動します。
    /// </summary>
    public void LoadDeckScene()
    {
        LoadScene(DECK_SCENE);
    }

    /// <summary>
    /// 勝利リザルトシーンに移動します。
    /// </summary>
    public void LoadWinResultScene()
    {
        LoadScene(WINRESULT_SCENE);
    }

    /// <summary>
    /// 敗北リザルトシーンに移動します。
    /// </summary>
    public void LoadLoseResultScene()
    {
        LoadScene(LOSERESULT_SCENE);
    }
}
