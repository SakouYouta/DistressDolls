using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    [SerializeField] private GameObject exitPanel; // ゲーム終了確認パネル

    private void Start()
    {
        exitPanel.SetActive(false);
    }

    #region ShowExitPanel() - パネルを表示する
    public void ShowExitPanel()
    {
        exitPanel.SetActive(true);
    }
    #endregion

    #region HideExitPanel() - パネルを非表示にする
    public void HideExitPanel()
    {
        exitPanel.SetActive(false);
    }
    #endregion

    #region QuitGame() - ゲームを終了する
    public void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion
}
