using UnityEngine;
using System.Collections;

public class TriggersManager : MonoBehaviour
{

    public GameObject scriptSystem;
    QuestManager qManager;
    CharacterClickingController controller;
    public GameObject player;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            StartCoroutine(TriggerQuest());
        }
    }

    public IEnumerator TriggerQuest()
    {
        if (qManager.sceneID == 2)
        {
            GameObject sailor = GameObject.Find("SailorBody");
            if (name == "SailorGoRight")
            {
                qManager.hasFollowedSailor = true;
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                GameObject newPos = GameObject.Find("SailorPosA");
                sailor.transform.position = newPos.transform.position;
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                sailor.GetComponent<NpcManager>().isTalkable = true;
                sailor.GetComponent<NpcManager>().DisplayObject();
            }
            else if (name == "SailorGoLeft")
            {
                qManager.hasFollowedSailor = false;
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                GameObject newPos = GameObject.Find("SailorPosB");
                sailor.transform.position = newPos.transform.position;
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                sailor.GetComponent<NpcManager>().isTalkable = true;
                sailor.GetComponent<NpcManager>().DisplayObject();
            }
            else if (name == "SailorInterrupt")
            {
                sailor.tag = "talking";
                StartCoroutine(qManager.SailorQuest(0));
            }
            else if (name == "SailorBlock")
            {
                controller.hasControl = false;
                controller.agent.ResetPath();
                controller.canSkipDial = false;
                GameObject newPos = GameObject.Find("SailorBlockPos");
                sailor.GetComponent<NpcManager>().canvas[3].SetActive(true);
                controller.agent.destination = newPos.transform.position;
                while (controller.agent.transform.position.z != newPos.transform.position.z)
                    yield return null;
                sailor.GetComponent<NpcManager>().canvas[3].SetActive(false);
                controller.hasControl = true;
                controller.canSkipDial = true;
                sailor.GetComponent<NpcManager>().isTalkable = true;
            }
            else if (name == "ScrewThis")
            {
                GameObject greed = GameObject.Find("GreedSailorBody");
                greed.tag = "talking";
                greed.GetComponent<NpcManager>().isTalkable = true;
                controller.agent.ResetPath();
                qManager.GreedQuest(0);
            }
            else if (name == "Village")
            {
                GameObject newPos = GameObject.Find("PlayerPosVillage");
                controller.hasControl = false;
                controller.agent.destination = newPos.transform.position;
                while (controller.agent.transform.position.z != newPos.transform.position.z)
                    yield return null;
                sailor.tag = "talking";
                sailor.GetComponent<NpcManager>().isTalkable = true;
                StartCoroutine(qManager.SailorQuest(2));
            }
            else if (name == "End")
            {
                GameObject newPos = GameObject.Find("PlayerEndPos");
                controller.hasControl = false;
                controller.agent.destination = newPos.transform.position;
                while (controller.agent.transform.position.z != newPos.transform.position.z)
                    yield return null;
                sailor.tag = "talking";
                qManager.RunQuest(1);
            }
        }
    }
}
