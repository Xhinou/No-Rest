using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    public string itemColor;
    public GameObject rightHand, clicCube;
    public AudioSource audioWalk;
    Camera mainCam;
    QuestManager qManager;
    GameObject
        scriptSystem,
        npcToLook;
    public bool lookAtNpc = false;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        qManager = scriptSystem.GetComponent<QuestManager>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMap = LayerMask.GetMask("NavMap");
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && hasControl && !qManager.pause.activeInHierarchy)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 1000, navMap.value))
            {
                agent.destination = hit.point;
                if (Input.GetMouseButtonDown(0) && !hasClicked)
                {
                    //StopCoroutine("ClicFeedback");
                    ClicFeedback(hit);
                } else if (Input.GetMouseButtonDown(0) && hasClicked) {
                    Destroy(_theClic);
                }
                if (hit.transform.tag == "static")
                    hasClicked = false;
            }
        }
        if (isPlayerTrigger && !hasControl && !isMoving)
        {
            Vector3 direction = (npc.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
        }
        if (!isPlayerTrigger && lookAtNpc)
        {
            Vector3 direction = (npcToLook.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);

        }
        if (agent.enabled)
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
                        audioWalk.mute = true;
                    }
                }
                else
                {
                    isMoving = true;
                    anim.SetBool("isMoving", isMoving);
                    audioWalk.mute = false;
                }
            }
        }
    }

    public void UnableControl()
    {
        hasControl = false;
    }

    public void LookAtNpc(GameObject _npcToLook)
    {
        lookAtNpc = true;
        npcToLook = _npcToLook;
    }

    GameObject _theClic;

    /*IEnumerator ClicFeedback(RaycastHit _hit)
    {
        if (_theClic != null)
            Destroy(_theClic);
        GameObject theClic = Instantiate(Resources.Load("Clic") as GameObject, agent.destination, Quaternion.identity);
        _theClic = theClic;
        yield return new WaitForSeconds(1f);
        Destroy(theClic);
    }*/

    void ClicFeedback(RaycastHit _hit)
    {
        if (_theClic != null)
            Destroy(_theClic);
        GameObject theClic = Instantiate(Resources.Load("Clic") as GameObject, agent.destination, Quaternion.identity);
        _theClic = theClic;
    }
}
