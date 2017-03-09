using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ItemManager : MonoBehaviour
{
    public GameObject player;
    public int itemID;
    public bool
        isPicked,
        isClicked;
    GameObject itemInfoText;
    Text itemInfo;
    string getItem = " added to inventory";
    Transform gOTransform;
    CharacterClickingController controller;
    GameObject heldItem;
    Transform scene;
    
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Start()
    {
        itemInfoText = GameObject.Find("ItemInfoText");
        itemInfo = itemInfoText.GetComponent<Text>();
        gOTransform = gameObject.transform;
        scene = GameObject.Find("Scene").transform;
    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player" && !isPicked)
        {
            ItemPicking();
        }
    }

    void OnTriggerStay(Collider colr)
    {
        if (colr.gameObject.tag == "Player" && !isPicked)
        {
            ItemPicking();
        }
    }

    void ItemPicking()
    {
        if (controller.hasClicked && isClicked)
        {
            if (!controller.isHolding)
            {
                gOTransform.parent = controller.rightHand.transform;
                gOTransform.localPosition = new Vector3(0.08f, -0.039f, 0);
                gOTransform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                isPicked = true;
                tag = "held";
                controller.isHolding = true;
            }
            else if (controller.isHolding)
            {
                heldItem = GameObject.FindWithTag("held");
                heldItem.transform.parent = scene;
                heldItem.GetComponent<ItemManager>().isPicked = false;
                heldItem.tag = "Untagged";
                gOTransform.parent = controller.rightHand.transform;
                gOTransform.localPosition = new Vector3(0.08f, -0.039f, 0);
                gOTransform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                isPicked = true;
                tag = "held";
            }
            DisplayText();
            controller.hasClicked = false;
            isClicked = false;
        }
    }

    void DisplayText()
    {
        Color color = controller.pickedItem;
        //controller.pickedItem
        itemInfo.text = "<size=8>" + "<color=" + color + ">" + gameObject.name + "</color></size>" + getItem;
        itemInfoText.GetComponent<Text>().canvasRenderer.SetAlpha(1f);
        itemInfoText.GetComponent<Text>().CrossFadeAlpha(0f, 4f, false);
    }
}
