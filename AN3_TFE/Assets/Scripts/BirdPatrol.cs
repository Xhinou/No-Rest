using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BirdPatrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private bool isCoroutineRunning, lastCoroutine;
    private Animator birdAnim;
    private SphereCollider col;
    private GameObject player;

    void Start()
    {
        birdAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindWithTag("Player");
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        StartCoroutine(GotoNextPoint());
    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (agent.enabled)
        {
            if (agent.remainingDistance < 0.5f && !isCoroutineRunning)
            {
                StartCoroutine(GotoNextPoint());
            }
        }
    }

    IEnumerator GotoNextPoint()
    {
        isCoroutineRunning = true;
        // Returns if no points have been set up
        if (points.Length == 0)
            yield return null;
        agent.ResetPath();
        birdAnim.SetBool("hasStopped", true);
        yield return new WaitForSeconds(2);
        birdAnim.SetBool("hasStopped", false);
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        /*if (lastCoroutine)
            agent.enabled = false;*/
        isCoroutineRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            agent.ResetPath();
            col.enabled = false;
            //lastCoroutine = true;
            agent.enabled = false;
            StartCoroutine(BirdFlying());
        }
    }

    public float flySpeed;

    IEnumerator BirdFlying()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-direction);
        //transform.Rotate(Vector3.up, 90f);
        birdAnim.SetBool("isTrig", true);
        while (transform.position.y < 50)
        {
            transform.Translate(Vector3.up * flySpeed);
            transform.Translate(Vector3.forward * flySpeed);
            flySpeed += 0.005f;
            yield return null;
        }
        Destroy(gameObject);
    }
}