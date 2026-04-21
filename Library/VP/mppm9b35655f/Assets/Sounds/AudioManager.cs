using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip gameStartClip;
    public AudioClip gameEndClip;
    public AudioClip coinCollectionClip;
    public AudioClip playerWallImpactClip;
    public AudioClip footstepsClip;
    public AudioClip coinSpawnClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGameStart() => audioSource.PlayOneShot(gameStartClip);
    public void PlayGameEnd() => audioSource.PlayOneShot(gameEndClip);
    public void PlayCoinCollection() => audioSource.PlayOneShot(coinCollectionClip);
    public void PlayPlayerWallImpact() => audioSource.PlayOneShot(playerWallImpactClip);
    public void PlayFootsteps() => audioSource.PlayOneShot(footstepsClip);
    public void PlayCoinSpawn() => audioSource.PlayOneShot(coinSpawnClip);
}