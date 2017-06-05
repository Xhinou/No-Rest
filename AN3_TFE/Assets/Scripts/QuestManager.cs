using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public ParticleSystem[] particles;
    public int sceneID;
    public AudioClip[] audioClips;
    public AudioSource theAudio;
    Animator playerAnimator, godAnimator;
    GameObject
        newPos,
        newPos2,
        cam;
    public GameObject[] triggers = new GameObject[7];
    [HideInInspector] public static int karma = -1;
    [HideInInspector] public bool
        hasFollowedSailor = true,
        intro,
        isCoroutineRunning;
    CharacterClickingController controller;
    public 
        DialoguesSystem dialogSystem;
    Camera mainCam;
    public GameObject
        scriptSystem,
        player;
    void Start()
    {
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        if (karmaStep != 0)
            theAudio = GameObject.FindWithTag("Audio").GetComponent<AudioSource>();
        cam = GameObject.Find("Main Camera");
        mainCam = cam.GetComponent<Camera>();
        mainCam.fieldOfView = 14;
        playerAnimator = player.GetComponent<Animator>();
        LoadNpc(sceneID);
        if (DialoguesSystem.language == null)
            DialoguesSystem.language = "EN_";
        if (sceneID != 0)
            RunIntro(sceneID);
        else
        {
            godAnimator = GameObject.Find("Divinity").GetComponent<Animator>();
            player.SetActive(false);
        }
    }
    int shot = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (Input.GetKeyDown(KeyCode.Print))
        {
            Application.CaptureScreenshot("Screenshots/Screenshot" + shot + ".png");
        }
     /*   else if (Input.GetKeyDown(KeyCode.F1))
            LoadWorld(1);
        else if (Input.GetKeyDown(KeyCode.F2))
            LoadWorld(2);
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            karma = 0;
            karmaStep = 0;
            LoadWorld(0);
        }*/
    }

    public void RunIntro(int _sceneID)
    {
        switch (_sceneID)
        {
            case 0:
                StartCoroutine(Incarnation());
                break;
            case 1:
                RunQuest(1);
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
                StartCoroutine(KarmaQuest(karmaStep));
                break;
            case 1:
                switch (npcID)
                {
                    case 0:
                        Debug.Log("NPC ID must not be less than 1");
                        break;
                    case 1:
                        StartCoroutine(SquireQuest(squireStep, npcID));
                        break;
                    case 2:
                        StartCoroutine(MerrynQuest(merrynStep, npcID));
                        break;
                    case 4:
                        if (squireStep == 1 || squireStep == 2)
                            dialogSystem.DisplayText(sceneID, npcID, squireStep-1, "Cam4.0", true);
                        break;
                    case 5:
                        if (squireStep == 1 || squireStep == 2)
                            dialogSystem.DisplayText(sceneID, npcID, squireStep-1, "Cam5.0", true);
                        break;
                    case 6:
                        if (squireStep == 1 || squireStep == 2)
                            dialogSystem.DisplayText(sceneID, npcID, squireStep-1, "Cam6.0", false);
                        break;
                    default:
                        SideQuest(npcID);
                        break;
                }
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

    public IEnumerator KarmaQuest(int step)
    {
        switch (step)
        {
            case 0:
                karma = -1;
                dialogSystem.DisplayText(sceneID, 0, step, "Main Camera", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                StartCoroutine(Desincarnation(2));
                karmaStep = 1;
                break;
            case 1:
                dialogSystem.DisplayText(sceneID, 0, step, "Main Camera", false);
               /* while (!dialogSystem.isNextDial)
                    yield return null;*/
                if (karma >= 1) //Karma is GOOD
                {
                    dialogSystem.ForceLine(1, 4, null);
                }
                else //Karma is BAD
                {
                    dialogSystem.ForceLine(6, null, null);
                }
                while (!dialogSystem.isDisabled)
                    yield return null;
                StartCoroutine(Desincarnation(1));
                karmaStep = 0; //to change
                break;
            case 2:
                dialogSystem.DisplayText(2, 0, step, "Main Camera", false);
                if (karma > 0) //Karma is GOOD
                {
                    dialogSystem.ForceLine(1, 4, null);
                }
                else //Karma is BAD
                {
                    dialogSystem.ForceLine(6, null, null);
                }
                while (!dialogSystem.isDisabled)
                    yield return null;
                StartCoroutine(Desincarnation(1));
                karmaStep = 0;
                break;
            default:
                break;
        }
    }
    #endregion World 0

    #region World 1
    //*------------------------------ WORLD 1 - THE KNIGHT ------------------------------*//
    private int
        squireStep = 0,
        merrynStep = 0;

    public IEnumerator SquireQuest(int step, int npcID)
    {
        switch (step)
        {
            case 0:
                intro = true;
                particles[0].Play();
                yield return new WaitForSeconds(2.5f);
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.0", false);
                while (dialogSystem.theText.enabled == true)
                    yield return null;
                while (dialogSystem.theText.enabled == false)
                    yield return null;
                while (dialogSystem.theText.enabled == true)
                    yield return null;
                GameObject paper = GameObject.Find("Paper");
                paper.GetComponent<AudioSource>().Play();
                paper.GetComponent<Image>().enabled = true;
                while (dialogSystem.theText.enabled == false)
                    yield return null;
                paper.SetActive(false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                controller.hasControl = true;
                intro = false;
                arthurScript.isTalkable = true;
                squireStep = 1;
                break;
            case 1:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.0", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                if (dialogSystem.lastChoice == 1)
                {
                    newPos = GameObject.Find("MidePos");
                    StartCoroutine(ObjectToPos(arthur, newPos));
                    squireStep = 2;
                }
                break;
            case 2:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                if (dialogSystem.lastChoice == 1)
                {
                    controller.hasControl = false;
                    GameObject.Find("Remparts").GetComponent<Animator>().Play("grid");
                    newPos = GameObject.Find("RunAwayPos");
                    StartCoroutine(ObjectToPos(arthur, newPos));
                    //yield return new WaitForSeconds(2.5f);
                    while (isCoroutineRunning)
                        yield return null;
                    controller.hasControl = true;
                    newPos = GameObject.Find("ArthurWaitPos");
                    arthurNav.ResetPath();
                    arthurNav.enabled = false;
                    arthur.transform.position = newPos.transform.position;
                    triggers[1].GetComponent<Collider>().enabled = true;
                    squireStep = 4;
                }
                else
                {
                    controller.hasControl = false;
                    arthurNav.speed = 9f;
                    newPos = GameObject.Find("RunAwayPos");
                    StartCoroutine(ObjectToPos(arthur, newPos));
                    while (isCoroutineRunning)
                        yield return null;
                    arthurNav.ResetPath();
                    arthurNav.enabled = false;
                    newPos = GameObject.Find("ArthurWaitPos");                   
                    arthur.transform.position = newPos.transform.position;
                    arthurNav.speed = 4.5f;
                    squireStep = 3;
                    RunQuest(1);
                }
                break;
            case 3:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                GameObject.Find("Remparts").GetComponent<Animator>().Play("grid");
                newPos = GameObject.Find("GuardPosPotence");
                StartCoroutine(ObjectToPos(GameObject.Find("Guard"), newPos));
                newPos = GameObject.Find("PlayerPosPotence");
                StartCoroutine(ObjectToPos(player, newPos));
                while (isCoroutineRunning)
                    yield return null;
                while (isCoroutineRunning)
                    yield return null;
                controller.hasControl = false;
                squireStep = 4;
                RunQuest(1);
                break;
            case 4:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                newPos = GameObject.Find("ArthurWaitPos2");
                arthur.transform.position = newPos.transform.position;
                arthurNav.enabled = true;
                arthurNav.speed = 9f;
                newPos = GameObject.Find("ArthurKillPos");
                StartCoroutine(ObjectToPos(arthur, newPos));              
                GameObject[] leftNpc = GameObject.FindGameObjectsWithTag("npcLeft");
                newPos = GameObject.Find("NPCrunLeft");
                foreach (GameObject g in leftNpc)
                    StartCoroutine(ObjectToPos(g, newPos));
                GameObject[] rightNpc = GameObject.FindGameObjectsWithTag("npcRight");
                newPos = GameObject.Find("NPCrunRight");
                foreach (GameObject g in rightNpc)
                    StartCoroutine(ObjectToPos(g, newPos));
                while (isCoroutineRunning)
                    yield return null;
                arthurNav.speed = 4.5f;
                squireStep = 5;
                RunQuest(1);
                break;
            case 5:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                controller.hasControl = false;
                merrynScript.isTalkable = true;
                king.GetComponent<Animator>().Play("Lying Down");
                newPos = GameObject.Find("ArthurWaitPos2");
                StartCoroutine(ObjectToPos(arthur, newPos));
                while (isCoroutineRunning)
                    yield return null;
                arthurNav.ResetPath();
                yield return null;
                arthurNav.enabled = false;
                newPos = GameObject.Find("ArthurChurchPos");
                arthur.transform.position = newPos.transform.position;
                arthurNav.enabled = true;
                controller.hasControl = true;
                squireStep = 6;
                break;
            default:
                break;
        }
    }

    public IEnumerator MerrynQuest (int step, int npcID)
    {
        switch (step)
        {
            case 0:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                if (dialogSystem.lastChoice == 0)
                {
                    newPos = GameObject.Find("MerrynChurchPos");
                    merryn.transform.position = newPos.transform.position;
                    merryn.GetComponent<NavMeshAgent>().enabled = true;
                }
                else
                {
                    merrynScript.isTalkable = false;
                    merrynScript.lookPlayer = false;
                }
                GameObject.Find("Remparts").GetComponent<Animator>().Play("grid2");
                merrynStep = 1;
                break;
            case 1:
                break;
        }
        yield return null;
    }

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
                yield return new WaitForSeconds(2.5f);
                playerAnimator.Play("Get Up");
                yield return new WaitForSeconds(4f);
                //anim réveil
                sailorNav.destination = player.transform.position;
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.0", false);
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
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1", false);
                    karma += 1;
                    dialogSystem.ForceLine(0, 1, null);
                    for (int i = 0; i < 3; i++)
                        triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                }
                else
                {
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1b", false);
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
                    dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.1", false);
                    GameObject heldItem = GameObject.FindWithTag("held");
                    if (heldItem.name == "Stone")
                    {
                        karma -= 1;
                        dialogSystem.ForceLine(2, 0, null);
                    }
                    else if (heldItem.name == "Shell")
                    {
                        karma -= 1;
                        dialogSystem.ForceLine(3, 1, null);
                    }
                    else if (heldItem.name == "Charcoal")
                    {
                        karma += 1;
                        dialogSystem.ForceLine(5, null, null);
                    }
                    Destroy(heldItem);
                    controller.isHolding = false;
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
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
                    dialogSystem.ForceLine(0, 1, null);
                }
                break;
            case 3:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2", false);
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
                for (int i = 5; i < 9; i++)
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
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
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3", false);
                if (killerStep == 2 || harshStep > 0)
                    dialogSystem.ForceLine(0, 4, null);
                else
                    dialogSystem.ForceLine(5, null, null);
                while (!dialogSystem.isDisabled)
                    yield return null;
                controller.hasControl = false;
                newPos = GameObject.Find("PlayerEndPos2");
                StartCoroutine(ObjectToPos(player, newPos));
                while (isCoroutineRunning)
                    yield return null;
                playerAnimator.Play("Lied Down");
                newPos = GameObject.Find("PlayerEndPos3");
                controller.agent.enabled = false;
                player.transform.position = newPos.transform.position;
                GameObject.Find("Sword").GetComponent<Animator>().Play("Damocles");
                yield return new WaitForSeconds(1.5f);
                particles[1].Play();
                yield return new WaitForSeconds(2.5f);
                triggers[9].GetComponent<BoxCollider>().isTrigger = false;
                if (karma >= 1)
                    Debug.Log("KARMA IS GOOD");
                else
                    Debug.Log("KARMA IS BAD");
                LoadWorld(0);
                break;
            case 5:
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3", false);
                while (!dialogSystem.isDisabled)
                    yield return null;
                controller.hasControl = false;
                newPos = GameObject.Find("PlayerEndPos2");
                StartCoroutine(ObjectToPos(player, newPos));
                while (isCoroutineRunning)
                    yield return null;
                playerAnimator.Play("Lied Down");
                newPos = GameObject.Find("PlayerEndPos3");
                controller.agent.enabled = false;
                player.transform.position = newPos.transform.position;
                GameObject.Find("Sword").GetComponent<Animator>().Play("Damocles");
                yield return new WaitForSeconds(1.5f);
                particles[1].Play();
                yield return new WaitForSeconds(2.5f);
                triggers[9].GetComponent<BoxCollider>().isTrigger = false;
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
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam2", false);
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
                dialogSystem.DisplayText(sceneID, npcID, step, "Cam2", false);
                GameObject heldItem = GameObject.FindWithTag("held");
                if (heldItem.name == "Pickaxe")
                {
                    dialogSystem.ForceLine(4, null, null);
                    for (int i = 5; i < 9; i++)
                        triggers[i].GetComponent<BoxCollider>().isTrigger = false;
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
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
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
                dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
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
            dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
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
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam5", false);
            sailorStep = 5;
            for (int i = 5; i < 9; i++)
                triggers[i].GetComponent<BoxCollider>().isTrigger = false;
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
                while (isCoroutineRunning)
                    yield return null;
                killer.SetActive(false);
                controller.hasControl = true;
                chief.SetActive(false);
                killerStep = 2;
                karma -= 2;
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
                    controller.isHolding = false;
                    // ASSASSIN FALLS
                    karma += 1;
                    controller.hasControl = false;
                    killerScript.isTalkable = false;
                    killerScript.lookPlayer = false;
                    killer.GetComponent<Animator>().Play("Lying Down");
                    yield return new WaitForSeconds(6);
                    //killer.SetActive(false);
                    controller.hasControl = true;
                    sailorStep = 5;
                    killerStep = 0;
                }
                else
                    dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
            }
            else
                dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
        }
    }

    void NativeQuest(int step, int npcID)
    {
        dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
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
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam6", false);
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
            dialogSystem.DisplayText(sceneID, npcID, step, "Main Camera", false);
    }

    #endregion World 2

    #region World 3
    //*------------------------------ WORLD 3 - THE OFFICER ------------------------------*//
    #endregion World 3

    void SideQuest(int npcID)
    {
        dialogSystem.DisplayText(sceneID, npcID, 0, "Main Camera", false);
    }

    #region NPCs Loading
    #region World 1 NPCs
    /*NPCs - WORLD 1*/
    [HideInInspector] public GameObject
        arthur,
        merryn,
        slaught,
        king;
    [HideInInspector] public NpcManager
       arthurScript,
       merrynScript,
       slaughtScript,
       kingScript;
    [HideInInspector]
    public NavMeshAgent
        arthurNav;
    #endregion World 1 NPCs
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

    void LoadNpc(int _sceneID)
    {
        switch (_sceneID)
        {
            case 1:
                arthur = GameObject.Find("Arthur");
                merryn = GameObject.Find("Merryn");
                slaught = GameObject.Find("Slaughterman");
                king = GameObject.Find("King");
                arthurScript = arthur.GetComponent<NpcManager>();
                merrynScript = merryn.GetComponent<NpcManager>();
                slaughtScript = slaught.GetComponent<NpcManager>();
                kingScript = king.GetComponent<NpcManager>();
                arthurNav = arthur.GetComponent<NavMeshAgent>();
                break;
            case 2:
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
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
        }           
    }
    #endregion NPCs Loading

    #region Methods

    bool anotherCR = false;
    public IEnumerator ObjectToPos(GameObject movable, GameObject newPos)
    {
        if (isCoroutineRunning)
            anotherCR = true;
        isCoroutineRunning = true;
        if (movable == player)
            controller.hasControl = false;
        else
        {
            movable.GetComponent<NpcManager>().isMoving = true;
            movable.GetComponent<NpcManager>().isTalkable = false;
        }
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
        else if (movable.tag == "npcLeft" || movable.tag == "npcRight")
            Destroy(movable);
        else
        {
            movable.GetComponent<NpcManager>().isMoving = false;
            movable.GetComponent<NpcManager>().isTalkable = true;
        }
        if (!anotherCR)
            isCoroutineRunning = false;
        else
            anotherCR = false;
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
        godAnimator.Play("Finger");
        particles[1].Play();
        player.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        LoadWorld(worldToLoad);
    }
    #endregion Lobby Only

    #endregion Methods
}