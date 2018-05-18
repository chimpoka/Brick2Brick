using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    #region Private_Variables

    private AudioSource sourceSFX;

    private AudioSource sourceMusic;

    private AudioSource sourceRandomPitchSFX;

    [SerializeField]
    private float musicVolume = 0.5f;

    [SerializeField]
    private float sfxVolume = 0.5f;

    [SerializeField]
    private AudioClip[] sounds;

    [SerializeField]
    private AudioClip defaultClip;

    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private AudioClip gameMusic;

    #endregion



    #region Public_Variables

    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }

        set
        {
            musicVolume = value;
            sourceMusic.volume = musicVolume;
        }
    }

    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }

        set
        {
            sfxVolume = value;
            sourceSFX.volume = sfxVolume;
            sourceRandomPitchSFX.volume = sfxVolume;
        }
    }

    public AudioSource SourceSFX
    {
        get
        {
            return sourceSFX;
        }

        set
        {
            sourceSFX = value;
        }
    }

    public AudioSource SourceMusic
    {
        get
        {
            return sourceMusic;
        }

        set
        {
            sourceMusic = value;
        }
    }

    public AudioSource SourceRandomPitchSFX
    {
        get
        {
            return sourceRandomPitchSFX;
        }

        set
        {
            sourceRandomPitchSFX = value;
        }
    }

    #endregion



    #region User_Methods

    private AudioClip GetSound(string clipName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == clipName)
            {
                return sounds[i];
            }
        }
        Debug.LogError("Can not find clip " + clipName);
        return defaultClip;
    }

    public void PlaySound(string clipName)
    {
        SourceSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSFX.pitch = Random.Range(0.7f, 1.3f);
        SourceRandomPitchSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    public void PlayMusic(bool menu)
    {
        if (menu)
        {
            SourceMusic.clip = menuMusic;
        }
        else
        {
            SourceMusic.clip = gameMusic;
        }
        SourceMusic.volume = MusicVolume;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }

    #endregion
}
