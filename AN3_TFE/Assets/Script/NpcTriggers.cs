using UnityEngine;
using System.Collections;

public class NpcTriggers : MonoBehaviour
{

    GameObject npcParent;

    void Start()
    {
        npcParent = gameObject.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcParent.GetComponent<NpcManager>().TriggerEnter();
        }
    }

    void OnTriggerStay(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcParent.GetComponent<NpcManager>().TriggerStay();
        }
    }

    void OnTriggerExit(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            npcParent.GetComponent<NpcManager>().TriggerExit();
        }
    }
}
