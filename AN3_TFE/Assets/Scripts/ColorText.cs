using UnityEngine.UI;
using UnityEngine;

public class ColorText : MonoBehaviour {

    Button parentButton;
    public Text childText;

	void Start () {
        parentButton = GetComponent<Button>();
        if (childText == null)
            Debug.Log("No child text found!");
	}

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            childText.color = parentButton.colors.pressedColor;
        else
            childText.color = parentButton.colors.highlightedColor;
    }

    private void OnMouseExit()
    {
        childText.color = parentButton.colors.normalColor;
    }
}
