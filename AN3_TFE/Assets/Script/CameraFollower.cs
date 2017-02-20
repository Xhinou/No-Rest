using UnityEngine;
using UnityEngine.AI;

public class CameraFollower : MonoBehaviour
{
    public GameObject
        player, 
        camTarget;
    private Vector3
        offset,
        velocity = Vector3.zero;
    public float smoothDuration = 0.8f;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (player.GetComponent<NavMeshAgent>().velocity.x != 0f && player.GetComponent<NavMeshAgent>().velocity.z != 0f)
            transform.position = Vector3.SmoothDamp(transform.position, camTarget.transform.position + offset, ref velocity, smoothDuration);
        else
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 1f);
    }
}
