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
                        StartCoroutine(qManager.CameraZoom(false));
                        GameObject newPos = GameObject.Find("WhoPos");
                        StartCoroutine(qManager.ObjectToPos(player, newPos));
                        while (qManager.isCoroutineRunning)
                            yield return null;
                        qManager.RunQuest(1);
                        break;
			case "W1Trigger":
				GameObject W1 = GameObject.Find("W1");
				Animator first = W1.GetComponent<Animator>();
				bool W1Triggered = false;
				if (isEntering)
				{
					W1Triggered = true;
					first.SetBool("doorTrig", W1Triggered);
				}
				else
				{
					W1Triggered = false;
					first.SetBool("doorTrig", W1Triggered);
				}
				break;

			case "W2Trigger":
				GameObject W2 = GameObject.Find("W2");
				Animator second = W2.GetComponent<Animator>();
				bool W2Triggered = false;
				if (isEntering)
				{
					W2Triggered = true;
					second.SetBool("doorTrig", W2Triggered);
				}
				else
				{
					W2Triggered = false;
					second.SetBool("doorTrig", W2Triggered);
				}
				break;

			case "W3Trigger":
				GameObject W3 = GameObject.Find("W3");
				Animator third = W3.GetComponent<Animator>();
				bool W3Triggered = false;
				if (isEntering)
				{
					W3Triggered = true;
					third.SetBool("doorTrig", W3Triggered);
				}
				else
				{
					W3Triggered = false;
					third.SetBool("doorTrig", W3Triggered);
				}
				break;

			case "W4Trigger":
				GameObject W4 = GameObject.Find("W4");
				Animator fourth = W4.GetComponent<Animator>();
				bool W4Triggered = false;
				if (isEntering)
				{
					W4Triggered = true;
					fourth.SetBool("doorTrig", W4Triggered);
				}
				else
				{
					W4Triggered = false;
					fourth.SetBool("doorTrig", W4Triggered);
				}
				break;

			case "W5Trigger":
				GameObject W5 = GameObject.Find("W5");
				Animator fifth = W5.GetComponent<Animator>();
				bool W5Triggered = false;
				if (isEntering)
				{
					W5Triggered = true;
					fifth.SetBool("doorTrig", W5Triggered);
				}
				else
				{
					W5Triggered = false;
					fifth.SetBool("doorTrig", W5Triggered);
				}
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