using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CameraFollower : MonoBehaviour
{
    public bool
        smooth,
        doRotate;
    public GameObject
        player,
        camTarget;
    private Vector3
        offset,
        velocity = Vector3.zero;
    public float
        smoothDuration = 0.8f,
        turningRate = 10f;
    NavMeshAgent playerAgent;
    private Quaternion
        downRotation,
        upRotation,
        targetRotation;

    void Start()
    {
        offset = transform.position - player.transform.position;
        playerAgent = player.GetComponent<NavMeshAgent>();
        downRotation = Quaternion.identity;
        upRotation = Quaternion.Euler(25.11f, 131.594f, 0);
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

    public IEnumerator CamRotation(string rotationDir)
    {
        if (rotationDir == "Up")
            targetRotation = upRotation;
        else if (rotationDir == "Down")
            targetRotation = downRotation;
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate * Time.deltaTime);
            yield return null;
        }
    }
}
