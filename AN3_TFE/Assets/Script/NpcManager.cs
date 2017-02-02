using UnityEngine;
using System.Collections;

public class NpcManager : MonoBehaviour
{

    public int npcID;
    public int canvasAmount;
    public GameObject[] canvas;
    public bool isTalkable;
    public bool isClicked;

    public GameObject scriptSystem;
    QuestManager qManager;
    CharacterClickingController controller;
    public GameObject player;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
    }

    public void TriggerEnter()
    {
        controller.isPlayerTrigger = true;
        if (controller.hasClicked && isClicked)
        {
            controller.hasControl = false;
            controller.agent.ResetPath();
            gameObject.tag = "talking";
            qManager.RunQuest(npcID);
            controller.hasClicked = false;
            isClicked = false;
        }
    }

    public void TriggerStay()
    {
        if (controller.hasClicked && isClicked)
        {
            controller.hasControl = false;
            controller.agent.ResetPath();

            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);

            //gameObject.tag = "talking";
            qManager.RunQuest(npcID);
            controller.hasClicked = false;
            isClicked = false;
        }
    }

    public void TriggerExit()
    {
        controller.isPlayerTrigger = false;
        for (int i = 0; i < canvasAmount; i++)
            canvas[i].SetActive(false);
        gameObject.tag = "Untagged";
    }

    public void DisplayObject()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void HideObject()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}