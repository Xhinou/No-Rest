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
        order;
    public int choicesCount;
    public GameObject[] buttons = new GameObject[3];
    public GameObject
        textBox,
        player;
    bool isTextboxActive;
    CharacterClickingController controller;
        
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterClickingController>();
    }

    void Update()
    {
        if (isTextboxActive)
            theText.text = textLines[currentLine];
    }

    public void ResumeDialog()
    {        
        if (choicesCount == 0)
        {
            currentLine = 0;
            textBox.SetActive(false);
            isTextboxActive = false;
        }
        if (currentLine >= endAtLine)
        {
            theText.enabled = false;
            for (int i = 0; i < choicesCount; i++)
                buttons[i].SetActive(true);
        } else if (currentLine < endAtLine)
            currentLine++;
    }

    public void DisplayText(int _sceneID, int _npcID, int _step)
    {
        sceneID = _sceneID;
        npcID = _npcID;
        step = _step;
        textBox.SetActive(true);
        if (controller.canSkipDial)
            textBox.GetComponent<Button>().interactable = true;
        order = 0;
        LoadFiles(0);
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);      
        isTextboxActive = true;
    }

    public void NextDialog(int choice)
    {
        order += 1;
        LoadFiles(choice);
        currentLine = 0;
        theText.enabled = true;       
        for (int i = 0; i < choicesCount; i++)
            buttons[i].SetActive(false);
        string[] fileName;
        fileName = textFile.name.Split('-');
        choicesCount = int.Parse(fileName[1]);
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
        }
        textLines = (textFile.text.Split('\n'));
        if (endAtLine == 0)
            endAtLine = textLines.Length - 1;
    }
}