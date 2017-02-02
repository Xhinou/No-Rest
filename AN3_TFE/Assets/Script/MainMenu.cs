using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public int sceneID = 0;
    public GameObject fadeScreen;
    public Button[] buttons = new Button[2];
    //Color goalColor = new Color(0,0,0,255);
    public float fadeDuration = 1f;

    void Start()
    {
    }

    public void RunGame()
    {
        StartCoroutine(FadeInAndPlay());
        SceneManager.LoadScene (1);
    }

    IEnumerator FadeInAndPlay()
    {
        Debug.Log("Coroutine Started");
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        fadeScreen.GetComponent<Image>().CrossFadeAlpha(255f, fadeDuration, false);
        //Debug.Log(fadeScreen.GetComponent<Image>().color);
        yield return new WaitForSeconds(2);
        Debug.Log("PLAY");
    }

    public void ExitGame()
    {
        /*fadeScreen.GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        while (fadeScreen.GetComponent<Image>().color.a != 1f)
        {
            return;
        }*/
        Debug.Log("EXIT");
        Application.Quit();
    }
}
