using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour
{

    public GameObject player;
    public GameObject camTarget;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public float smoothDuration = 0.8f;

    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (player.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.x != 0f && player.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.z != 0f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, camTarget.transform.position + offset, ref velocity, smoothDuration);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 1f);
        }
    }
}
