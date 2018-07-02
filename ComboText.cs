using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ComboText : UIMethods {

    Text combo;
    public float[] portraitRectBounds = { 0.51f, 0.99f, 0.93f, 0.99f };
    public float[] landscapeRectBounds = { 0.51f, 0.99f, 0.85f, 0.99f };

    void Start()
    {
        combo = GetComponent<Text>();
    }
    
    void Update()
    {
        combo.text = string.Format("Combo: {0}", Player.player.nuggetcombo);
    }

    void OnRectTransformDimensionsChange()
    {
        changeButtonSize(portraitRectBounds, landscapeRectBounds);
    }
}
