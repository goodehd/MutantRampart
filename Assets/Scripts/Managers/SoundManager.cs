using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IManagers
{
    private List<AudioSource> _audioSourcesChannel = new List<AudioSource>();
    private Dictionary<string, AudioClip> _audioClipDic = new Dictionary<string, AudioClip>();
    public float BGMValue { get; set; }
    public float EffectValue { get; set; }
    public float UIValue { get; set; }

    public bool Init()
    {
        GameObject root = GameObject.Find("Sound");
        if (root == null)
        {
            root = new GameObject { name = "Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            foreach (ESoundType soundType in Enum.GetValues(typeof(ESoundType)))
            {
                GameObject obj = new GameObject { name = soundType.ToString() };
                _audioSourcesChannel.Add(obj.AddComponent<AudioSource>());
                obj.transform.parent = root.transform;
            }

            _audioSourcesChannel[(int)ESoundType.BGM].loop = true;
        }

        BGMValue = 1f;
        EffectValue = 1f;
        UIValue = 1f;
        return true;
    }

    public void SoundPlay(string path, ESoundType type)
    {
        // Resources/Sounds폴더 안 path를 넣어준다
        // ex) Resources/Sounds/UI/sound.egg일 경우 UI/sound으로 path를 전달.
        AudioClip audioClip = LoadClip(path);
        if (audioClip == null)
        {
            return;
        }

        AudioSource source = _audioSourcesChannel[(int)type];

        switch (type)
        {
            case ESoundType.BGM:
                source.clip = audioClip;
                source.Play();
                break;
            case ESoundType.Effect:
            case ESoundType.UI:
                source.PlayOneShot(audioClip);
                break;
            default:
                break;
        }
    }

    public void SoundStop(ESoundType type)
    {
        _audioSourcesChannel[(int)type].Stop();
    }

    public void SoundPause(ESoundType type)
    {
        _audioSourcesChannel[(int)type].Pause();
    }

    public void SoundResume(ESoundType type)
    {
        _audioSourcesChannel[(int)type].Play();
    }

    // volume은 0.0 ~ 1.0 사이의 값
    public void SetVolume(ESoundType type, float volume)
    {
        _audioSourcesChannel[(int)type].volume = volume;
    }

    // volume은 0.0 ~ 1.0 사이의 값
    public void SetAllVolume(float volume)
    {
        foreach (AudioSource audio in _audioSourcesChannel)
        {
            audio.volume = volume;
        }
    }

    public float GetVolume(ESoundType type)
    {
        return _audioSourcesChannel[(int)type].volume;
    }

    private AudioClip LoadClip(string path)
    {
        // 기본적으로 Resources 폴더 안의 Sounds폴더 안에 있는 sound파일을 찾는다.
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (_audioClipDic.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Main.Get<ResourceManager>().Load<AudioClip>(path);

            if (audioClip != null)
            {
                _audioClipDic.Add(path, audioClip);
            }
        }

        return audioClip;
    }

    public void Clear()
    {
        _audioSourcesChannel[(int)ESoundType.BGM].clip = null;
        _audioSourcesChannel[(int)ESoundType.BGM].Stop();
    }
}
