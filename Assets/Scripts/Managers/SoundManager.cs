using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : IManagers
{
    private List<AudioSource> _audioSourcesChannel = new List<AudioSource>();
    private Dictionary<string, AudioClip> _audioClipDic = new Dictionary<string, AudioClip>();

    public bool Init()
    {
        GameObject root = GameObject.Find("Sound");
        if (root == null)
        {
            root = new GameObject { name = "Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
            {
                GameObject obj = new GameObject { name = soundType.ToString() };
                _audioSourcesChannel.Add(obj.AddComponent<AudioSource>());
                obj.transform.parent = root.transform;
            }

            _audioSourcesChannel[(int)SoundType.BGM].loop = true;
        }
        return true;
    }

    public void SoundPlay(string path, SoundType type)
    {
        // Resources/Sounds폴더 안 path를 넣어준다
        // ex) Resources/Sounds/UI/sound.egg일 경우 UI/sound으로 path를 전달.
        AudioClip audioClip = LoadClip(path);
        if (audioClip == null)
        {
            Debug.Log("None AudioClip");
            return;
        }

        AudioSource source = _audioSourcesChannel[(int)type];

        switch (type)
        {
            case SoundType.BGM:
                source.clip = audioClip;
                source.Play();
                break;
            case SoundType.Effect:
            case SoundType.UI:
                source.PlayOneShot(audioClip);
                break;
            default:
                Debug.Log("Invalid Sound Type");
                break;
        }
    }

    public void SoundStop(SoundType type)
    {
        _audioSourcesChannel[(int)type].Stop();
    }

    public void SoundPause(SoundType type)
    {
        _audioSourcesChannel[(int)type].Pause();
    }

    public void SoundResume(SoundType type)
    {
        _audioSourcesChannel[(int)type].Play();
    }

    // volume은 0.0 ~ 1.0 사이의 값
    public void SetVolume(SoundType type, float volume)
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

    public float GetVolume(SoundType type)
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
        _audioSourcesChannel[(int)SoundType.BGM].clip = null;
        _audioSourcesChannel[(int)SoundType.BGM].Stop();
    }
}
