using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource _effectAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private AudioClip _backgroundMusicClip;
    [SerializeField] private Slider _effectVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSettings()
    {
        _effectAudioSource.volume = PlayerPrefs.GetFloat(Constants.DATA.SETTINGS_SOUND, 1);
        _musicAudioSource.volume = PlayerPrefs.GetFloat(Constants.DATA.BACK_SOUND, 1);
        _effectVolumeSlider.value = _effectAudioSource.volume;
        _musicVolumeSlider.value = _musicAudioSource.volume;

        _effectVolumeSlider.onValueChanged.AddListener(ChangeEffectVolume);
        _musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void RegisterButtonSounds()
    {
        var buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => PlaySound(_buttonClickSound));
        }
    }

    public void PlaySound(AudioClip clip) =>
        _effectAudioSource.PlayOneShot(clip);

    public void StopSound(AudioClip clip)
    {
        if (_effectAudioSource.clip == clip && _effectAudioSource.isPlaying)
        {
            _effectAudioSource.Stop();
        }
    }

    public void ChangeEffectVolume(float volume)
    {
        _effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat(Constants.DATA.SETTINGS_SOUND, volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        _musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat(Constants.DATA.BACK_SOUND, volume);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _musicAudioSource.clip = _backgroundMusicClip;
        _musicAudioSource.loop = true;
        _musicAudioSource.Play();
    }
}
