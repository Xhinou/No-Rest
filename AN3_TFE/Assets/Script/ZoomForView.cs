using UnityEngine;

public class ZoomForView : MonoBehaviour {

    QuestManager qManager;

	void Start () {
        qManager = GameObject.Find("ScriptSystem").GetComponent<QuestManager>();
	}

    private void OnTriggerEnter(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
            StartCoroutine(qManager.CameraZoom(false));
    }

    private void OnTriggerExit(Collider colr)
    {
        if (colr.gameObject.tag == "Player")
            StartCoroutine(qManager.CameraZoom(true));
    }
}
