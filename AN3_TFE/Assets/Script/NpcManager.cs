using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public int
        npcID,
        canvasAmount;
    public GameObject[] canvas;
    public bool
        isTalkable,
        isClicked;
    public GameObject
        scriptSystem,
        player;
    QuestManager qManager;
    CharacterClickingController controller;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
    }

    public void TriggerEnter()
    {
        tag = "closeToPlayer";
        controller.npc = GameObject.FindWithTag("closeToPlayer");
        controller.isPlayerTrigger = true;
        if (controller.hasClicked && isClicked)
        {
            qManager.RunQuest(npcID);
            controller.hasClicked = false;
            isClicked = false;
        }
    }

    public void TriggerStay()
    {
        if (controller.hasClicked && isClicked)
        {            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
            qManager.RunQuest(npcID);
            controller.hasClicked = false;
            isClicked = false;
        }
    }

    public void TriggerExit()
    {        
        controller.isPlayerTrigger = false;
        tag = "Untagged";
        /*for (int i = 0; i < canvasAmount; i++)
            canvas[i].SetActive(false);*/
    }
}