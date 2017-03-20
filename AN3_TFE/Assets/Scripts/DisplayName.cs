using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayName : MonoBehaviour {

    Vector3 positionOffset = Vector3.up;
    Camera mainCam;
    [HideInInspector] public GameObject canvas;
    GameObject
        panel,
        theText;
    Text nameText;

	void Awake () {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas = Instantiate(Resources.Load("ObjectNameDisplay")) as GameObject;
        panel = canvas.transform.FindChild("Panel").gameObject;
        theText = panel.transform.FindChild("Text").gameObject;
        nameText = theText.GetComponent<Text>();
	}

    private void Start()
    {
        nameText.text = gameObject.name.ToUpper();
        SetCanvasPos();
    }

    void Update () {        
        canvas.transform.LookAt(mainCam.transform);
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - mainCam.transform.position);
    }

    public void SetCanvasPos()
    {
        canvas.transform.position = transform.position + positionOffset;
    }
}
