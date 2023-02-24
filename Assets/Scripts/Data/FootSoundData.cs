using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string SoundTag;
    public AudioClip[] SoundList;
}

[CreateAssetMenu(menuName ="MyFPS Data/ Foot Sound Data")]
public class FootSoundData:ScriptableObject
{
    public SoundData[] FootSoundList;

    public AudioClip GetSoundByTag(string tag)
    {
        foreach (SoundData sd in FootSoundList)
        {
            if(sd.SoundTag == tag)
            {
                return sd.SoundList[Random.Range(0,sd.SoundList.Length)];
            }
        }
        return null;
    }

}
