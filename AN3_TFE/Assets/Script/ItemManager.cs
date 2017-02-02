using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{

    public int itemID;
    public bool isPickable = true;
    public bool isClicked;
    GameObject itemInfoText;
    Text itemInfo;
    string getItem = " added to inventory";

    CharacterClickingController controller;
    public GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Start()
    {
        itemInfoText = GameObject.Find("ItemInfoText");
        itemInfo = itemInfoText.GetComponent<Text>();
    }

    void OnCollisionEnter(Collision coln)
    {
        if (coln.gameObject.tag == "Player")
        {
            if (controller.hasClicked && isPickable && isClicked)
            {
                if (!controller.isHolding)
                {
                    gameObject.transform.parent = GameObject.Find("Character1_RightHand").transform;
                    gameObject.transform.localPosition = new Vector3(0.08f, -0.039f, 0);
                    gameObject.transform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                    controller.isHolding = true;
                }
                else if (controller.isHolding)
                {
                    GameObject heldItem = GameObject.FindWithTag("held");
                    heldItem.GetComponent<Rigidbody>().isKinematic = false;
                    heldItem.GetComponent<Rigidbody>().AddForce(10f, 10f, 10f);
                    heldItem.GetComponent<Collider>().enabled = true;
                    heldItem.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                    heldItem.transform.parent = GameObject.Find("Scene").transform;
                    heldItem.tag = "Untagged";
                    gameObject.transform.parent = GameObject.Find("Character1_RightHand").transform;
                    gameObject.transform.localPosition = new Vector3(0.08f, -0.039f, 0);
                    gameObject.transform.localRotation = new Quaternion(0, 0.7f, 0.7f, 0);
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
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
                    gameObject.transform.parent = GameObject.FindWithTag("Player").transform;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
                    gameObject.tag = "held";
                    controller.isHolding = true;
                }
                else if (controller.isHolding)
                {
                    GameObject heldItem = GameObject.FindWithTag("held");
                    heldItem.GetComponent<Rigidbody>().isKinematic = false;
                    heldItem.GetComponent<Rigidbody>().AddForce(10f, 10f, 10f);
                    heldItem.GetComponent<Collider>().enabled = true;
                    heldItem.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                    heldItem.transform.parent = GameObject.Find("Scene").transform;
                    heldItem.tag = "Untagged";
                    gameObject.transform.parent = GameObject.FindWithTag("Player").transform;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Collider>().enabled = false;
                    gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
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
        itemInfo.text = "<b>" + gameObject.name + "</b>" + getItem;
        itemInfoText.GetComponent<Text>().canvasRenderer.SetAlpha(1f);
        itemInfoText.GetComponent<Text>().CrossFadeAlpha(0f, 2f, false);
    }
}
