using UnityEngine;

public class RaycastReceiver : MonoBehaviour
{
    public GameObject
        highlight,
        player;
    public bool isNpc;
    CharacterClickingController controller;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
        highlight = GameObject.Find("Highlight");
    }

    void Start()
    {
        highlight.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
        {
            if (isNpc)
            {
                if (gameObject.tag != "held" && controller.hasControl && gameObject.GetComponent<NpcManager>().isTalkable)
                {
                    highlight.transform.position = gameObject.transform.position;
                    highlight.SetActive(true);
                }
            }
            else if (!isNpc)
            {
                if (gameObject.tag != "held" && controller.hasControl && gameObject.GetComponent<ItemManager>().isPickable)
                {
                    highlight.transform.position = gameObject.transform.position;
                    highlight.SetActive(true);
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (isNpc)
        {
            if (gameObject.tag != "held" && controller.hasControl && gameObject.GetComponent<NpcManager>().isTalkable)
            {            
                controller.hasClicked = true;
                gameObject.GetComponent<NpcManager>().isClicked = true;
            }
        }
        else if (!isNpc)
        {
            if (gameObject.tag != "held" && controller.hasControl && gameObject.GetComponent<ItemManager>().isPickable)
            {
                controller.hasClicked = true;
                gameObject.GetComponent<ItemManager>().isClicked = true;
            }
        }
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
