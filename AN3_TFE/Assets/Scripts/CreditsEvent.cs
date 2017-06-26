using UnityEngine;

public class CreditsEvent : MonoBehaviour {

    public MainMenu menu;

    public void EndCredits()
    {
        if (!QuestManager.gameOver)
        {
            menu.credits.SetActive(false);
            menu.mainMenu.SetActive(true);
        }
        else
        {
            menu.credits.SetActive(false);
            menu.tyMenu.SetActive(true);
        }
    }
}
