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
        #region World 0
        if (qManager.sceneID == 0)
        {
            if (name == "DoorTrigger")
            {
                GameObject door = GameObject.Find("Door");
                Animator anim = door.GetComponent<Animator>();
                bool doorTriggered = false;
                if (isEntering)
                {
                    doorTriggered = true;
                    anim.SetBool("doorTrig", doorTriggered);
                } else
                {
                    doorTriggered = false;
                    anim.SetBool("doorTrig", doorTriggered);
                }
            }
            if (name == "WhoTrigger")
            {
               // GameObject mainCam = GameObject.Find("Main Camera");
               // GameObject cam2 = GameObject.Find("WhoCamera");
                if (isEntering)
                {
                    //Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 280, Time.deltaTime * 2f);
                    //cam2.GetComponent<Camera>().enabled = true;
                } else
                {
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, -280, Time.deltaTime * 2f);
                    //cam2.GetComponent<Camera>().enabled = false;
                }
                //qManager.karmaQuest(0);
            }
        }
        #endregion World 0
        
        #region World 2
        if (qManager.sceneID == 2)
        {
            NpcManager sailorNpc = qManager.sailor.GetComponent<NpcManager>();
            if (isEntering)
            {
                if (name == "SailorGoRight")
                {
                    qManager.hasFollowedSailor = true;
                    qManager.sailorNav.enabled = false;
                    GameObject newPos = GameObject.Find("SailorPosA");
                    qManager.sailorTr.position = newPos.transform.position;
                    qManager.sailorNav.enabled = true;
                    sailorNpc.isTalkable = true;
                    qManager.sailor.SetActive(true);
                }
                else if (name == "SailorGoLeft")
                {
                    qManager.hasFollowedSailor = false;
                    qManager.sailorNav.enabled = false;
                    GameObject newPos = GameObject.Find("SailorPosB");
                    qManager.sailorTr.position = newPos.transform.position;
                    qManager.sailorNav.enabled = true;
                    sailorNpc.isTalkable = true;
                    qManager.sailor.SetActive(true);
                }
                else if (name == "SailorInterrupt")
                {
                    qManager.RunQuest(1);
                }
                else if (name == "SailorBlock")
                {
                    controller.agent.ResetPath();
                    controller.canSkipDial = false;
                    GameObject newPos = GameObject.Find("SailorBlockPos");
                    dialogSystem.DisplayText(qManager.sceneID, 1, 2, "Main Camera");
                    dialogSystem.ForceLine(0, 0, null);
                    StartCoroutine(qManager.ObjectToPos(player, newPos));
                    while (qManager.isCoroutineRunning)
                        yield return null;
                    /*controller.agent.destination = newPos.transform.position;
                    while (controller.agent.transform.position.z != newPos.transform.position.z)
                        yield return null;*/
                    dialogSystem.EndDialog();
                    controller.canSkipDial = true;
                    sailorNpc.isTalkable = true;
                }
                else if (name == "ScrewThis")
                {
                    GameObject greed = GameObject.Find("GreedSailor");
                    greed.GetComponent<NpcManager>().isTalkable = true;
                    controller.agent.ResetPath();
                    qManager.GreedQuest(0, 2);
                }
                else if (name == "Village")
                {
                    GameObject newPos = GameObject.Find("PlayerPosVillage");
                    /*controller.hasControl = false;
                    controller.agent.destination = newPos.transform.position;*/
                    StartCoroutine(qManager.ObjectToPos(player, newPos));
                    while (qManager.isCoroutineRunning)
                        yield return null;
                   /* float dist = Vector3.Distance(controller.agent.transform.position, newPos.transform.position);
                    while (dist > 0.8f)
                    {
                        dist = Vector3.Distance(controller.agent.transform.position, newPos.transform.position);
                        yield return null;
                    }*/
                    /*while (player.transform.position.z != newPos.transform.position.z)
                        yield return null;*/
                    qManager.RunQuest(1);
                }
                else if (name == "End")
                {
                    GameObject newPos = GameObject.Find("PlayerEndPos");
                    StartCoroutine(qManager.ObjectToPos(player, newPos));
                    while (qManager.isCoroutineRunning)
                        yield return null;
                    /*controller.agent.destination = newPos.transform.position;
                    while (controller.agent.transform.position.z != newPos.transform.position.z)
                        yield return null;*/
                    qManager.RunQuest(1);
                }
            }
        }
        #endregion World 2
    }
}