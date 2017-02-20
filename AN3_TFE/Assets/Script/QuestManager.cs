using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class QuestManager : MonoBehaviour
{
    public int
        sceneID,
        introStep = 0,
        introEndStep;
    public GameObject introGoal;
    
    GameObject
        npc,
        player;
    public GameObject[] triggers = new GameObject[7];
    [HideInInspector] public int karma = 0;
    [HideInInspector]
    public bool
        hasFollowedSailor = true,
        intro;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    public GameObject scriptSystem;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        RunIntro();
        if (sceneID == 2)
            introEndStep = 3;
    }

    void Update()
    {
        //Debug.Log("Karma value is: " + karma);
        //Debug.Log(sailorStep);
    }

    void RunIntro()
    {
        if (sceneID == 0)
        {
            //ANIM EMBODY "SPIRIT FORM"
            controller.hasControl = true;
        }
        else if (sceneID == 1)
        {

        }
        else if (sceneID == 2)
        {
            RunQuest(1);
        }
        else if (sceneID == 3)
        {

        }
    }

    public void RunQuest(int npcID)
    {
        //npc = GameObject.FindWithTag("talking");
        if (sceneID == 2)
        {
            if (npcID == 1)
            {
                StartCoroutine(SailorQuest(sailorStep, npcID));
            }
            else if (npcID == 2)
            {
                GreedQuest(greedStep);
            }
            else if (npcID == 4)
            {
                NativeQuest(0);
            }
            else if (npcID == 5)
            {
                StartCoroutine(AssassinQuest(killerStep));
            }
            else if (npcID == 6)
            {
                StartCoroutine(HarshQuest(harshStep));
            }
            else if (npcID == 8)
            {
                SideQuest();
            }
            else if (npcID == 10)
            {
                GoldDigging();
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    #region World 0
    //*------------------------------ WORLD 0 - LOBBY ------------------------------*//

    int karmaStep = 0;

    public void karmaQuest(int step)
    {
        if (karmaStep == 0)
        {
            //TALK WITH THE ENTITY BETWEEN THE WORLDS
        }
    }
    #endregion World 0

    #region World 1
    //*------------------------------ WORLD 1 - THE KNIGHT ------------------------------*//
    #endregion World 1

    #region World 2
    //*------------------------------ WORLD 2 - THE CAPTAIN ------------------------------*//

    int
        sailorStep = 0,
        greedStep = 0,
        killerStep = 0,
        harshStep = 0;
    bool goldGet = false;

    public IEnumerator SailorQuest(int step, int npcID)
    {
        npc = GameObject.FindWithTag("talking");
        GameObject sailor = GameObject.Find("SailorBody");
        if (step == 0)
        {
            intro = true;
            //anim réveil			
            sailor.GetComponent<NavMeshAgent>().destination = player.transform.position;
            dialogSystem.DisplayText(sceneID, npcID, step);
            int w = 0;
            while (w == 0)
            {
                if (controller.isPlayerTrigger)
                {
                    sailor.GetComponent<NavMeshAgent>().ResetPath();
                    w++;
                }
                yield return null;
            }
            dialogSystem.ForceLine(1, null, null);
            controller.canSkipDial = true;
            while (!dialogSystem.isDisabled)
                yield return null;
            sailor.GetComponent<NavMeshAgent>().destination = introGoal.transform.position;
            while (sailor.transform.position.z >= introGoal.transform.position.z + 0.03f)
                yield return null;
            sailor.GetComponent<NavMeshAgent>().ResetPath();
            sailor.GetComponent<NpcManager>().HideObject();
            controller.hasControl = true;
            intro = false;
            sailorStep = 1;
        }
        else if (step == 1)
        {
            dialogSystem.DisplayText(sceneID, npcID, step);
            if (hasFollowedSailor)
            {
                karma += 1;
                dialogSystem.ForceLine(0, 1, null);
                for (int i = 0; i < 3; i++)
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
            }
            else
            {
                dialogSystem.dialCam.enabled = false;
                dialogSystem.dialCam = GameObject.Find("NPC1Cam1.5").GetComponent<Camera>();
                dialogSystem.dialCam.enabled = true;
                karma -= 1;
                dialogSystem.ForceLine(2, null, null);
                while (!dialogSystem.isDisabled)
                    yield return null;
                npc.tag = "Untagged";
                for (int i = 0; i < 3; i++)
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                GameObject newPos = GameObject.Find("SailorPosA");
                npc.GetComponent<NavMeshAgent>().destination = newPos.transform.position;
                npc.GetComponent<NpcManager>().isTalkable = false;
                while (npc.transform.position.z != newPos.transform.position.z)
                    yield return null;
                npc.GetComponent<NpcManager>().isTalkable = true;
                hasFollowedSailor = true;
            }
            sailorStep = 2;
        }
        else if (step == 2)
        {
            dialogSystem.DisplayText(sceneID, npcID, step);
            if (controller.isHolding)
            {
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
                npc.GetComponent<NpcManager>().isTalkable = false;
                GameObject newPos = GameObject.Find("SailorPosVillage");
                npc.GetComponent<NavMeshAgent>().destination = newPos.transform.position;
                while (npc.transform.position.z != newPos.transform.position.z)
                    yield return null;
                sailorStep = 3;
            }
            else
                dialogSystem.ForceLine(0, 1, null);
        }
        else if (step == 3)
        {
            GameObject chief = GameObject.Find("ChiefBody");
            npc.tag = "Untagged";
            dialogSystem.DisplayText(sceneID, npcID, step);
            if (greedStep == 0)
                dialogSystem.ForceLine(0, 3, null);
            else
                dialogSystem.ForceLine(4, 2, 0);
            triggers[5].GetComponent<BoxCollider>().isTrigger = false;
            GameObject newPos = GameObject.Find("ChiefEndPos");
            GameObject newPos2 = GameObject.Find("SailorEndPos");
            chief.GetComponent<NpcManager>().isTalkable = false;
            npc.GetComponent<NpcManager>().isTalkable = false;
            chief.GetComponent<NavMeshAgent>().destination = newPos.transform.position;
            npc.GetComponent<NavMeshAgent>().destination = newPos2.transform.position;
            while (chief.transform.position.z != newPos.transform.position.z && npc.transform.position.z != newPos2.transform.position.z)
                yield return null;
            chief.GetComponent<NpcManager>().isTalkable = true;
            npc.GetComponent<NpcManager>().isTalkable = true;
            sailorStep = 4;
        }
        else if (step == 4)
        {
            karma += 1;
            GameObject chief = GameObject.Find("ChiefBody");
            chief.GetComponent<NpcManager>().canvas[3].SetActive(true);
            while (chief.GetComponent<NpcManager>().canvas[3].activeInHierarchy)
                yield return null;
            if (!npc.GetComponent<NpcManager>().canvas[11].activeInHierarchy)
            {
                npc.GetComponent<NpcManager>().canvas[11].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[11].activeInHierarchy)
                    yield return null;
            }
            else
            {
                while (npc.GetComponent<NpcManager>().canvas[11].activeInHierarchy)
                    yield return null;
            }
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
        }
        else if (step == 5)
        {
            sailorStep = 5;
            GameObject second = GameObject.Find("SecondBody");
            if (killerStep != 2)
            {
                npc.GetComponent<NpcManager>().canvas[14].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[14].activeInHierarchy)
                    yield return null;
                second.GetComponent<NpcManager>().canvas[0].SetActive(true);
                while (!npc.GetComponent<NpcManager>().canvas[16].activeInHierarchy)
                    yield return null;
                while (npc.GetComponent<NpcManager>().canvas[16].activeInHierarchy)
                    yield return null;
            }
            else if (killerStep == 2)
            {
                second.GetComponent<NpcManager>().canvas[1].SetActive(true);
                while (second.GetComponent<NpcManager>().canvas[1].activeInHierarchy)
                    yield return null;
                npc.GetComponent<NpcManager>().canvas[12].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[12].activeInHierarchy)
                    yield return null;
                second.GetComponent<NpcManager>().canvas[2].SetActive(true);
                while (!npc.GetComponent<NpcManager>().canvas[13].activeInHierarchy)
                    yield return null;
                while (npc.GetComponent<NpcManager>().canvas[13].activeInHierarchy)
                    yield return null;
            }
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
        }
    }

    int bored;

    public void GreedQuest(int step)
    {
        npc = GameObject.FindWithTag("talking");
        if (step == 0)
        {
            greedStep = 0;
            npc.GetComponent<NpcManager>().canvas[0].SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                triggers[i].GetComponent<BoxCollider>().isTrigger = false;
            }
            npc.tag = "Untagged";
            if (sailorStep != 3)
            {
                GameObject sailor = GameObject.Find("SailorBody");
                sailor.GetComponent<NavMeshAgent>().enabled = false;
                GameObject newPos = GameObject.Find("SailorPosVillage");
                sailor.transform.position = newPos.transform.position;
                sailor.GetComponent<NavMeshAgent>().enabled = true;
            }
            bored = 0;
            karma -= 1;
            greedStep = 1;
        }
        else if (step == 1)
        {
            greedStep = 1;
            if (controller.isHolding)
            {
                GameObject heldItem = GameObject.FindWithTag("held");
                if (heldItem.name == "Pickaxe")
                {
                    npc.GetComponent<NpcManager>().canvas[4].SetActive(true);
                    //Destroy(heldItem);
                    //controller.isHolding = false;
                    triggers[5].GetComponent<BoxCollider>().isTrigger = false;
                    if (killerStep != 2)
                    {
                        GameObject newPos = GameObject.Find("ChiefEndPos");
                        GameObject newPos2 = GameObject.Find("SailorEndPos");
                        GameObject sailor = GameObject.Find("SailorBody");
                        GameObject chief = GameObject.Find("ChiefBody");
                        chief.transform.position = newPos.transform.position;
                        sailor.transform.position = newPos2.transform.position;
                        chief.GetComponent<NpcManager>().isTalkable = true;
                        sailor.GetComponent<NpcManager>().isTalkable = true;
                    }
                    GameObject gold = GameObject.Find("GoldSeam");
                    gold.GetComponent<NpcManager>().isTalkable = true;
                    sailorStep = 3;
                    greedStep = 2;
                }
                else
                {
                    if (bored == 0)
                    {
                        npc.GetComponent<NpcManager>().canvas[1].SetActive(true);
                        bored = 1;
                    }
                    else if (bored == 1)
                    {
                        npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
                        bored = 2;
                    }
                    else if (bored == 2)
                    {
                        npc.GetComponent<NpcManager>().canvas[3].SetActive(true);
                    }
                }
            }
            else
            {
                if (bored == 0)
                {
                    npc.GetComponent<NpcManager>().canvas[1].SetActive(true);
                    bored = 1;
                }
                else if (bored == 1)
                {
                    npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
                    bored = 2;
                }
                else if (bored == 2)
                {
                    npc.GetComponent<NpcManager>().canvas[3].SetActive(true);
                }
            }
        }
        else if (step == 2)
        {
            greedStep = 2;
            npc.GetComponent<NpcManager>().canvas[5].SetActive(true);
        }
    }

    void GoldDigging()
    {
        npc = GameObject.FindWithTag("talking");
        if (controller.isHolding)
        {
            GameObject heldItem = GameObject.FindWithTag("held");
            if (heldItem.name == "Pickaxe")
            {
                goldGet = true;
                karma -= 1;
                npc.GetComponent<NpcManager>().isTalkable = false;
                npc.tag = "Untagged";
                controller.hasControl = true;
            }
            else
            {
                controller.hasControl = true;
            }
        }
        else
        {
            controller.hasControl = true;
        }
    }

    IEnumerator AssassinQuest(int step)
    {
        npc = GameObject.FindWithTag("talking");
        if (step == 0)
        {
            sailorStep = 4;
            GameObject sailor = GameObject.Find("SailorBody");
            GameObject chief = GameObject.Find("ChiefBody");
            sailor.GetComponent<NavMeshAgent>().enabled = false;
            chief.GetComponent<NavMeshAgent>().enabled = false;
            GameObject newPos = GameObject.Find("SailorEndPos");
            GameObject newPos2 = GameObject.Find("ChiefEndPos");
            sailor.transform.position = newPos.transform.position;
            chief.transform.position = newPos2.transform.position;
            npc.GetComponent<NpcManager>().canvas[0].SetActive(true);
            while (npc.GetComponent<NpcManager>().canvas[0].activeInHierarchy)
                yield return null;
            controller.hasControl = false;
            if (npc.GetComponent<NpcManager>().canvas[1].activeInHierarchy)
            {
                while (npc.GetComponent<NpcManager>().canvas[1].activeInHierarchy)
                    yield return null;
                controller.hasControl = false;
                newPos = GameObject.Find("AssassinPos");
                npc.GetComponent<NavMeshAgent>().destination = newPos.transform.position;
                while (npc.GetComponent<NavMeshAgent>().transform.position.z != newPos.transform.position.z)
                    yield return null;
                npc.GetComponent<NpcManager>().HideObject();
                controller.hasControl = true;
                chief.GetComponent<NpcManager>().HideObject();
                sailorStep = 4;
                karma -= 1;
                killerStep = 2;
            }
            else
            {
                controller.hasControl = true;
                killerStep = 1;
            }

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
                    npc.GetComponent<NpcManager>().HideObject();
                    npc.GetComponent<NpcManager>().isTalkable = false;
                    controller.hasControl = true;
                }
                else
                {
                    npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
                }
            }
            else
            {
                npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
            }
        }
        npc.tag = "Untagged";
    }

    void NativeQuest(int step)
    {
        npc = GameObject.FindWithTag("talking");
        if (step == 0)
        {
            if (sailorStep == 3)
            {
                npc.GetComponent<NpcManager>().canvas[1].SetActive(true);
            }
            else
            {
                npc.GetComponent<NpcManager>().canvas[0].SetActive(true);
            }
        }
    }

    IEnumerator HarshQuest(int step)
    {
        npc = GameObject.FindWithTag("talking");
        if (step == 0)
        {
            sailorStep = 4;
            GameObject sailor = GameObject.Find("SailorBody");
            GameObject chief = GameObject.Find("ChiefBody");
            sailor.GetComponent<NavMeshAgent>().enabled = false;
            chief.GetComponent<NavMeshAgent>().enabled = false;
            GameObject newPos = GameObject.Find("SailorEndPos");
            GameObject newPos2 = GameObject.Find("ChiefEndPos");
            sailor.transform.position = newPos.transform.position;
            chief.transform.position = newPos2.transform.position;
            if (!goldGet)
            {
                npc.GetComponent<NpcManager>().canvas[0].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[0].activeInHierarchy)
                    yield return null;
            }
            else
            {
                npc.GetComponent<NpcManager>().canvas[1].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[1].activeInHierarchy)
                    yield return null;
            }
            if (npc.GetComponent<NpcManager>().canvas[2].activeInHierarchy)
            {
                harshStep = 1;
            }
            else if (npc.GetComponent<NpcManager>().canvas[3].activeInHierarchy)
            {
                karma -= 1;
                harshStep = 2;
            }
            else
            {
                npc.GetComponent<NpcManager>().isTalkable = false;
            }
        }
        else if (step == 1)
        {
            npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
        }
        else if (step == 2)
        {
            npc.GetComponent<NpcManager>().canvas[4].SetActive(true);
        }
    }

    void SideQuest()
    {
        npc = GameObject.FindWithTag("talking");
        npc.GetComponent<NpcManager>().canvas[0].SetActive(true);
        //npc.tag = "Untagged";
    }
    #endregion World 2

    #region World 3
    //*------------------------------ WORLD 3 - THE OFFICER ------------------------------*//
    #endregion World 3
}