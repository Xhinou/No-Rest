using UnityEngine;
using System.Collections.Generic;

public class FadeOutBlockObjects : MonoBehaviour
{
    LayerMask fadeOut;
    private bool hitFlag;
    RaycastHit oldHit;
    public Transform player;
    Vector3 dist;
    Camera mainCam;
    public List<Material> materials;
    public Color hideColor;
    public Color visibleColor;

    void Start()
    {
        fadeOut = LayerMask.GetMask("FadeOut");
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        materials = new List<Material>();
        FindAllChildren(player);
    }

    void Update()
    {
        RaycastHit hit;
        dist = player.position - mainCam.transform.position;
        if (Physics.Raycast(mainCam.transform.position, dist, out hit, 1000, fadeOut.value))
        {
            hitFlag = true;
            /*
            Color colorA = hit.collider.GetComponent<Renderer>().material.color;
            colorA.a = 0.3f;
            hit.collider.GetComponent<Renderer>().material.SetColor("_Color", colorA);
            */
            foreach (Material mat in materials)
            {
                mat.SetColor("_OutlineColor", visibleColor);
            }
            oldHit = hit;
        }
        else if (hitFlag)
        {
            hitFlag = false;
            /*Color colorB = oldHit.collider.GetComponent<Renderer>().material.color;
            colorB.a = 1f;
            oldHit.collider.GetComponent<Renderer>().material.SetColor("_Color", colorB);*/
            foreach (Material mat in materials) {
                mat.SetColor("_OutlineColor", hideColor);
            }
        }
    }

    void FindAllChildren(Transform papa)
    {
        foreach (Transform child in papa)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                materials.Add(child.GetComponent<Renderer>().material);
            }
            FindAllChildren(child);
        }
    }
}
