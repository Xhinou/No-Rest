using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public GameObject player;
    public int itemID;
    public bool
        isPicked,
        isClicked;
    GameObject
        itemInfoText,
        heldItem,
        nameCanvas;
    Text itemInfo;
    string getItem = " added to inventory";
    Transform gOTransform;
    CharacterClickingController controller;
    Transform scene;
    SphereCollider trigCol;
    
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
        nameCanvas = gameObject.GetComponent<DisplayName>().canvas;
        trigCol = gameObject.GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player" && !isPicked)
        {
            if (controller.hasClicked && isClicked)
                ItemPicking();
        }
    }

    void OnTriggerStay(Collider colr)
    {
        if (colr.gameObject.tag == "Player" && !isPicked)
        {
            if (controller.hasClicked && isClicked)
                ItemPicking();
        }
    }

    void ItemPicking()
    {
        if (!controller.isHolding)
        {
            PutInHand();
            controller.isHolding = true;
        }
        else if (controller.isHolding)
        {
            heldItem = GameObject.FindWithTag("held");
            DisplayName hDisplayName = heldItem.GetComponent<DisplayName>();
            heldItem.transform.parent = scene;
            heldItem.GetComponent<ItemManager>().isPicked = false;
            heldItem.transform.position = gameObject.transform.position;
            heldItem.transform.rotation = gameObject.transform.rotation;
            heldItem.GetComponent<SphereCollider>().enabled = true;
            hDisplayName.canvas.SetActive(true);
            hDisplayName.SetCanvasPos();
            heldItem.tag = "Untagged";
            PutInHand();
        }
        DisplayText();
        controller.hasClicked = false;
        isClicked = false;
    }
    void PutInHand()
    {
        gOTransform.parent = controller.rightHand.transform;
        gOTransform.localPosition = new Vector3(0.08f, -0.039f, 0);
        gOTransform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
        isPicked = true;
        trigCol.enabled = false;
        nameCanvas.SetActive(false);
        tag = "held";
    }

    void DisplayText()
    {
        string color = controller.itemColor;
        //controller.pickedItem
        
        int size = itemInfo.fontSize + 5;
        itemInfo.text = "<size=" + size + ">" + "<color=#" + color + ">" + gameObject.name + "</color></size>" + getItem;
        itemInfoText.GetComponent<Text>().canvasRenderer.SetAlpha(1f);
        itemInfoText.GetComponent<Text>().CrossFadeAlpha(0f, 4f, false);
    }
}
