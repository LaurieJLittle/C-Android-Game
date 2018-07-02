using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColour : MonoBehaviour {
    

	void Start()
    {
		if (!GameControl.control.showIntro)
            changeColour(Color.red);
        else
            changeColour(Color.green);
    }
	

    public void toggleCol()
    {
        if (GetComponent<Button>().colors.normalColor == Color.green)
            changeColour(Color.red);
        else
            changeColour(Color.green);
    }

    void changeColour(Color newColour)
    {
        ColorBlock buttonColours = GetComponent<Button>().colors;
        buttonColours.normalColor = newColour;
        buttonColours.highlightedColor = newColour;
        buttonColours.pressedColor = newColour;
        GetComponent<Button>().colors = buttonColours;
    }
}
