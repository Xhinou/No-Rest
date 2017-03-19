using UnityEngine;

public class DisplayObjectName : MonoBehaviour {

    //public Transform toDisplayObject; //enemy's transform
    public Vector3 positionOffset = Vector3.up; //offset relative to enemy's origin, so it doesn't, say, draw the text at the enemy's feet
    GameObject objectName;
    Camera mainCam;

    private void Start()
    {
        objectName = Instantiate(Resources.Load("HoverText")) as GameObject;
        GUIText nameDisplay = objectName.GetComponent<GUIText>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        nameDisplay.text = gameObject.name; // Change Me
    }

    private void Update()
    {
        //keep in mind this needs special code if your enemy is behind the camera
        //transform.position = Camera.main.WorldToViewportPoint(toDisplayObject.position + positionOffset);
        objectName.transform.position = mainCam.WorldToViewportPoint(transform.position + positionOffset); // Change the 0.05f value to some other value for desired heigh
    }

}
