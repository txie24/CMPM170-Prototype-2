using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private AudioSource sfxSource; 

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip changingGravitySFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayJumpSound()
    {
        if (jumpSFX != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(jumpSFX);
        }
    }

    public void PlayGravityChangeSound()
    {
        if (changingGravitySFX != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(changingGravitySFX);
        }
    }
}