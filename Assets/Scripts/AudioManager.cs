using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip bounceSound;
    public AudioClip spikeHitSound;
    public AudioClip gameEndSound;

    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump() => audioSource.PlayOneShot(jumpSound);
    public void PlayBounce() => audioSource.PlayOneShot(bounceSound);
    public void PlaySpikeHit() => audioSource.PlayOneShot(spikeHitSound);
    public void PlayGameEnd() => audioSource.PlayOneShot(gameEndSound);
}