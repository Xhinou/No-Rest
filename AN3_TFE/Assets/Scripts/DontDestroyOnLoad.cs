using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoad : MonoBehaviour
{
    List<GameObject> audioSources;
    QuestManager qManager;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSources = new List<GameObject>();
        FindAllInstance();
        if (audioSources.Count > 1)
        {
            qManager = GameObject.Find("ScriptSystem").GetComponent<QuestManager>();
            Destroy(audioSources[1]);
            qManager.theAudio = audioSources[0].GetComponent<AudioSource>();
        }
    }

    void FindAllInstance()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Audio"))
        {
            audioSources.Add(obj);
        }
    }

}