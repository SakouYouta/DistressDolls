using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // SE用のAudioSource
    [SerializeField] private AudioClip[] soundEffects; // 効果音リスト

    public static SEManager instance;

    // Awake() - インスタンスの初期化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // SEを再生するメソッド
    public void PlaySE(int index)
    {
        if (index < 0 || index >= soundEffects.Length) return;

        audioSource.PlayOneShot(soundEffects[index]); // 指定のSEを再生
    }
}
