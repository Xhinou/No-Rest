using UnityEngine;
using System.Collections.Generic;

public class FadeOutBlockObjects : MonoBehaviour
{
    LayerMask fadeOut;
    private bool hitFlag;
    //RaycastHit oldHit;
    public Transform player;
    Vector3 dist;
    Camera mainCam;
    List<Material> materials;
    public Color hiddenColor;
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
            foreach (Material mat in materials)
            {
                mat.SetColor("_OutlineColor", hiddenColor);
            }
        }
        else if (hitFlag)
        {
            hitFlag = false;
            foreach (Material mat in materials)
            {
                mat.SetColor("_OutlineColor", visibleColor);
            }
        }
    }

    void FindAllChildren(Transform legacy)
    {
        foreach (Transform child in legacy)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                materials.Add(child.GetComponent<Renderer>().material);
            }
            FindAllChildren(child);
        }
    }
}
