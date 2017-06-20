using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject
        particles,
        menuCanvas,
        credits,
        mainMenu;
    public Image
        fadeScreenStart,
        fadeScreenPlay,
        title;
    public GameObject[] buttons;
    public Button[] theButtons;
    public float fadeDuration, creditsSpeed;
    public Text[] textToFade;
    QuestManager qManager;
    GameObject scriptSystem;
    public Animator creditsAnimator;

    private void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        qManager = scriptSystem.GetComponent<QuestManager>();
        theButtons = new Button[buttons.Length];
        textToFade = new Text[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            theButtons[i] = buttons[i].GetComponent<Button>();
            textToFade[i] = buttons[i].GetComponent<Text>();
        }
    }

    private void Start()
    {
        if (QuestManager.karmaStep == 0)
        {
            StartCoroutine(FadeOutStart());
            if (QuestManager.gameOver)
            {
                mainMenu.SetActive(false);
                credits.SetActive(true);
            }
        }
        else
        {
            menuCanvas.SetActive(false);
            particles.SetActive(false);
            qManager.RunIntro(qManager.sceneID);
        }
        //qManager.player.SetActive(false);
    }

    private void Update()
    {
        if (credits.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
                creditsAnimator.speed = creditsSpeed;
            else if (Input.GetMouseButtonUp(0))
                creditsAnimator.speed = 1;
        }
    }

    public void RunGame()
    {
        print("PLAY");
        QuestManager.karma = -1;
        StartCoroutine(FadeOutAndPlay());
    }

    IEnumerator FadeOutStart()
    {
        fadeScreenStart.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(1);
        fadeScreenStart.color = new Color(0,0,0,0);
        fadeScreenStart.raycastTarget = false;
    }

    IEnumerator FadeOutAndPlay()
    {
        Debug.Log("Coroutine Started");
        for (int i = 0; i < buttons.Length; i++)
        {
            theButtons[i].interactable = false;
            textToFade[i].CrossFadeAlpha(0f, fadeDuration, false);
        }        
        title.CrossFadeAlpha(0f, fadeDuration, false);
        fadeScreenPlay.CrossFadeAlpha(0f, fadeDuration, false);
        particles.SetActive(false);
        StartCoroutine(qManager.CameraZoom(true));
        yield return new WaitForSeconds(2);
        menuCanvas.SetActive(false);
        qManager.RunIntro(qManager.sceneID);
    }

   /* IEnumerator FadeInCredits()
    {
        fadeScreenStart.canvasRenderer.SetAlpha(1f);
        fadeScreenStart.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(1);
        fadeScreenStart.color = new Color(0, 0, 0, 1);
        credits.SetActive(true);
    }

    public void ShowCredits()
    {
        StartCoroutine(FadeInCredits());
    }*/

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
