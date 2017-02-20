using UnityEngine;

public class FadeOutBlockObjects : MonoBehaviour
{
    LayerMask fadeOut;
    private bool hitFlag;
    RaycastHit oldHit;
    public GameObject player;
    Vector3 dir;
    Camera mainCam;

    void Start()
    {
        fadeOut = LayerMask.GetMask("FadeOut");
        mainCam = Camera.main;
    }

    void Update()
    {
        RaycastHit hit;
        dir = player.transform.position - mainCam.transform.position;
        if (Physics.Raycast(mainCam.transform.position, dir, out hit, 1000, fadeOut.value))
        {
            hitFlag = true;
            Color colorA = hit.collider.GetComponent<Renderer>().material.color;
            colorA.a = 0.3f;
            hit.collider.GetComponent<Renderer>().material.SetColor("_Color", colorA);
            oldHit = hit;
        }
        else if (hitFlag)
        {
            hitFlag = false;
            Color colorB = oldHit.collider.GetComponent<Renderer>().material.color;
            colorB.a = 1f;
            oldHit.collider.GetComponent<Renderer>().material.SetColor("_Color", colorB);
        }
    }
}
