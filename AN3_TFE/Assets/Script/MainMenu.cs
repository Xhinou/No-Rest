using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image
        fadeScreen,
        title;
    public Button[] buttons = new Button[2];
    public float fadeDuration = 1f;
    public Text[] textToFade;
    QuestManager qManager;
    GameObject scriptSystem;

    private void Awake()
    {
        scriptSystem = GameObject.Find("ScriptSystem");
        qManager = scriptSystem.GetComponent<QuestManager>();

    }

    public void RunGame()
    {
        print("PLAY");
        StartCoroutine(FadeInAndPlay());
    }

    IEnumerator FadeInAndPlay()
    {
        Debug.Log("Coroutine Started");
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;
        for (int i = 0; i < textToFade.Length; i++)
            textToFade[i].CrossFadeAlpha(0f, fadeDuration, false);          
        title.CrossFadeAlpha(0f, fadeDuration, false);
        fadeScreen.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(2);
        qManager.RunIntro(qManager.sceneID);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
