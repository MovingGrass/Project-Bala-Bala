using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _mixer;

    [Header("Audio Start Volume")]
    public float masterStartVolume = 0.5f;
    public float musicStartVolume = 0.5f;
    public float sfxStartVolume = 0.5f;

    private const string MIXER_MASTER = "MasterVol";
    private const string MIXER_MUSIC = "BGMVol";
    private const string MIXER_SFX = "SFXVol";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    protected override void Awake()
    {
        base.Awake();

        _musicSource.loop = true;
    }

    public void Start()
    {
        //Later Change 0.5f to player Pref
        SetMasterVolume(masterStartVolume);
        SetMusicVolume(musicStartVolume);
        SetSFXVolume(sfxStartVolume);
    }

    public void SetMasterVolume(float sliderValue)
    {
        SetVolume(MIXER_MASTER, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        SetVolume(MIXER_MUSIC, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        SetVolume(MIXER_SFX, sliderValue);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicSource.clip == clip) return;

        // Stop any current fade animation on this object and reset volume
        _musicSource.DOKill();
        _musicSource.volume = 1f;

        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void TransitionMusic(AudioClip clip, float duration = 1f)
    {
        _musicSource.DOKill();

        _musicSource.DOFade(0f, duration).OnComplete(() =>
        {
            _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.Play();
        });
    }

    public void StopMusicWithFade(float duration = 1f)
    {
        _musicSource.DOKill();

        _musicSource.DOFade(0f, duration).OnComplete(() =>
        {
            _musicSource.Stop();
            _musicSource.clip = null;
        });
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        float value = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float db = Mathf.Log10(value) * 20;

        _mixer.SetFloat(parameterName, db);
    }
}
