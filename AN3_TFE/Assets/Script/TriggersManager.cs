using UnityEngine;
using System.Collections;

public class TriggersManager : MonoBehaviour
{

    public GameObject scriptSystem;
    QuestManager qManager;
    CharacterClickingController controller;
    public GameObject player;
    bool isEntering;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
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
                    StartCoroutine(pute());
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
            GameObject sailor = GameObject.Find("SailorBody");
            if (isEntering)
            {
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
        #endregion World 2
    }

    private IEnumerator pute()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 280, Time.deltaTime * 2f);
        yield return null;
    }
}