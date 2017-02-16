using UnityEngine;
using UnityEngine.UI;

public class DialoguesSystem : MonoBehaviour
{

    public Text theText;
    public TextAsset textFile;
    public string[]
        textLines;
    public int
        currentLine,
        endAtLine;
    public GameObject[] buttons = new GameObject[3];
    public GameObject
        textBox,
        buttonResume,
        canvas,
        scriptSystem,
        player;
    public bool
        endDialog,
        isTextboxActive;
    public static bool mousePressed;
   // QuestManager qManager;
    CharacterClickingController controller;
    public int
        sceneID,
        npcID,
        step,
        order,
        choicesCount;

    void Awake()
    {

       // scriptSystem = GameObject.Find("ScriptSystem");
       // qManager = scriptSystem.GetComponent<QuestManager>();
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Update()
    {
        if (isTextboxActive)
        {
            theText.text = textLines[currentLine];
            if (controller.canSkipDial)
            {
                if (mousePressed && currentLine >= endAtLine)
                {
                    mousePressed = false;
                    else
                    {
                        theText.enabled = false;
                        for (int i = 0; i < choicesCount; i++)
                        {
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
    }

    public void ResumeDialog()
    {
        if (endDialog)
        {
            currentLine = 0;
            textBox.SetActive(false);
            isTextboxActive = false;
        } else
        mousePressed = true;
    }

    void OnEnable()
    {
        currentLine = 0;
        //buttonResume.SetActive(true);
    }

    public void DisplayText(int _sceneID, int _npcID, int _step)
    {
        sceneID = _sceneID;
        npcID = _npcID;
        step = _step;
        textBox.SetActive(true);
        textBox.GetComponent<Button>().interactable = true;
        order = 0;
        textFile = null;
        //textFile = Resources.Load("Texts/" + sceneID + "_" + npcID + "_" + step + "_" + order + 0) as TextAsset;
        for (int i = 0; i < 3; i++) {
            textFile = Resources.Load("Texts/" + sceneID + "_" + npcID + "_" + step + "_" + order + "-" + i) as TextAsset;
            if (textFile != null)
                break;
        }
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        if (choicesCount == 0)
            endDialog = true;
        if (textFile != null)
            textLines = (textFile.text.Split('\n'));
        if (endAtLine == 0)
            endAtLine = textLines.Length - 1;
       // theText.text = textLines[currentLine];
        isTextboxActive = true;
    }


    //A REFAIRE, TROUVER UN MOYEN DE LOAD TEXT DANS L'ORDRE
    public void nextDialog(int choice)
    {
        order += 1;
        string strRange = "";
        for (int i = 0; i < order; i++)
            strRange += "0";
        string choiceString = choice.ToString(strRange);
        textFile = null;
        for (int i = 0; i < 3; i++)
        {
            textFile = Resources.Load("Texts/" + sceneID + "_" + npcID + "_" + step + "_" + order + choiceString + "-" + i) as TextAsset;
            if (textFile != null)
                break;
        }
        //textFile = Resources.Load("Texts/" + sceneID + "_" + npcID + "_" + step + "_" + order + choice + anyChar) as TextAsset;
        if (textFile != null)
            textLines = (textFile.text.Split('\n'));
        if (endAtLine == 0)
            endAtLine = textLines.Length - 1;
        currentLine = 0;
        theText.enabled = true;       
        for (int i = 0; i < choicesCount; i++)
        {
            print(choicesCount);
            buttons[i].SetActive(false);
        }
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        if (choicesCount == 0)
            endDialog = true;
    }
}