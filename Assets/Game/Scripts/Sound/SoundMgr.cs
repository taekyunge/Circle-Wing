using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : Singleton<SoundMgr>
{
    [SerializeField] private Sound[] sounds;

    private Pooling<Sound>[] soundPools;

    private void Start()
    {
        soundPools = new Pooling<Sound>[sounds.Length];

        for (int i = 0; i < sounds.Length; i++)
        {
            soundPools[i] = new Pooling<Sound>(5, sounds[i], transform);
        }
    }

    public void Play(SoundType soundType)
    {
        int index = (int)soundType;

        var sound = soundPools[index].Get();

        sound.Play();
    }

    public void DeleteSound(Sound sound)
    {
        int index = (int)sound.SoundType;

        sound.Stop();
        soundPools[index].Delete(sound);
    }
}
