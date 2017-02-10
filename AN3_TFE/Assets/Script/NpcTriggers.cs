using UnityEngine;

public class NpcTriggers : MonoBehaviour
{

    GameObject npcParent;
    NpcManager npcManager;

    void Start()
    {
        npcParent = gameObject.transform.parent.gameObject;
        npcManager = npcParent.GetComponent<NpcManager>();
    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcManager.TriggerEnter();
        }
    }

    void OnTriggerStay(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcManager.TriggerStay();
        }
    }

    void OnTriggerExit(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcManager.TriggerExit();
        }
    }
}
