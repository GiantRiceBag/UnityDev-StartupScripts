using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : BSingleton<AudioManager>
{
    const string PATH_SOUND = "Sound/";
    const string PATH_MUSIC = "Music/";
    const int MAX_POOL_SIZE = 20;
    const float TIME_MUSIC_FADE = 1f;

    int playerID = 0;
    bool mute = false;

    List<AudioSource> playingPool = new List<AudioSource>();
    Queue<AudioSource> sourcePool = new Queue<AudioSource>();

    AudioSource sourceMusic;
    AudioSource SourceMusic
    {
        get
        {
            if (sourceMusic == null)
            {
                sourceMusic = gameObject.AddComponent<AudioSource>();
                sourceMusic.loop = true;
                sourceMusic.volume = VolumeMusic;
                sourceMusic.spatialBlend = 0;
            }
            return sourceMusic;
        }
    }

    [Range(0, 1f)] float volumeMain = 1;
    [Range(0, 1f)] float volumeMusic = 0.5f;
    [Range(0, 1f)] float volumeSound = 0.7f;

    public float VolumeMain
    {
        set
        {
            volumeMain = value;
            VolumeMusic = volumeMusic;
            VolumeSound = volumeSound;

            onVolumeMusicChange?.Invoke(VolumeMusic);
            onVolumeSoundChange?.Invoke(volumeSound);
        }
        get
        {
            return volumeMain;
        }
    }
    public float VolumeMusic
    {
        set
        {
            volumeMusic = value;
            SourceMusic.volume = VolumeMusic;

            onVolumeMusicChange?.Invoke(VolumeMusic);
        }
        get
        {
            return volumeMusic * volumeMain * (Mute ? 0 : 1);
        }
    }
    public float VolumeSound
    {
        set
        {
            volumeSound = value;
            foreach (var i in playingPool)
            {
                i.volume = VolumeSound;
            }

            onVolumeSoundChange?.Invoke(volumeSound);
        }
        get
        {
            return volumeSound * volumeMain * (Mute ? 0 : 1);
        }
    }
    protected bool Mute
    {
        set
        {
            if (mute != value)
            {
                mute = value;
                VolumeMain = VolumeMain;
            }
        }
        get
        {
            return mute;
        }
    }

    public event System.Action<float> onVolumeMusicChange;
    public event System.Action<float> onVolumeSoundChange;

    public void PlayMusic(string musicURL, bool fade = true, float fadeScale = 1)
    {
        AudioClip clip = LoadClip(PATH_MUSIC + musicURL);
        PlayMusic(clip, fade, fadeScale);
    }
    public void PlayMusic(AudioClip audioClip, bool fade = true, float fadeScale = 1)
    {
        if (fade)
        {
            if (SourceMusic.clip == null)
            {
                SourceMusic.Stop();
                SourceMusic.volume = 0;
                SourceMusic.clip = audioClip;
                SourceMusic.Play();
                SourceMusic.DOFade(VolumeMusic, TIME_MUSIC_FADE * fadeScale).SetEase(Ease.Linear);
            }
            else
            {
                SourceMusic.DOFade(0, TIME_MUSIC_FADE * fadeScale).SetEase(Ease.Linear).onComplete = () =>
                {
                    SourceMusic.Stop();
                    SourceMusic.clip = audioClip;
                    SourceMusic.Play();
                    SourceMusic.DOFade(VolumeMusic, TIME_MUSIC_FADE * fadeScale).SetEase(Ease.Linear);
                };
            }
        }
        else
        {
            SourceMusic.clip = audioClip;
            SourceMusic.volume = VolumeMusic;
            SourceMusic.Play();
        }
    }
    public void UnpauseMusic(bool fade = true,float fadeScale = 1)
    {
        if (sourceMusic.clip == null || !SourceMusic.isPlaying)
            return;
        SourceMusic.UnPause();
        if (fade)
            SourceMusic.DOFade(volumeMusic, TIME_MUSIC_FADE * fadeScale).SetEase(Ease.Linear);
        else
            SourceMusic.volume = volumeMusic;
    }

    public void PauseMusic(bool fade = true, float fadeScale = 1)
    {
        if (!SourceMusic.isPlaying)
            return;

        if (fade)
        {
            SourceMusic.DOFade(0, TIME_MUSIC_FADE * fadeScale).SetEase(Ease.Linear).onComplete = () =>
            {
                SourceMusic.Pause();
            };
        }
        else
        {
            SourceMusic.Pause();
        }
    }

    public int PlaySound(string soundURL, bool isOneShot = true, float scale = 1)
    {
        AudioClip clip = LoadClip(PATH_SOUND + soundURL);
        return PlaySound(clip, isOneShot, scale);
    }

    public int PlaySound(AudioClip audioClip, bool isOneShot = true, float scale = 1)
    {
        int id = playerID++;
        AudioSource source = GetSource();

        //source.volume = isOneShot ? Volume_Sound : Volume_Music;
        source.volume = VolumeSound;
        source.name = id.ToString();
        source.loop = !isOneShot;

        if (isOneShot)
        {
            source.PlayOneShot(audioClip, scale); ;
            ReleaseSource(source, audioClip.length);
        }
        else
        {
            source.clip = audioClip;
            source.Play();
        }

        return id;
    }

    void ReleaseSource(AudioSource source, float time)
    {
        StartCoroutine(CReleaseSource(source, time));
    }

    IEnumerator CReleaseSource(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);
        ReleaseSource(source);
    }

    void ReleaseSource(AudioSource source)
    {
        if (sourcePool.Count >= MAX_POOL_SIZE)
        {
            Destroy(source.gameObject);
        }
        else
        {
            source.clip = null;
            sourcePool.Enqueue(source);
        }

        playingPool.Remove(source);
    }

    AudioClip LoadClip(string url)
    {
        AudioClip clip = Resources.Load<AudioClip>(url);
        if (clip is null)
        {
            Debug.LogError($"Unable to find audio clip at {url}");
            return null;
        }
        return clip;
    }

    AudioSource GetSource(int playerID)
    {
        return transform.Find(playerID.ToString()).GetComponent<AudioSource>();
    }

    AudioSource GetSource()
    {
        AudioSource source;

        if (sourcePool.Count != 0)
        {
            source = sourcePool.Dequeue();
        }
        else
        {
            GameObject go = new GameObject("AudioSource");
            go.transform.SetParent(transform);

            source = go.AddComponent<AudioSource>();
            source.spatialBlend = 0;
        }

        playingPool.Add(source);
        return source;
    }
}
