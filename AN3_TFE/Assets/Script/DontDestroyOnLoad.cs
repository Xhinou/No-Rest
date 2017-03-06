using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private bool destroyingAtFirst = false;

    void Awake()
    {
        if (!destroyingAtFirst)
        {
            destroyingAtFirst = true;
            DontDestroyOnLoad(transform.gameObject);
        }       
    }

}