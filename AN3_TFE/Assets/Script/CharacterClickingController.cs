using UnityEngine;
using UnityEngine.AI;

public class CharacterClickingController : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool
        isHolding = false,
        hasControl = false,
        hasClicked = false,
        isPlayerTrigger,
        canSkipDial = false;
    LayerMask navMap;
    public GameObject npc;
    public Animator anim;
    public bool isMoving;
    public Color pickedItem;
    public GameObject rightHand;
    Camera mainCam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMap = LayerMask.GetMask("NavMap");
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && hasControl)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 1000, navMap.value))
            {
                agent.destination = hit.point;
                if (hit.transform.tag == "static")
                    hasClicked = false;
            }
        }
        if (isPlayerTrigger && !hasControl)
        {
            Vector3 direction = (npc.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
        }
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

    public void UnableControl()
    {
        hasControl = false;
    }
}
