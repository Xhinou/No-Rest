using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ItemManager : MonoBehaviour
{
    public GameObject player;
    public int itemID;
    public bool
        isPickable = true,
        isClicked;
    GameObject itemInfoText;
    Text itemInfo;
    string getItem = " added to inventory";
    Transform gOTransform;
    CharacterClickingController controller;
    
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
    }

    void OnCollisionEnter(Collision coln)
    {
        if (coln.gameObject.tag == "Player")
        {
            if (controller.hasClicked && isPickable && isClicked)
            {              
                if (!controller.isHolding)
                {
                    gOTransform.parent = controller.rightHand.transform;
                    gOTransform.localPosition = new Vector3(0.08f, -0.039f, 0);
                    gOTransform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                    controller.isHolding = true;
                }
                else if (controller.isHolding)
                {
                    GameObject heldItem = GameObject.FindWithTag("held");
                    heldItem.GetComponent<Rigidbody>().isKinematic = false;
                    heldItem.GetComponent<Rigidbody>().AddForce(10f, 10f, 10f);
                    heldItem.GetComponent<Collider>().enabled = true;
                    heldItem.GetComponent<NavMeshObstacle>().enabled = true;
                    heldItem.transform.parent = GameObject.Find("Scene").transform;
                    heldItem.tag = "Untagged";
                    gOTransform.parent = GameObject.Find("Character1_RightHand").transform;
                    gOTransform.localPosition = new Vector3(0.08f, -0.039f, 0);
                    gOTransform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                }
                DisplayText();
                controller.hasClicked = false;
                isClicked = false;
            }
        }
    }

    void OnCollisionStay(Collision coln)
    {
        if (coln.gameObject.tag == "Player")
        {
            if (controller.hasClicked && isPickable && isClicked)
            {
                if (!controller.isHolding)
                {
                    gOTransform.parent = GameObject.FindWithTag("Player").transform;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                    controller.isHolding = true;
                }
                else if (controller.isHolding)
                {
                    GameObject heldItem = GameObject.FindWithTag("held");
                    heldItem.GetComponent<Rigidbody>().isKinematic = false;
                    heldItem.GetComponent<Rigidbody>().AddForce(10f, 10f, 10f);
                    heldItem.GetComponent<Collider>().enabled = true;
                    heldItem.GetComponent<NavMeshObstacle>().enabled = true;
                    heldItem.transform.parent = GameObject.Find("Scene").transform;
                    heldItem.tag = "Untagged";
                    gOTransform.parent = GameObject.FindWithTag("Player").transform;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                }
                DisplayText();
                controller.hasClicked = false;
                isClicked = false;
            }
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
