using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SoundManager
{
    private AudioSource bgmAudioSource;
    private AudioSource effectAudioSource;
    private AudioSource uiAudioSource;

    private GameObject soundRoot = null;

    public float BGMVolume = 1;

    public SoundManager()
    {
        Init();
    }

    public void Init()
    {
        if (soundRoot == null)
        {
            soundRoot = GameObject.Find("@SoundRoot");
            if (soundRoot == null)
            {
                soundRoot = new GameObject { name = "@SoundRoot" };
                UnityEngine.Object.DontDestroyOnLoad(soundRoot);

                GameObject go = new GameObject { name = "BGMAudioSource" };
                go.transform.SetParent(soundRoot.transform);
                bgmAudioSource = go.AddComponent<AudioSource>();
                bgmAudioSource.loop = true;
                
                go = new GameObject { name = "EffectAudioSource" };
                go.transform.SetParent(soundRoot.transform);
                effectAudioSource = go.AddComponent<AudioSource>();
                
                go = new GameObject { name = "UIAudioSource" };
                go.transform.SetParent(soundRoot.transform);
                uiAudioSource = go.AddComponent<AudioSource>();

            }
        }
    }

    public void Clear()
    {
        bgmAudioSource = null; 
        effectAudioSource = null; 
        uiAudioSource = null;
        Managers.Resource.Destroy(soundRoot);
    }

    public void PlayEffect(string _clipKey)
    {
        effectAudioSource.PlayOneShot(Managers.Resource.Load<AudioClip>(_clipKey));
    }

    public void PlayUI(string _clipKey)
    {
        uiAudioSource.PlayOneShot(Managers.Resource.Load<AudioClip>(_clipKey));
    }

    public void PlayBGM(string _clipKey)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = Managers.Resource.Load<AudioClip>(_clipKey);
        bgmAudioSource.Play();
    }

    // 배경음악 FadeIn 설정
    public void FadeInBGM(string _clipKey, float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeInBGMRoutine(_clipKey, _fadeTime, _callback));
    }

    // 배경음악 FadeIn 루틴
    private IEnumerator FadeInBGMRoutine(string _clipKey, float _fadeTime, Action _callback = null)
    {
        bgmAudioSource.volume = 0;
        PlayBGM(_clipKey);

        while (bgmAudioSource.volume < BGMVolume)
        {
            bgmAudioSource.volume += BGMVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }

        _callback?.Invoke();
    }

    // 배경음악 FadeOut 설정
    public void FadeOutBGM(float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeOutBGMRoutine(_fadeTime, _callback));
    }

    // 배걍음악 FadeOut 루틴
    private IEnumerator FadeOutBGMRoutine(float fadeTime, Action _callback = null)
    {
        while (bgmAudioSource.volume > 0.0f)
        {
            bgmAudioSource.volume -= BGMVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        bgmAudioSource.Stop();
        _callback?.Invoke();
    }

    // 배경음악 FadeIn,FadeOut으로 변경
    public void FadeChangeBGM(string _clipKey, float _fadeTotalTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeChangeBGMRoutine(_clipKey, _fadeTotalTime, _callback));
    }

    // 배경음악 FadeIn, FadeOut 루틴
    private IEnumerator FadeChangeBGMRoutine(string _clipKey, float _fadeTotalTime, Action _callback = null)
    {
        yield return Managers.Routine.StartCoroutine(FadeOutBGMRoutine(_fadeTotalTime * 0.5f));
        yield return Managers.Routine.StartCoroutine(FadeInBGMRoutine(_clipKey, _fadeTotalTime * 0.5f));
        _callback?.Invoke();
    }
}