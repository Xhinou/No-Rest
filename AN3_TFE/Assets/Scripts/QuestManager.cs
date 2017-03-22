using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class QuestManager : MonoBehaviour
{
    public ParticleSystem[] particles;
    public int sceneID;
    public AudioClip[] audioClips;
    public AudioSource theAudio;
    Animator playerAnimator;
    GameObject
        newPos,
        newPos2,
        cam;
    public GameObject[] triggers = new GameObject[7];
    [HideInInspector] public static int karma = 0;
    [HideInInspector] public bool
        hasFollowedSailor = true,
        intro,
        isCoroutineRunning;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    Camera mainCam;
    [HideInInspector] public GameObject scriptSystem, player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        if (karmaStep != 0)
            theAudio = GameObject.FindWithTag("Audio").GetComponent<AudioSource>();
        cam = GameObject.Find("Main Camera");
        mainCam = cam.GetComponent<Camera>();
        playerAnimator = player.GetComponent<Animator>();
        LoadNpc(sceneID);
        if (DialoguesSystem.language == null)
            DialoguesSystem.language = "EN_";
        if (sceneID != 0)
            RunIntro(sceneID);
        else
            player.SetActive(false);
    }

    public void RunIntro(int _sceneID)
    {
        switch (_sceneID)
        {
            case 0:
                StartCoroutine(Incarnation());
                break;
            case 1:
                break;
            case 2:
                playerAnimator.Play("Lied Down");
                RunQuest(1);
                break;
            case 3:
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
        }   
    }



    public void RunQuest(int npcID)
    {
        switch (sceneID)
        {
            case 0:
                StartCoroutine(karmaQuest(karmaStep));
                break;
            case 1:
                break;
            case 2:
                switch (npcID)
                {
                    case 0:
                        Debug.Log("NPC ID must not be less than 1");
                        break;
                    case 1:
                        StartCoroutine(SailorQuest(sailorStep, npcID));
                        break;
                    case 2:
                        GreedQuest(greedStep, npcID);
                        break;
                    case 4:
                        NativeQuest(0, npcID);
                        break;
                    case 5:
                        StartCoroutine(AssassinQuest(killerStep, npcID));
                        break;
                    case 6:
                        StartCoroutine(HarshQuest(harshStep, npcID));
                        break;
                    case 7:
                        Debug.Log("How the hell did you talk to that guy ?");
                        break;
                    case 8:
                        GoldDigging();
                        break;
                    default:
                        SideQuest(npcID);
                        break;
                }
                break;
            case 3:
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
        }
    }

    #region World 0
    //*------------------------------ WORLD 0 - LOBBY ------------------------------*//

    public static int karmaStep = 0;

    public IEnumerator karmaQuest(int step)
    {
        switch (step)
        {
            case 0:
                dialogSystem.DisplayText(sceneID, 0, step, "Main Camera");
                while (!dialogSystem.isDisabled)
                    yield return null;
                StartCoroutine(Desincarnation(2));
                karmaStep = 1;
                break;
            case 1:
                dialogSystem.DisplayText(sceneID, 0, step, "Main Camera");
                if (karma > 0) //Karma is GOOD
                {

                }
                else //Karma is BAD
                {

                }
                while (!dialogSystem.isDisabled)
                    yield return null;
                StartCoroutine(Desincarnation(1));
                break;
            case 2:
                dialogSystem.DisplayText(sceneID, 0, step, "Main Camera");
                if (karma > 0) //Karma is GOOD
                {

                }
                else //Karma is BAD
                {

                }
                break;
            default:
                break;
        }
    }
    #endregion World 0

    #region World 1
    //*------------------------------ WORLD 1 - THE KNIGHT ------------------------------*//
    #endregion World 1

    #region World 2
    //*------------------------------ WORLD 2 - THE CAPTAIN ------------------------------*//
    
    private int
        sailorStep = 0,
        greedStep = 0,
        killerStep = 0,
        harshStep = 0;
    private bool goldGet = false;    
    public IEnumerator SailorQuest(int step, int npcID)
    {
        switch (step) {
            case 0:
                intro = true;
                particles[0].Play();
                yield return new WaitForSeconds(2);
                playerAnimator.Play("Get Up");
                yield return new WaitForSeconds(6);
                //anim réveil
                sailorNav.destination = player.transform.position;
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.0");
                int w = 0;
                while (w == 0)
                {
                    if (controller.isPlayerTrigger)
                    {
                        sailorNav.ResetPath();
                        w++;
                    }
                    yield return null;
                }
                dialogSystem.ForceLine(1, null, null);
                controller.canSkipDial = true;
                while (!dialogSystem.isDisabled)
                    yield return null;
                newPos = GameObject.Find("IntroGoal");
                StartCoroutine(ObjectToPos(sailor, newPos));
                while (isCoroutineRunning)
                    yield return null;
                sailorNav.ResetPath();
                sailor.SetActive(false);
                controller.hasControl = true;
                intro = false;
                sailorStep = 1;
                break;
            case 1:
                if (hasFollowedSailor)
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1");
                    karma += 1;
                    dialogSystem.ForceLine(0, 1, null);
                    for (int i = 0; i < 3; i++)
                        triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1b");
                    karma -= 1;
                    dialogSystem.ForceLine(2, null, null);
                    while (!dialogSystem.isDisabled)
                        yield return null;
                    sailor.tag = "Untagged";
                    for (int i = 0; i < 3; i++)
                        triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                     newPos = GameObject.Find("SailorPosA");
                    sailorScript.isTalkable = false;
                    StartCoroutine(ObjectToPos(sailor, newPos));
                    while (isCoroutineRunning)
                        yield return null;
                    sailorScript.isTalkable = true;
                    hasFollowedSailor = true;
                }
                sailorStep = 2;
                break;
            case 2:
                if (controller.isHolding)
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1");
                    GameObject heldItem = GameObject.FindWithTag("held");
                    if (heldItem.name == "Stone")
                    {
                        karma -= 1;
                        dialogSystem.ForceLine(2, 0, null);
                        Destroy(heldItem);
                        controller.isHolding = false;
                    }
                    else if (heldItem.name == "Shell")
                    {
                        karma -= 1;
                        dialogSystem.ForceLine(3, 1, null);
                        Destroy(heldItem);
                        controller.isHolding = false;
                    }
                    else if (heldItem.name == "Charcoal")
                    {
                        karma += 1;
                        dialogSystem.ForceLine(5, null, null);
                        Destroy(heldItem);
                        controller.isHolding = false;
                    }
                    for (int i = 0; i < 4; i++)
                        triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                    while (!dialogSystem.isDisabled)
                        yield return null;
                    sailorScript.isTalkable = false;
                     newPos = GameObject.Find("SailorPosVillage");
                    StartCoroutine(ObjectToPos(sailor, newPos));
                    while (isCoroutineRunning)
                        yield return null;
                    sailorStep = 3;
                }
                else
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
                    dialogSystem.ForceLine(0, 1, null);
                }
                break;
            case 3:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2");
                if (greedStep == 0 && killerStep == 0 && harshStep == 0)
                {
                    dialogSystem.ForceLine(0, 2, null);
                    dialogSystem.SetToDial("2_1_3_10-2", 1, "0");
                }
                else
                {
                    dialogSystem.ForceLine(3, 2, 0);
                    dialogSystem.SetToDial("", 0, "");
                }
                triggers[5].GetComponent<BoxCollider>().isTrigger = false;
                while (!dialogSystem.isDisabled)
                    yield return null;
                newPos = GameObject.Find("ChiefEndPos");
                newPos2 = GameObject.Find("SailorEndPos");
                chiefScript.isTalkable = false;
                sailorScript.isTalkable = false;
                StartCoroutine(ObjectToPos(sailor, newPos));
                StartCoroutine(ObjectToPos(chief, newPos2));
                while (isCoroutineRunning)
                    yield return null;
                chiefScript.isTalkable = true;
                sailorScript.isTalkable = true;
                sailorStep = 4;
                break;
            case 4:
                karma += 1;
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3");
                if (killerStep == 2 || harshStep > 0)
                    dialogSystem.ForceLine(0, 4, null);
                else
                    dialogSystem.ForceLine(5, null, null);
                while (!dialogSystem.isDisabled)
                    yield return null;
                newPos = GameObject.Find("PlayerEndPos2");
                StartCoroutine(ObjectToPos(player, newPos));
                while (isCoroutineRunning)
                    yield return null;
                playerAnimator.Play("Lied Down");
                newPos = GameObject.Find("PlayerEndPos3");
                player.transform.position = newPos.transform.position;
                particles[1].Play();
                yield return new WaitForSeconds(2);
                triggers[6].GetComponent<BoxCollider>().isTrigger = false;
                if (karma >= 1)
                    Debug.Log("KARMA IS GOOD");
                else
                    Debug.Log("KARMA IS BAD");
                LoadWorld(0);
                break;
            case 5:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3");
                while (!dialogSystem.isDisabled)
                    yield return null;
                newPos = GameObject.Find("PlayerEndPos2");
                StartCoroutine(ObjectToPos(player, newPos));
                while (isCoroutineRunning)
                    yield return null;
                triggers[6].GetComponent<BoxCollider>().isTrigger = false;
                //END ANIMATION
                if (karma >= 1)
                    Debug.Log("KARMA IS GOOD");
                else
                    Debug.Log("KARMA IS BAD");
                LoadWorld(0);
                break;
            default:
                break;
        }
    }

    private int bored;

    public void GreedQuest(int step, int npcID)
    {
        if (step == 0)
        { 
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam2");
            for (int i = 0; i < 5; i++)
                triggers[i].GetComponent<BoxCollider>().isTrigger = false;
            if (sailorStep < 3)
            {
                sailorNav.enabled = false;
                newPos = GameObject.Find("SailorPosVillage");
                sailorTr.position = newPos.transform.position;
                sailorNav.enabled = true;
                sailorStep = 3;
            }
            bored = 0;
            karma -= 1;
            greedStep = 1;
        }        
        else if (step == 1)
        {         
            if (controller.isHolding)
            {
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam2");
                GameObject heldItem = GameObject.FindWithTag("held");
                if (heldItem.name == "Pickaxe")
                {
                    dialogSystem.ForceLine(4, null, null);
                    triggers[5].GetComponent<BoxCollider>().isTrigger = false;
                    if (killerStep != 2)
                    {
                        newPos = GameObject.Find("ChiefEndPos");
                        newPos2 = GameObject.Find("SailorEndPos");
                        chiefTr.position = newPos.transform.position;
                        sailorTr.position = newPos2.transform.position;
                        chiefScript.isTalkable = true;
                        sailorScript.isTalkable = true;
                    }
                    goldScript.isTalkable = true;
                    sailorStep = 4;
                    greedStep = 2;
                }
                else
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
                    if (bored < 2)
                        dialogSystem.ForceLine(bored, 0, null);
                    else
                        dialogSystem.ForceLine(bored, 1, null);
                    bored += 1;
                    if (bored > 2)
                        bored = 2;
                }
            }
            else
            {
                dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
                if (bored < 2) 
                    dialogSystem.ForceLine(bored, 0, null);
                else
                    dialogSystem.ForceLine(bored, 1, null);
                bored += 1;
                if (bored > 2)
                    bored = 2;
            }
        }
        else if (step == 2)
            dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
    }

    void GoldDigging()
    {
        if (controller.isHolding)
        {
            GameObject heldItem = GameObject.FindWithTag("held");
            if (heldItem.name == "Pickaxe")
            {
                goldGet = true;
                karma -= 1;
                goldScript.isTalkable = false;
                controller.hasControl = true;
            }
            else
                controller.hasControl = true;
        }
        else
            controller.hasControl = true;
    }

    IEnumerator AssassinQuest(int step, int npcID)
    {      
        if (step == 0)
        {
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam5");
            sailorStep = 5;
            triggers[5].GetComponent<BoxCollider>().isTrigger = false;
            sailorNav.enabled = false;
            chiefNav.enabled = false;
            newPos = GameObject.Find("SailorEndPos");
            newPos2 = GameObject.Find("ChiefEndPos");
            sailorTr.position = newPos.transform.position;
            chiefTr.position = newPos2.transform.position;
            sailorNav.enabled = true;
            chiefNav.enabled = true;
            while (dialogSystem.theText.enabled == true)
                yield return null;
            while (dialogSystem.theText.enabled == false)
                yield return null;
            if (dialogSystem.lastChoice == 1)
            {
                while (!dialogSystem.isDisabled)
                    yield return null;
                controller.hasControl = false;
                newPos = GameObject.Find("AssassinPos");
                StartCoroutine(ObjectToPos(killer, newPos));
                killer.SetActive(false);
                controller.hasControl = true;
                chief.SetActive(false);
                killerStep = 2;
                karma -= 1;
            }
            else
                killerStep = 1;
        }
        else if (step == 1)
        {
            if (controller.isHolding)
            {
                GameObject heldItem = GameObject.FindWithTag("held");
                if (heldItem.name == "Coconut")
                {
                    //THROW COCONUT
                    Destroy(heldItem);
                    // ASSASSIN FALLS
                    karma += 1;
                    controller.hasControl = false;
                    killerScript.isTalkable = false;
                    killerScript.lookPlayer = false;
                    killer.GetComponent<Animator>().Play("Lying Down");
                    yield return new WaitForSeconds(6);
                    //killer.SetActive(false);
                    controller.hasControl = true;
                    sailorStep = 4;
                    killerStep = 0;
                }
                else
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
            }
            else
                dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
        }
    }

    void NativeQuest(int step, int npcID)
    {
        dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
        if (sailorStep <= 3)
            dialogSystem.ForceLine(0, 1, null);
        else
            dialogSystem.ForceLine(2, null, null);
    }
    
    IEnumerator HarshQuest(int step, int npcID)
    {
        if (step == 0)
        {
            if (killerStep == 0)
                sailorStep = 4;
            else
                sailorStep = 5;
            sailorNav.enabled = false;
            chiefNav.enabled = false;
            newPos = GameObject.Find("SailorEndPos");
            newPos2 = GameObject.Find("ChiefEndPos");
            sailorTr.position = newPos.transform.position;
            chiefTr.position = newPos2.transform.position;
            sailorNav.enabled = true;
            chiefNav.enabled = true;
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam6");
            if (!goldGet)
                dialogSystem.ForceLine(0, null, 2);
            while (dialogSystem.theText.enabled == true)
                yield return null;
            while (dialogSystem.theText.enabled == false)
                yield return null;
            if (dialogSystem.lastChoice == 0)
            {
                harshScript.isTalkable = false;
                harshScript.lookPlayer = false;
            }
            else if (dialogSystem.lastChoice == 1)
                harshStep = 1;
            else if (dialogSystem.lastChoice == 2)
            {
                karma -= 1;
                harshStep = 2;
            }
        }
        else if (step >= 1)
            dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera");
    }

    void SideQuest(int npcID)
    {
        dialogSystem.DisplayText(sceneID, npcID, 0, "Main Camera");
    }
    
    #endregion World 2

    #region World 3
    //*------------------------------ WORLD 3 - THE OFFICER ------------------------------*//
    #endregion World 3

    #region NPCs Loading
    /*NPCs - WORLD 1*/
    #region World 2 NPCs
    /*NPCs - WORLD 2*/
    [HideInInspector] public GameObject
        sailor,
        chief,
        gold,
        killer,
        harsh;
    [HideInInspector] public Transform
        sailorTr,
        chiefTr,
        killerTr;
    [HideInInspector] public NavMeshAgent
        sailorNav,
        chiefNav,
        killerNav;
    [HideInInspector] public NpcManager
        sailorScript,
        chiefScript,
        goldScript,
        killerScript,
        harshScript;
    #endregion World 2 NPCs
    /*NPCs - WORLD 3*/

    void LoadNpc(int _sceneID)
    {
        if (_sceneID == 0)
        {
            //GameObject deus = GameObject.Find("Deus");
        }
        else if (_sceneID == 1)
        {

        }
        else if (_sceneID == 2)
        {
            sailor = GameObject.Find("Sailor");
            chief = GameObject.Find("Chief");
            gold = GameObject.Find("GoldSeam");
            killer = GameObject.Find("Assassin");
            harsh = GameObject.Find("HarshNative");
            sailorTr = sailor.transform;
            chiefTr = chief.transform;
            killerTr = killer.transform;
            sailorNav = sailor.GetComponent<NavMeshAgent>();
            chiefNav = chief.GetComponent<NavMeshAgent>();
            killerNav = killer.GetComponent<NavMeshAgent>();
            sailorScript = sailor.GetComponent<NpcManager>();
            chiefScript = chief.GetComponent<NpcManager>();
            goldScript = gold.GetComponent<NpcManager>();
            killerScript = killer.GetComponent<NpcManager>();
            harshScript = harsh.GetComponent<NpcManager>();
        }
        else if (_sceneID == 3)
        {

        }
        else
        {
            Debug.Log("Error in scene ID");
            return;
        }            
    }
    #endregion NPCs Loading

    #region Methods

    public IEnumerator ObjectToPos(GameObject movable, GameObject newPos)
    {
        isCoroutineRunning = true;
        if (movable == player)
            controller.hasControl = false;
        else
            movable.GetComponent<NpcManager>().isMoving = true;
        NavMeshAgent movableAgent = movable.GetComponent<NavMeshAgent>();
        movableAgent.destination = newPos.transform.position;
        while (movableAgent.pathPending)
            yield return null;
        float dist = movableAgent.remainingDistance;//Vector3.Distance(movable.transform.position, newPos.transform.position);
        while (dist > 0.2f)
        {
            dist = movableAgent.remainingDistance;//Vector3.Distance(movable.transform.position, newPos.transform.position);
            yield return null;
        }
        if (movable == player)
            controller.hasControl = true;
        else
            movable.GetComponent<NpcManager>().isMoving = false;
        isCoroutineRunning = false;
    }

    protected void LoadWorld(int worldToLoad)
    {
        if (theAudio != null)
        {
            theAudio.clip = audioClips[worldToLoad];
            theAudio.Play();
        }
        SceneManager.LoadScene(worldToLoad);
    }

    [Range(1,5)] public float smoothSpeed;
    [Range(25, 40)] public int maxView;
    bool inInterpolation;
    int numberOfZooms = 0;

    public IEnumerator CameraZoom(bool isZoomIn)
    {
        if (numberOfZooms < 2)
        {
            numberOfZooms++;
            while (inInterpolation)
                yield return null;
            inInterpolation = true;
            if (isZoomIn)
            {
                while (mainCam.fieldOfView > 14)
                {
                    mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 12, Time.deltaTime * smoothSpeed);
                    yield return null;
                }
            }
            else
            {
                while (mainCam.fieldOfView < maxView)
                {
                    mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, maxView + 2, Time.deltaTime * smoothSpeed);
                    yield return null;
                }
                //cam.transform.Rotate(cam.transform.right, 8f);
            }
            inInterpolation = false;
            numberOfZooms--;
            Debug.Log("End of Zoom");
        }
    }

    #region Lobby Only
    private IEnumerator Incarnation()
    {
        player.SetActive(false);
        particles[0].Play();
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        yield return new WaitForSeconds(1);
        controller.hasControl = true;
    }

    private IEnumerator Desincarnation(int worldToLoad)
    {
        particles[1].Play();
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        LoadWorld(worldToLoad);
    }
    #endregion Lobby Only

    #endregion Methods
}