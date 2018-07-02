using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreText : UIMethods {

    Text scoreText;
    public float[] portraitRectBounds = { 0.01f, 0.49f, 0.93f, 0.99f };
    public float[] landscapeRectBounds = { 0.01f, 0.49f, 0.85f, 0.99f };

    void Start ()
    {
        scoreText = GetComponent<Text>();
    }
	
	void Update ()
    {
        int intScore = (int) Player.player.score;
        scoreText.text =  string.Format("Score {0}", intScore) ;
    }

    void OnRectTransformDimensionsChange()
    {
        changeButtonSize(portraitRectBounds, landscapeRectBounds);
    }
}
