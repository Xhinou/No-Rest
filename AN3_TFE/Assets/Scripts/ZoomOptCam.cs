using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOptCam : MonoBehaviour {

    Camera mainCam, optCam;

	void Start () {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        optCam = GetComponent<Camera>();
	}
	
	void Update () {
        optCam.fieldOfView = mainCam.fieldOfView;
	}
}
