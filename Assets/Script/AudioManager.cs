using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayer;
    int channelIndx;

    public enum Sfx { Slingshot ,Fly}

    private void Awake()
    {
        instance = this;
        Init();
    }
    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[channels];

        for (int index= 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        sfxPlayer[0].clip = sfxClip[(int)sfx];
        sfxPlayer[0].Play();
    }
}
