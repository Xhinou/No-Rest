using UnityEngine;
using UnityEngine.UI;

public class DialoguesSystem : MonoBehaviour
{
    public Text theText;
    TextAsset textFile;
    string[]
        textLines;
    int
        currentLine = 0,
        endAtLine,
        sceneID,
        npcID,
        step,
        order,
        choicesCount;
    public GameObject[] buttons = new GameObject[3];
    public GameObject
        textBox,
        player,
        scriptSystem;
    public bool isDisabled;
    [Range(0,2)]public int karmaMod = 0;
    CharacterClickingController controller;
    QuestManager qManager;
    Button resume;
    Camera mainCam;
    [HideInInspector] public Camera dialCam;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        qManager = scriptSystem.GetComponent<QuestManager>();
        mainCam = Camera.main;
        textBox.SetActive(false);
        isDisabled = true;
        resume = textBox.GetComponent<Button>();
    }

    void Update()
    {
        if (controller.canSkipDial && theText.enabled == true)
            resume.interactable = true;
        else
            resume.interactable = false;
    }

    public void DisplayText(int _sceneID, int _npcID, int _step)
    {
        sceneID = _sceneID;
        npcID = _npcID;
        step = _step;
        dialCam = GameObject.Find("NPC" + npcID + "Cam" + step).GetComponent<Camera>();
        mainCam.enabled = false;
        dialCam.enabled = true;
        textBox.SetActive(true);
        isDisabled = false;
        order = 0;
        LoadFiles(0);
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        UpdateLine();
    }

    public void ResumeDialog()
    {
        currentLine++;      
        if (currentLine > endAtLine)
        {
            if (choicesCount == 0)
            {
                EndDialog();
            }
            else
            {
                theText.enabled = false;
                resume.interactable = false;
                for (int i = 0; i < choicesCount; i++)
                    buttons[i].SetActive(true);
            }
        } else
            UpdateLine();
    }
    
    public void NextDialog(int choice)
    {
        order += 1;
        LoadFiles(choice);
        currentLine = 0;
        theText.enabled = true;
        resume.interactable = true;
        for (int i = 0; i < choicesCount; i++)
            buttons[i].SetActive(false);
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        UpdateLine();
    }

    public void EndDialog()
    {
        currentLine = 0;
        textBox.SetActive(false);
        isDisabled = true;
        dialCam.enabled = false;
        mainCam.enabled = true;
        if (!qManager.intro)
            controller.hasControl = true;
    }

    void LoadFiles(int choice)
    {
        string strRange = "";
        for (int i = 0; i < order; i++)
            strRange += "0";
        string choiceString = choice.ToString(strRange);
        if (order == 0)
            choiceString = "";
        textFile = null;
        for (int i = 0; i < 3; i++)
        {
            textFile = Resources.Load("Texts/" + sceneID + "_" + npcID + "_" + step + "_" + order + choiceString + "-" + i) as TextAsset;
            if (textFile != null)
                break;
            else if (i > 3)
            {
                Debug.Log("Some files don't have a right name. Make sure using the template specified in the README");
                break;
            }
        }
        textLines = (textFile.text.Split('\n'));
        endAtLine = textLines.Length - 1;
    }

    public void ForceLine(int line, int? modLine, int? choice)
    {
        currentLine=line;
        if (modLine != null)
            endAtLine = currentLine + (int)modLine;
        if (choice != null)
            choicesCount = (int)choice;
        UpdateLine();
    }

    void UpdateLine()
    {
        theText.text = textLines[currentLine];
    }  

    public void KarmaMod ()
    {
        if (karmaMod == 1)
            qManager.karma++;
        else if (karmaMod == 2)
            qManager.karma--;
    }

    public void OrderMod(int mod)
    {
        order += mod;
    }
}