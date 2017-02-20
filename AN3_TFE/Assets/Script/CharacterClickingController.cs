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
    GameObject npc;
    public Animator anim;
    bool isMoving;
    public Color pickedItem;
    public GameObject rightHand;
    Camera mainCam;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMap = LayerMask.GetMask("NavMap");
        npc = GameObject.FindWithTag("talking");
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
        if (agent.velocity.x != 0 || agent.velocity.z != 0)
            isMoving = true;
        else
            isMoving = false;
        anim.SetBool("isMoving", isMoving);
    }

    public void UnableControl()
    {
        hasControl = false;
    }
}
