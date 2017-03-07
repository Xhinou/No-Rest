using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class QuestManager : MonoBehaviour
{
    public int sceneID;
    GameObject
        npc,
        player;
    public GameObject[] triggers = new GameObject[7];
    [HideInInspector] public int karma = 0;
    [HideInInspector] public bool
        hasFollowedSailor = true,
        intro,
        isCoroutineRunning;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    [HideInInspector] public GameObject scriptSystem;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        LoadNpc(sceneID);
        RunIntro(sceneID);
    }

    void RunIntro(int _sceneID)
    {
        switch (_sceneID)
        {
            case 0:
                controller.hasControl = true;
                break;
            case 1:
                break;
            case 2:
                RunQuest(1);
                break;
            case 3:
                break;
            default:
                Debug.Log("Error in scene ID");
                break;
        }
        /*if (_sceneID == 0)
            controller.hasControl = true;
        else if (_sceneID == 1) return;
        else if (_sceneID == 2)
            RunQuest(1);
        else if (_sceneID == 3)return;
        else
        {
            Debug.Log("Error in scene ID");
            return;
        }     */      
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
        
        /*if (sceneID == 2)
        {
            if (npcID == 1)
                StartCoroutine(SailorQuest(sailorStep, npcID));
            else if (npcID == 2)
                GreedQuest(greedStep, npcID);
            else if (npcID == 4)
                NativeQuest(0, npcID);
            else if (npcID == 5)
                StartCoroutine(AssassinQuest(killerStep, npcID));
            else if (npcID == 6)
                StartCoroutine(HarshQuest(harshStep, npcID));
            else if (npcID == 8)
                GoldDigging();
            else if (npcID >= 9)
                SideQuest(npcID);
            else
            {
                Debug.Log("NPC ID must not be less than 1");
                return;
            }                
        }
        else
            return;*/
    }

    #region World 0
    //*------------------------------ WORLD 0 - LOBBY ------------------------------*//

    private int karmaStep = 0;

    public IEnumerator karmaQuest(int step)
    {
        if (karmaStep == 0)
        {
            dialogSystem.DisplayText(sceneID, 0, step, "Main Camera");
            while (!dialogSystem.isDisabled)
                yield return null;
            SceneManager.LoadScene(2);
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
        GameObject newPos;
        switch (step) {
            case 0:
                intro = true;
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
                /*sailorNav.destination = introGoal.transform.position;
                while (sailorTr.position.z >= introGoal.transform.position.z + 0.03f)
                    yield return null;*/
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
                    /*sailorNav.destination = newPos.transform.position;
                    while (sailorTr.position.z != newPos.transform.position.z)
                        yield return null;*/
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
                    /*sailorNav.destination = newPos.transform.position;
                    while (sailorTr.position.z != newPos.transform.position.z)
                        yield return null;*/
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
                GameObject newPos2 = GameObject.Find("SailorEndPos");
                chiefScript.isTalkable = false;
                sailorScript.isTalkable = false;
                StartCoroutine(ObjectToPos(sailor, newPos));
                StartCoroutine(ObjectToPos(chief, newPos2));
                while (isCoroutineRunning)
                    yield return null;
                /*chiefNav.destination = newPos.transform.position;
                sailorNav.destination = newPos2.transform.position;
                while (chiefTr.position.z != newPos.transform.position.z && sailorTr.position.z != newPos2.transform.position.z)
                    yield return null;*/
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
                triggers[6].GetComponent<BoxCollider>().isTrigger = false;
                if (karma >= 1)
                    Debug.Log("KARMA IS GOOD");
                else
                    Debug.Log("KARMA IS BAD");
                SceneManager.LoadScene(0);
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
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
        /*if (step == 0)
        {
            intro = true;
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
            GameObject newPos = GameObject.Find("IntroGoal");
            StartCoroutine(ObjectToPos(sailor, newPos));
            while (isCoroutineRunning)
                yield return null;
            sailorNav.ResetPath();
            sailor.SetActive(false);
            controller.hasControl = true;
            intro = false;
            sailorStep = 1;
        }
        else if (step == 1)
        {
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
                GameObject newPos = GameObject.Find("SailorPosA");
                sailorScript.isTalkable = false;
                StartCoroutine(ObjectToPos(sailor, newPos));
                while (isCoroutineRunning)
                    yield return null;
                sailorScript.isTalkable = true;
                hasFollowedSailor = true;
            }
            sailorStep = 2;
        }
        else if (step == 2)
        {
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
                GameObject newPos = GameObject.Find("SailorPosVillage");
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
        }
        else if (step == 3)
        {
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.2");
            if (greedStep == 0 && killerStep == 0 && harshStep == 0)
            {
                dialogSystem.ForceLine(0, 2, null);
                dialogSystem.SetToDial("2_1_3_10-2");
            }
            else
            {
                dialogSystem.ForceLine(3, 2, 0);
                dialogSystem.SetToDial("");
            }
            triggers[5].GetComponent<BoxCollider>().isTrigger = false;
            while (!dialogSystem.isDisabled)
                yield return null;
            GameObject newPos = GameObject.Find("ChiefEndPos");
            GameObject newPos2 = GameObject.Find("SailorEndPos");
            chiefScript.isTalkable = false;
            sailorScript.isTalkable = false;
            StartCoroutine(ObjectToPos(sailor, newPos));
            StartCoroutine(ObjectToPos(chief, newPos));
            while (isCoroutineRunning)
                yield return null;
            chiefScript.isTalkable = true;
            sailorScript.isTalkable = true;
            sailorStep = 4;
        }
        else if (step == 4)
        {
            karma += 1;
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3");
            if (killerStep == 2 || harshStep > 0)
                dialogSystem.ForceLine(0, 4, null);
            else
                dialogSystem.ForceLine(5, null, null);
            while (!dialogSystem.isDisabled)
                yield return null;
            GameObject newPos = GameObject.Find("PlayerEndPos2");
            //controller.agent.destination = newPos.transform.position;
            triggers[6].GetComponent<BoxCollider>().isTrigger = false;
            controller.hasControl = false;
            StartCoroutine(ObjectToPos(player, newPos));
            //END ANIMATION
            if (karma >= 1)
            {
                Debug.Log("KARMA IS GOOD");
            }
            else
            {
                Debug.Log("KARMA IS BAD");
            }
            SceneManager.LoadScene(0);
        }
        else if (step == 5)
        {
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam1.3");
            GameObject newPos = GameObject.Find("PlayerEndPos2");
            controller.agent.destination = newPos.transform.position;
            triggers[6].GetComponent<BoxCollider>().isTrigger = false;
            controller.hasControl = false;
            while (player.transform.position.z != newPos.transform.position.z)
                yield return null;
            //END ANIMATION
            if (karma >= 1)
            {
                Debug.Log("KARMA IS GOOD");
            }
            else
            {
                Debug.Log("KARMA IS BAD");
            }
            SceneManager.LoadScene(0);
        }*/
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
                GameObject newPos = GameObject.Find("SailorPosVillage");
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
                        GameObject newPos = GameObject.Find("ChiefEndPos");
                        GameObject newPos2 = GameObject.Find("SailorEndPos");
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
            GameObject newPos = GameObject.Find("SailorEndPos");
            GameObject newPos2 = GameObject.Find("ChiefEndPos");
            sailorTr.position = newPos.transform.position;
            chiefTr.position = newPos2.transform.position;
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
                /* killerNav.destination = newPos.transform.position;
                 while (killerTr.position.z != newPos.transform.position.z)
                     yield return null;*/
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
                    killer.SetActive(false);
                    killerScript.isTalkable = false;
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
            GameObject newPos = GameObject.Find("SailorEndPos");
            GameObject newPos2 = GameObject.Find("ChiefEndPos");
            sailorTr.position = newPos.transform.position;
            chiefTr.position = newPos2.transform.position;
            dialogSystem.DisplayText(sceneID, npcID, step, "Cam6");
            if (!goldGet)
                dialogSystem.ForceLine(0, 0, 2);
            while (dialogSystem.theText.enabled == true)
                yield return null;
            while (dialogSystem.theText.enabled == false)
                yield return null;
            if (dialogSystem.lastChoice == 0)
                npc.GetComponent<NpcManager>().isTalkable = false;
            else if (dialogSystem.lastChoice == 2)
                harshStep = 1;
            else if (dialogSystem.lastChoice == 3)
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
        killer;
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
        killerScript;
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
}