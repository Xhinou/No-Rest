using UnityEngine;
using System.Collections;

public class TriggersManager : MonoBehaviour
{
    public GameObject
        scriptSystem,
        player;
    QuestManager qManager;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    CameraFollower camFollower;
    bool isEntering;
    [Header("Lobby Particles")]
    public GameObject lobbyParticle;
    public bool isRight;
    Animator jumpParticle;
    [Header("Houses World 2")]
    public GameObject houseW2;
    Animator houseW2Animator;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        camFollower = GameObject.Find("Main Camera").GetComponent<CameraFollower>();       
    }

    private void Start()
    {
        if (gameObject.name == "JumpTrigger")
        {
            jumpParticle = lobbyParticle.GetComponent<Animator>();
            if (isRight)
                jumpParticle.Play("IdleRight");
            else
                jumpParticle.Play("IdleLeft");
        }
        else if (gameObject.name == "HouseTrigger")
            houseW2Animator = houseW2.GetComponent<Animator>();
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
            #region Lobby
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
                        StartCoroutine(camFollower.CamRotation("Up"));
                        GameObject newPos = GameObject.Find("WhoPos");
                        StartCoroutine(qManager.ObjectToPos(player, newPos));
                        while (qManager.isCoroutineRunning)
                            yield return null;
                        qManager.RunQuest(1);
                        break;
                    case "JumpTrigger":
                        if (isEntering)
                        {
                            if (isRight)
                            {
                                jumpParticle.Play("JumpLeft");
                                isRight = !isRight;
                            }
                            else
                            {
                                jumpParticle.Play("JumpRight");
                                isRight = !isRight;
                            }
                        }
                        break;
                    default:
                        Debug.Log("Can't find the trigger. Check for its name in the code");
                        break;
                }
                break;
            #endregion Lobby

            #region World 1
            case 1:
                if (isEntering)
                {
                    GameObject newPos;
                    switch (gameObject.name)
                    {
                        case "BlockTrig":
                            controller.agent.ResetPath();
                            controller.hasControl = false;
                            controller.canSkipDial = false;
                            qManager.RunQuest(1);
                            break;
                        case "GateTrigger":
                            GetComponent<Collider>().enabled = false;
                            newPos = GameObject.Find("PlayerPosCastle");
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            controller.hasControl = false;
                            qManager.RunQuest(1);
                            break;
                        case "PotenceTrig":
                            GameObject.Find("Remparts").GetComponent<Animator>().Play("gridclose");
                            GetComponent<Collider>().enabled = false;
                            controller.agent.ResetPath();
                            newPos = GameObject.Find("PlayerPosPotence");
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            controller.hasControl = false;
                            qManager.RunQuest(1);
                            break;
                        case "OutCityTrig":
                            GameObject.Find("Remparts").GetComponent<Animator>().Play("grid2close");
                            GetComponent<Collider>().enabled = false;
                            break;
                        case "GraalTrig":
                            controller.agent.ResetPath();
                            controller.canSkipDial = false;
                            controller.hasControl = false;
                            qManager.squireStep = 10;
                            qManager.RunQuest(1);
                            break;
                        default:
                            break;
                    }
                } else
                {
                    if (gameObject.name == "ChurchTrig")
                        qManager.CameraZoom(true);
                }
                break;
            #endregion World 1

            #region World 2
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
                            dialogSystem.DisplayText(qManager.sceneID, 1, 2, "Main Camera", false);
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
                            GetComponent<Collider>().enabled = false;
                            newPos = GameObject.Find("PlayerEndPos");
                            StartCoroutine(qManager.ObjectToPos(player, newPos));
                            while (qManager.isCoroutineRunning)
                                yield return null;
                            qManager.RunQuest(1);
                            break;
                        case "HouseTrigger":
                            houseW2Animator.SetBool("isTrig", isEntering);
                            break;
                        default:
                            Debug.Log("Can't find the trigger. Check for its name in the code");
                            break;
                    }
                }
                else
                {
                    switch (gameObject.name)
                    {
                        case "HouseTrigger":
                            houseW2Animator.SetBool("isTrig", isEntering);
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
                #endregion World 2
        }
    }
}