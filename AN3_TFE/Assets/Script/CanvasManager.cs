using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour
{

    public Camera cameraToLookAt;
    public bool noControlAtDisable = false;

    public GameObject scriptSystem;
    QuestManager qManager;
    CharacterClickingController controller;
    public GameObject player;

    void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        qManager = scriptSystem.GetComponent<QuestManager>();
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Start()
    {
        cameraToLookAt = Camera.main;
    }

    void Update()
    {
        //transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - cameraToLookAt.transform.position);
    }

    void OnEnable()
    {
        controller.hasControl = false;
        controller.agent.ResetPath();
    }

    void OnDisable()
    {
        if (qManager.introStep == qManager.introEndStep && !noControlAtDisable) controller.hasControl = true;
    }
}
