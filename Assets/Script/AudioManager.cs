using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // AudioMixerの参照
    [SerializeField] private Slider BGMSlider; // BGM音量スライダー
    [SerializeField] private Slider SESlider; // SE音量スライダー
    [SerializeField] private GameObject settingPanel; // 設定パネル

    private void Start()
    {
        LoadAudioSettings(); // 設定を読み込む

        if (settingPanel != null)
        {
            settingPanel.SetActive(false); // 最初はパネルを非表示
        }

        // BGMの初期値をAudioMixerから取得
        if (audioMixer.GetFloat("BGMVolume", out float bgmVolume))
        {
            BGMSlider.value = Mathf.Pow(10, bgmVolume / 20); // dB → リニア変換
        }

        // SEの初期値をAudioMixerから取得
        if (audioMixer.GetFloat("SEVolume", out float seVolume))
        {
            SESlider.value = Mathf.Pow(10, seVolume / 20); // dB → リニア変換
        }

        // スライダーのリスナー登録
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SESlider.onValueChanged.AddListener(SetSEVolume);
    }

    #region SetBGMVolume() - BGM音量を変更
    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20; // リニア → dB変換
        audioMixer.SetFloat("BGMVolume", dB);
        SaveAudioSettings();
    }
    #endregion

    #region SetSEVolume() - SE音量を変更
    public void SetSEVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20; // リニア → dB変換
        audioMixer.SetFloat("SEVolume", dB);
        SaveAudioSettings();
    }
    #endregion

    #region SaveAudioSettings() - 設定を適用して保存
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value); // スライダーのリニア値を保存
        PlayerPrefs.SetFloat("SEVolume", SESlider.value);
        PlayerPrefs.Save(); // 設定を保存
    }
    #endregion

    #region LoadAudioSettings() - 設定を読み込む
    private void LoadAudioSettings()
    {
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float seVol = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        BGMSlider.value = bgmVol;
        SESlider.value = seVol;
    }
    #endregion

    #region ShowSettingPanel() - パネルを表示する
    public void ShowSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
    }
    #endregion

    #region HideSettingPanel() - パネルを非表示にする
    public void HideSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }
    #endregion
}
