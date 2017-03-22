using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoad : MonoBehaviour
{
    List<GameObject> audioSources;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSources = new List<GameObject>();
        FindAllInstance();
        if (audioSources.Count > 1)
            Destroy(audioSources[1]);
    }

    void FindAllInstance()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Audio"))
        {
            audioSources.Add(obj);
        }
    }

}