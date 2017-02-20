using UnityEngine;
using System.Collections;

public class CanvasFollowNpc : MonoBehaviour
{
    public GameObject refNpc;

    void Update()
    {
        transform.position = new Vector3(refNpc.transform.position.x, transform.position.y, refNpc.transform.position.z);
    }
}
