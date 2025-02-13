using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // AudioMixer
    [SerializeField] private Slider bgmSlider; // BGM音量スライダー
    [SerializeField] private Slider seSlider; // SE音量スライダー
    [SerializeField] private GameObject SettingPanel; // 設定パネル

    private void Start()
    {
        LoadAudioSettings(); // 設定を読み込む
        SettingPanel.SetActive(false);
    }

    #region SetBGMVolume() - BGM音量を変更
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20); // dB変換
    }
    #endregion

    #region SetSEVolume() - SE音量を変更
    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("SEVolume", Mathf.Log10(volume) * 20); // dB変換
    }
    #endregion

    #region SaveAudioSettings() - 設定を適用して保存
    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SEVolume", seSlider.value);
        PlayerPrefs.Save();
    }
    #endregion

    #region LoadAudioSettings() - 設定を読み込む
    private void LoadAudioSettings()
    {
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float seVol = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        bgmSlider.value = bgmVol;
        seSlider.value = seVol;

        SetBGMVolume(bgmVol);
        SetSEVolume(seVol);
    }
    #endregion

    #region ShowSettingPanel() - パネルを表示する
    public void ShowSettingPanel()
    {
        SettingPanel.SetActive(true);
    }
    #endregion

    #region HideSettingPanel() - パネルを非表示にする
    public void HideSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    #endregion
}
