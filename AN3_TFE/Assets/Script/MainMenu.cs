using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public int sceneID = 0;
    public GameObject fadeScreen;
    public Button[] buttons = new Button[2];
    public float fadeDuration = 1f;

    public void RunGame()
    {
        StartCoroutine(FadeInAndPlay());        
    }

    IEnumerator FadeInAndPlay()
    {
        Debug.Log("Coroutine Started");
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;
        fadeScreen.GetComponent<Image>().CrossFadeAlpha(255f, fadeDuration, false);
        yield return new WaitForSeconds(2);
        Debug.Log("PLAY");
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
