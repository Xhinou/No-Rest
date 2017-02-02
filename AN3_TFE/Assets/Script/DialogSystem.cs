using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{

    public GameObject textBox;
    public Text theText;
    public TextAsset textFile;
    public string[] textLines;
    public string[] textAsk;
    public int currentLine;
    public int endAtLine;
    public GameObject[] buttons;
    public GameObject buttonResume;
    public bool endDialog;
    public GameObject canvas;
    public static bool mousePressed;

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

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
        buttonResume = GameObject.Find("ButtonResume");
    }

    void Update()
    {
        theText.text = textLines[currentLine];
        if (controller.canSkipDial)
        {
            if (mousePressed && currentLine >= endAtLine)
            {
                mousePressed = false;
                //buttonResume.SetActive (false);
                if (endDialog)
                {
                    buttonResume.SetActive(false);
                    if (qManager.introStep != qManager.introEndStep)
                        qManager.introStep++;
                    currentLine = 0;
                    canvas.SetActive(false);
                    //StartCoroutine(IntroRunning (scriptSystem.GetComponent<QuestManager> ().introID));
                }
                else
                {
                    buttonResume.SetActive(false);
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        //if (i == 2) break;
                        buttons[i].SetActive(true);
                    }
                }
            }
            if (mousePressed && currentLine < endAtLine)
            {
                mousePressed = false;
                currentLine++;
            }
        }
    }

    void ResumeDialog()
    {
        mousePressed = true;
    }

    void OnEnable()
    {
        currentLine = 0;
        buttonResume.SetActive(true);
    }

}