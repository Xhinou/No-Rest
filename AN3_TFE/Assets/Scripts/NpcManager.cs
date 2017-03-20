using UnityEngine;
using UnityEngine.AI;

public class NpcManager : MonoBehaviour
{
    public int npcID;
    public bool
        isTalkable,
        isClicked,
        notNpc,
        isMoving;
    [HideInInspector] public GameObject
        scriptSystem,
        player;
    QuestManager qManager;
    CharacterClickingController controller;
    NavMeshAgent agent;
    Animator anim;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        player = GameObject.FindWithTag("Player");
        qManager = scriptSystem.GetComponent<QuestManager>();
        controller = player.GetComponent<CharacterClickingController>();
        if (!notNpc)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            anim = gameObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (!notNpc)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        agent.ResetPath();
                        isMoving = false;
                        anim.SetBool("isMoving", isMoving);
                    }
                }
                else
                {
                    isMoving = true;
                    anim.SetBool("isMoving", isMoving);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!notNpc && !isMoving)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        }
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