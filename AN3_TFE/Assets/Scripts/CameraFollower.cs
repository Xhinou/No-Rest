using UnityEngine;
using UnityEngine.AI;

public class CameraFollower : MonoBehaviour
{
    public bool smooth;
    public GameObject
        player,
        camTarget;
    private Vector3
        offset,
        velocity = Vector3.zero;
    public float smoothDuration = 0.8f;
    NavMeshAgent playerAgent;

    void Start()
    {
        offset = transform.position - player.transform.position;
        playerAgent = player.GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        if (smooth)
        {
            if (playerAgent.velocity.x != 0f && playerAgent.velocity.z != 0f)
                transform.position = Vector3.SmoothDamp(transform.position, camTarget.transform.position + offset, ref velocity, smoothDuration);
            else
                transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 1f);
        } else
            transform.position = player.transform.position + offset;
    }
}
