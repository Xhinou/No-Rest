using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public void ChangeVol(float newValue)
    {
        float newVol = AudioListener.volume;
        newVol = newValue;
        AudioListener.volume = newVol;
        QuestManager.audioLisVolume = newVol;
    }
}