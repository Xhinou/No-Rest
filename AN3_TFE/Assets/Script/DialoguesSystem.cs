﻿using UnityEngine;
using UnityEngine.UI;

public class DialoguesSystem : MonoBehaviour
{

    public Text theText;
    public TextAsset textFile;
    public string[]
        textLines, textAsk;
    public int
        currentLine,
        endAtLine;
    public GameObject[] buttons;
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
            if (controller.canSkipDial)
            {
                if (mousePressed && currentLine >= endAtLine)
                {
                    mousePressed = false;
                    if (endDialog)
                    {
                        currentLine = 0;
                    }
                    else
                    {
                        for (int i = 0; i < buttons.Length; i++)
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

    void ResumeDialog()
    {
        mousePressed = true;
    }

    void OnEnable()
    {
        currentLine = 0;
        buttonResume.SetActive(true);
    }

    public void DisplayText(int npcID, int step)
    {
        textBox.SetActive(true);
        int order = 0;
        var textFile = Resources.Load("Assets/Texts/" + npcID + step + order + ".txt") as TextAsset;
        if (textFile != null)
            textLines = (textFile.text.Split('\n'));
        if (endAtLine == 0)
            endAtLine = textLines.Length - 1;
        theText.text = textLines[currentLine];
        isTextboxActive = true;
    }
}