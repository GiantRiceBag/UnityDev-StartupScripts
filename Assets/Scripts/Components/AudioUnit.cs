using UnityEngine;

public enum AudioType
{
   Sound,
   Music
}

[RequireComponent(typeof(AudioSource))]
public class AudioUnit : MonoBehaviour
{
    [SerializeField] AudioType audioType = AudioType.Sound;
    AudioSource audioSource => GetComponent<AudioSource>();
    public AudioType AudioType
    {
        set
        {
            UpdateAudioType(value);
        }
        get
        { 
            return audioType;
        }
    }

    private void Awake()
    {
        switch (audioType)
        {
            case AudioType.Sound:
                {
                    AudioManager.Instance.onVolumeSoundChange += UpdateVolume;
                    audioSource.volume = AudioManager.Instance.VolumeSound;
                    break;
                }
            case AudioType.Music:
                {

                    AudioManager.Instance.onVolumeMusicChange += UpdateVolume;
                    audioSource.volume = AudioManager.Instance.VolumeMusic;
                    break;
                }
            default: break;
        }
    }

#if !UNITY_EDITOR
    private void OnDestroy()
    {
        switch (audioType)
        {
            case AudioType.Sound:
                {
                    AudioManager.Instance.onVolumeSoundChange -= UpdateVolume;
                    break;
                }
            case AudioType.Music:
                {

                    AudioManager.Instance.onVolumeMusicChange -= UpdateVolume;
                    break;
                }
            default: break;
        }
    }
#endif

    void UpdateAudioType(AudioType newType)
    {
        switch (audioType)
        {
            case AudioType.Sound:
                {
                    if(newType != audioType)
                    {
                        AudioManager.Instance.onVolumeMusicChange -= UpdateVolume;
                        AudioManager.Instance.onVolumeSoundChange += UpdateVolume;
                        UpdateVolume(AudioManager.Instance.VolumeSound);
                    }
                    break;
                }
            case AudioType.Music:
                {
                    if (newType != audioType)
                    { 
                        AudioManager.Instance.onVolumeMusicChange += UpdateVolume;
                        AudioManager.Instance.onVolumeSoundChange -= UpdateVolume;
                        UpdateVolume(AudioManager.Instance.VolumeMusic);
                    }
                break;
                }
            default: break;
        }
    }

    void UpdateVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
