using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class AudioManagerSingleton
{
    public static class Audio
    {
        public static readonly string Bgm = "Audios/beat0203";
        public static readonly string Select = "Audios/button17";
        public static readonly string Cancel = "Audios/button14";
        public static readonly string Wall = "Audios/button32";
        public static readonly string Bound = "Audios/button69";
        public static readonly string Coin = "Audios/button70";
        public static readonly string OneUp = "Audios/button25";
        public static readonly string Out = "Audios/button24";
    }

    static AudioManagerSingleton instance;

    public static AudioManagerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AudioManagerSingleton();
            }
            return instance;
        }
    }

    AudioSource bgmAudioSource;
    AudioSource seAudioSource;
    Dictionary<string, AudioClip> audioMap;

    AudioManagerSingleton()
    {
        var go = new GameObject("AudioManagerSingleton");
        bgmAudioSource = go.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        seAudioSource = go.AddComponent<AudioSource>();

        audioMap = new Dictionary<string, AudioClip>();

        Load(Audio.Bgm);
        Load(Audio.Select);
        Load(Audio.Cancel);
        Load(Audio.Wall);
        Load(Audio.Bound);
        Load(Audio.Coin);
        Load(Audio.OneUp);
        Load(Audio.Out);
    }

 
    void Load(string key)
    {
        audioMap.Add(key, Resources.Load<AudioClip>(key));
    }

    public void PlayBgm()
    {
        bgmAudioSource.clip = audioMap[Audio.Bgm];
        bgmAudioSource.Play();
        bgmAudioSource.DOFade(1f, 0f);
    }

    public void FadeOutBgm()
    {
        var seq = DOTween.Sequence();
        seq.SetDelay(1f);
        seq.Append(bgmAudioSource.DOFade(0f, 1f));
        seq.Play();
    }

    public void PlaySe(string key)
    {
        if (!audioMap.ContainsKey(key))
        {
            return;
        }

        seAudioSource.PlayOneShot(audioMap[key]);
    }
}
