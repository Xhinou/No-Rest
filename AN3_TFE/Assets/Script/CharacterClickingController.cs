using UnityEngine;
using System.Collections;

public class CharacterClickingController : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public bool isHolding = false;
    public bool hasControl = false;
    public bool hasClicked = false;
    public bool isPlayerTrigger;
    public bool canSkipDial = true;
    LayerMask navMap;
    GameObject npc;
    public Animator anim;
    bool isMoving;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMap = LayerMask.GetMask("NavMap");
        npc = GameObject.FindWithTag("talking");
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && hasControl)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, navMap.value))
            {
                agent.destination = hit.point;
                if (hit.transform.tag == "static")
                {
                    hasClicked = false;
                }
                //mouseHit = hit;
            }
        }
        if (isPlayerTrigger && !hasControl)
        {
            //npc = GameObject.FindWithTag("talking");
            Vector3 direction = (npc.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
        }
        if (agent.velocity.x != 0 || agent.velocity.z != 0)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }
        anim.SetBool("isMoving", isMoving);
    }

    public void UnableControl()
    {
        hasControl = false;
    }

    public static void DropItem()
    {
        /*GameObject heldItem;
		heldItem= GameObject.Find ("player/Item " + heldItem.GetComponent<ItemManager>().itemID);
		heldItem.GetComponent<Rigidbody> ().AddForce (1f, 0f, 0f);
		heldItem.GetComponent<CapsuleCollider> ().isTrigger = false;
		heldItem.GetComponent<ItemManager> ().isPicked = false;
		isHolding = false;
		heldItem.transform.parent = GameObject.Find ("PNJ " + GetComponent<NpcManager>().npcID).transform;
		heldItem.SetActive (false);*/
    }
}
