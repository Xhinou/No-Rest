using UnityEngine;
using UnityEngine.UI;

public class DialoguesSystem : MonoBehaviour
{
    public static string language;
    public Text theText;
    [HideInInspector] TextAsset
        textFile,
        buttonFile;
    private string modDial, modDialButtons, modDialPrevString;
    private int prevOrder;
    string[]
        textLines,
        buttonLines;
    int
        currentLine = 0,
        endAtLine,
        sceneID,
        npcID,
        step,
        order,
        choicesCount;
    public GameObject[]
        buttons = new GameObject[3],
        buttonTexts = new GameObject[3];
    public GameObject
        textBox,
        player,
        scriptSystem;
    [HideInInspector] public bool isNewLine = false;
    public bool
        isDisabled,
        isNextDial = false;
    public bool toDial;
    [HideInInspector] /*&&*/ [Range(0,2)]
    public int
        karmaMod = 0,
        lastChoice;
    CharacterClickingController controller;
    QuestManager qManager;
    Button resume;
    GameObject dialCamObj;
    Camera
        mainCam,
        dialCam;
    Light lightCam;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        scriptSystem = GameObject.Find("ScriptSystem");
        controller = player.GetComponent<CharacterClickingController>();
        qManager = scriptSystem.GetComponent<QuestManager>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
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

    public void NewLineTrue()
    {
        isNewLine = true;
    }

    public void DisplayText(int _sceneID, int _npcID, int _step, string cam, bool hasLightCam)
    {
        sceneID = _sceneID;
        npcID = _npcID;
        step = _step;
        dialCamObj = GameObject.Find(cam);
        dialCam = dialCamObj.GetComponent<Camera>();
        if (hasLightCam)
            lightCam = dialCamObj.GetComponentInChildren<Light>();
        controller.hasControl = false;
        if (controller.agent != null)
            controller.agent.ResetPath();
        mainCam.enabled = false;
        dialCam.enabled = true;
        if (hasLightCam)
            lightCam.enabled = true;
        textBox.SetActive(true);
        isDisabled = false;
        order = 0;
        LoadFiles(-1);
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        UpdateLine();
        GameObject npcTalk = GameObject.FindWithTag("closeToPlayer");
        if (npcTalk != null)
        {
            if (npcTalk.GetComponent<NpcManager>().generic)
                npcTalk.GetComponent<Animator>().Play("Talk");
        }
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
                {
                    buttons[i].SetActive(true);
                    buttonTexts[i].GetComponent<Text>().text = buttonLines[i];
                }
            }
        } else
            UpdateLine();
    }
    
    public void NextDialog(int choice)
    {
        isNextDial = true;
        order += 1;
        lastChoice = choice;
        for (int i = 0; i < choicesCount; i++)
            buttons[i].SetActive(false);
        LoadFiles(choice);
        currentLine = 0;
        theText.enabled = true;
        resume.interactable = true;
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
        UpdateLine();
    }

    public void EndDialog()
    {
        currentLine = 0;
        prevChoiceString = "";
        choiceString = "";
        modDialPrevString = "";
        textBox.SetActive(false);
        isDisabled = true;
        dialCam.enabled = false;
        if (lightCam != null)
        {
            if (lightCam.enabled == true)
                lightCam.enabled = false;
        }
        mainCam.enabled = true;
        if (!qManager.intro)
            controller.hasControl = true;
    }

    private string
        choiceString,
        prevChoiceString;

    void LoadFiles(int choice)
    {
       if (choiceString != "")
            prevChoiceString = choiceString;
        if (choice != -1)
            choiceString = prevChoiceString + choice;
        if (order == 0)
            choiceString = "";
        textFile = null;
        for (int i = 0; i < 4; i++)
        {
            textFile = Resources.Load("Texts/" + language +  sceneID + "_" + npcID + "_" + step + "_" + order + choiceString + "-" + i) as TextAsset;
            if (textFile != null)
            {
                if (i > 0)
                {
                    buttonFile = Resources.Load("Texts/" + language + sceneID + "_" + npcID + "_" + step + "_" + order + choiceString + "-buttons") as TextAsset;
                    buttonLines = buttonFile.text.Split('\n');
                    if (buttonFile == null)
                    {
                        buttonFile = Resources.Load("Texts/" + language + sceneID + "_" + npcID + "_" + step + "_" + order + choiceString + "-buttons") as TextAsset;
                        if (buttonFile == null)
                            Debug.Log("Some files don't have a right name. Make sure you use the template specified in the README");
                    }
                }
                else
                    buttonFile = null;
                break;
            }
        }
        if (textFile == null)
        {
            if (!toDial)
                Debug.Log("Some files don't have a right name. Make sure you use the template specified in the README");
            else
            {
                textFile = Resources.Load("Texts/" + modDial) as TextAsset;
                string[] fileName;
                fileName = modDial.Split('-');
                choicesCount = int.Parse(fileName[1]);
                if (choicesCount != 0)
                {
                    buttonFile = Resources.Load("Texts/" + fileName[0] + "-buttons") as TextAsset;
                    buttonLines = buttonFile.text.Split('\n');
                }
                else
                    buttonFile = null;
                choiceString = modDialPrevString;
                order = prevOrder;
                toDial = false;
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
        isNextDial = false;
    }  

    public void KarmaMod ()
    {
        if (karmaMod == 1)
            QuestManager.karma++;
        else if (karmaMod == 2)
            QuestManager.karma--;
    }

    public void SetToDial(string mod, int newOrder, string prev)
    {
        if (mod != "")
        {
            toDial = true;
            modDialPrevString = prev;
            prevOrder = newOrder;
            modDial = language + mod;
        }
        else
            toDial = false;
    }

    public void ChangeLanguage(string _language)
    {
        language = _language;
        if (language == "EN_")
            print("Language is: English");
        else if (language == "FR_")
            print("Language is: Français");
    }
}