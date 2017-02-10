using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{

    public int
        sceneID,
        introStep = 0,
        introEndStep;
    public GameObject introGoal;
    public bool hasFollowedSailor = true;
    GameObject
        npc,
        player;
    public GameObject[] triggers = new GameObject[7];
    int karma = 0;
    CharacterClickingController controller;
    DialoguesSystem dialogSystem;
    public GameObject scriptSystem;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        dialogSystem = scriptSystem.GetComponent<DialoguesSystem>();
        StartCoroutine(RunIntro());
        if (sceneID == 2) introEndStep = 3;
    }

    void Update()
    {
        // Debug.Log("Karma value is: " + karma);
        //Debug.Log(sailorStep);
    }

    IEnumerator RunIntro()
    {
        if (sceneID == 0)
        {
            yield return new WaitForSeconds(2);
            //ANIM EMBODY "SPIRIT FORM"
            controller.hasControl = true;
        }
        else if (sceneID == 1)
        {

        }
        else if (sceneID == 2)
        {
            GameObject sailor = GameObject.Find("SailorBody");
            //GameObject player = GameObject.FindWithTag("Player");
            sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = player.transform.position;
            while (introStep == 0)
            {
                if (controller.isPlayerTrigger)
                {
                    sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
                    introStep++;
                }
                yield return null;
            }
            sailor.GetComponent<NpcManager>().canvas[0].SetActive(true);
            while (introStep == 1)
                yield return null;
            if (introStep == 2)
            {
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = introGoal.transform.position;
                while (sailor.transform.position.z >= introGoal.transform.position.z + 0.03f)
                {
                    //Debug.Log(sailor.transform.position.z);
                    yield return null;
                }
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
                sailor.GetComponent<NpcManager>().HideObject();
                controller.agent.ResetPath();
                controller.hasControl = true;
                introStep++;
            }
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
        if (step == 0)
        {
            dialogSystem.DisplayText(npcID, step);
            sailorStep = 0;
            if (hasFollowedSailor)
            {
                karma += 1;
                npc.GetComponent<NpcManager>().canvas[1].SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                }
                sailorStep = 1;
            }
            else
            {
                karma -= 1;
                npc.GetComponent<NpcManager>().canvas[2].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[2].activeInHierarchy)
                    yield return null;
                npc.tag = "Untagged";
                for (int i = 0; i < 3; i++)
                {
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                }
                GameObject newPos = GameObject.Find("SailorPosA");
                npc.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = newPos.transform.position;
                npc.GetComponent<NpcManager>().isTalkable = false;
                while (npc.transform.position.z != newPos.transform.position.z)
                    yield return null;
                npc.GetComponent<NpcManager>().isTalkable = true;
                hasFollowedSailor = true;
                sailorStep = 0;
            }
        }
        else if (step == 1)
        {
            sailorStep = 1;
            if (controller.isHolding)
            {
                GameObject heldItem = GameObject.FindWithTag("held");
                if (heldItem.name == "Stone")
                {
                    karma -= 1;
                    npc.GetComponent<NpcManager>().canvas[4].SetActive(true);
                    Destroy(heldItem);
                    controller.isHolding = false;
                }
                else if (heldItem.name == "Shell")
                {
                    karma -= 1;
                    npc.GetComponent<NpcManager>().canvas[5].SetActive(true);
                    Destroy(heldItem);
                    controller.isHolding = false;
                }
                else if (heldItem.name == "Charcoal")
                {
                    karma += 1;
                    npc.GetComponent<NpcManager>().canvas[6].SetActive(true);
                    Destroy(heldItem);
                    controller.isHolding = false;
                }
                for (int i = 0; i < 4; i++)
                {
                    triggers[i].GetComponent<BoxCollider>().isTrigger = false;
                }
                while (npc.GetComponent<NpcManager>().canvas[4].activeInHierarchy ||
                npc.GetComponent<NpcManager>().canvas[5].activeInHierarchy ||
                npc.GetComponent<NpcManager>().canvas[6].activeInHierarchy)
                    yield return null;
                npc.GetComponent<NpcManager>().isTalkable = false;
                GameObject newPos = GameObject.Find("SailorPosVillage");
                npc.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = newPos.transform.position;
                while (npc.transform.position.z != newPos.transform.position.z)
                    yield return null;
                sailorStep = 2;
            }
            else
                npc.GetComponent<NpcManager>().canvas[3].SetActive(true);
        }
        else if (step == 2)
        {
            sailorStep = 2;
            GameObject chief = GameObject.Find("ChiefBody");
            npc.tag = "Untagged";
            if (greedStep == 0)
            {
                npc.GetComponent<NpcManager>().canvas[7].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[7].activeInHierarchy)
                    yield return null;
                chief.GetComponent<NpcManager>().canvas[0].SetActive(true);
                while (chief.GetComponent<NpcManager>().canvas[0].activeInHierarchy)
                    yield return null;
                if (npc.GetComponent<NpcManager>().canvas[8].activeInHierarchy)
                {
                    while (npc.GetComponent<NpcManager>().canvas[8].activeInHierarchy)
                        yield return null;
                    chief.GetComponent<NpcManager>().canvas[1].SetActive(true);
                    while (chief.GetComponent<NpcManager>().canvas[1].activeInHierarchy)
                        yield return null;
                    karma += 1;
                }
                else
                {
                    karma -= 1;
                }
            }
            else if (greedStep == 1)
            {
                npc.GetComponent<NpcManager>().canvas[9].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[9].activeInHierarchy)
                    yield return null;
                chief.GetComponent<NpcManager>().canvas[2].SetActive(true);
                while (chief.GetComponent<NpcManager>().canvas[2].activeInHierarchy)
                    yield return null;
                npc.GetComponent<NpcManager>().canvas[10].SetActive(true);
                while (npc.GetComponent<NpcManager>().canvas[10].activeInHierarchy)
                    yield return null;
            }
            triggers[5].GetComponent<BoxCollider>().isTrigger = false;
            GameObject newPos = GameObject.Find("ChiefEndPos");
            GameObject newPos2 = GameObject.Find("SailorEndPos");
            chief.GetComponent<NpcManager>().isTalkable = false;
            npc.GetComponent<NpcManager>().isTalkable = false;
            chief.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = newPos.transform.position;
            npc.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = newPos2.transform.position;
            while (chief.transform.position.z != newPos.transform.position.z && npc.transform.position.z != newPos2.transform.position.z)
                yield return null;
            chief.GetComponent<NpcManager>().isTalkable = true;
            npc.GetComponent<NpcManager>().isTalkable = true;
            sailorStep = 3;
        }
        else if (step == 3)
        {
            sailorStep = 3;
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
        else if (step == 4)
        {
            sailorStep = 4;
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
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                GameObject newPos = GameObject.Find("SailorPosVillage");
                sailor.transform.position = newPos.transform.position;
                sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
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
            sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            chief.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
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
                npc.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = newPos.transform.position;
                while (npc.GetComponent<UnityEngine.AI.NavMeshAgent>().transform.position.z != newPos.transform.position.z)
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
        npc = GameObject.FindWithTag ("talking");
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
            sailor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            chief.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
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