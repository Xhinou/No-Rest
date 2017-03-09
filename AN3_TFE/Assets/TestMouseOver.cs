using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseOver : MonoBehaviour {

    private void OnMouseEnter()
    {
        Debug.Log(gameObject.name);
    }
}
