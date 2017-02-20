using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Camera cameraToLookAt;
    public bool noControlAtDisable = false;
    public GameObject
        scriptSystem,
        player;
    QuestManager qManager;
    CharacterClickingController controller;

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
        transform.rotation = Quaternion.LookRotation(transform.position - cameraToLookAt.transform.position);
    }

    void OnEnable()
    {
        controller.hasControl = false;
        controller.agent.ResetPath();
    }

    void OnDisable()
    {
        if (qManager.introStep == qManager.introEndStep && !noControlAtDisable)
            controller.hasControl = true;
    }
}
