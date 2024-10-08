using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource _effectSource;

    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioClip _clickSound;

    [SerializeField]
    private AudioClip _backgroundMusic;

    [SerializeField]
    private Slider _volumeSlider;

    [SerializeField]
    private Slider _musicVolumeSlider;

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
            return;
        }
        _effectSource.volume = PlayerPrefs.GetFloat(Constants.DATA.SETTINGS_SOUND, 1);
        _musicSource.volume = PlayerPrefs.GetFloat(Constants.DATA.BACK_SOUND, 1);
        _volumeSlider.value = _effectSource.volume;
        _musicVolumeSlider.value = _musicSource.volume;
        _volumeSlider.onValueChanged.AddListener(ChangeVolume);
        _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddButtonSound()
    {
        var buttons = FindObjectsOfType<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => {
                PlaySound(_clickSound);
            });
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void StopSound(AudioClip clip)
    {
        if (_effectSource.clip == clip && _effectSource.isPlaying)
        {
            _effectSource.Stop();
        }
    }

    public void ChangeVolume(float volume)
    {
        _effectSource.volume = volume;
        PlayerPrefs.SetFloat(Constants.DATA.SETTINGS_SOUND, volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat(Constants.DATA.BACK_SOUND, volume);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _musicSource.clip = _backgroundMusic;
        _musicSource.loop = true;
        _musicSource.Play();
    }
}
