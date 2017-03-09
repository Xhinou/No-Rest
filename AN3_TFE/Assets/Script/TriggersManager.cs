using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TriggersManager : MonoBehaviour
{
    public GameObject
        scriptSystem,
        player;
    QuestManager qManager;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    bool isEntering;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            isEntering = true;
            StartCoroutine(TriggerQuest());
        }
    }

    void OnTriggerExit(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
        {
            isEntering = false;
            StartCoroutine(TriggerQuest());
        }
    }

    public IEnumerator TriggerQuest()
    {
        switch (qManager.sceneID)
        {
            case 0:
                switch (gameObject.name)
                {
                    case "DoorTrigger":
                        GameObject door = GameObject.Find("Door");
                        Animator anim = door.GetComponent<Animator>();
                        bool doorTriggered = false;
                        if (isEntering)
                        {
                            doorTriggered = true;
                            anim.SetBool("doorTrig", doorTriggered);
                        }
                        else
                        {
                            doorTriggered = false;
                            anim.SetBool("doorTrig", doorTriggered);
                        }
                        break;
                    case "WhoTrigger":
                        GameObject newPos = GameObject.Find("WhoPos");
                        StartCoroutine(qManager.ObjectToPos(player, newPos));
                        while (qManager.isCoroutineRunning)
                            yield return null;
                        qManager.RunQuest(0);
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                break;
            case 2:
                NpcManager sailorNpc = qManager.sailor.GetComponent<NpcManager>();
                if (isEntering)
                {
                    GameObject newPos;
                    switch (gameObject.name)
                    {
                        case "SailorGoRight":
                            qManager.hasFollowedSailor = true;
                            qManager.sailorNav.enabled = false;
                            newPos = GameObject.Find("SailorPosA");
                            qManager.sailorTr.position = newPos.transform.position;
                            qManager.sailorNav.enabled = true;
                            sailorNpc.isTalkable = true;
                            qManager.sailor.SetActive(true);
                            qManager.triggers[2].SetActive(false);
                            break;
                        case "SailorGoLeft":
                            qManager.hasFollowedSailor = false;
                            qManager.sailorNav.enabled = false;
                            newPos = GameObject.Find("SailorPosB");
                            qManager.sailorTr.position = newPos.transform.position;
                            qManager.sailorNav.enabled = true;
                            sailorNpc.isTalkable = false;
                            qManager.sailor.SetActive(true);
                            qManager.triggers[2].SetActive(true);
                            break;
                        case "SailorInterrupt":
                            qManager.RunQuest(1);
                            break;
                        case "SailorBlock":
                            controller.agent.ResetPath();
                            controller.canSkipDial = false;
                            newPos = GameObject.Find("SailorBlockPos");
                            dialogSystem.DisplayText(qManager.sceneID, 1, 2, "Main Camera");
                            dialogSystem.ForceLine(0, 0, null);
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            dialogSystem.EndDialog();
                            controller.canSkipDial = true;
                            sailorNpc.isTalkable = true;
                            break;
                        case "ScrewThis":
                            GameObject greed = GameObject.Find("GreedSailor");
                            greed.GetComponent<NpcManager>().isTalkable = true;
                            controller.agent.ResetPath();
                            qManager.GreedQuest(0, 2);
                            break;
                        case "Village":
                            newPos = GameObject.Find("PlayerPosVillage");
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            qManager.RunQuest(1);
                            break;
                        case "End":
                            newPos = GameObject.Find("PlayerEndPos");
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            qManager.RunQuest(1);
                            break;
                        default:
                            Debug.Log("Can't find the trigger. Check for its name in the code");
                            break;
                    }
                }
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
        }
    }
}