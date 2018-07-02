using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMethods : MonoBehaviour {


    protected void changeButtonSize(float[] portraitRectBounds, float[] landscapeRectBounds)
    {
        RectTransform Rect = GetComponent<RectTransform>();

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            ChangeRectDimensions(Rect, portraitRectBounds);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            ChangeRectDimensions(Rect, landscapeRectBounds);
        }
    }


    protected void ChangeRectDimensions(RectTransform rect, float[] bounds)
    {
        float minX = bounds[0];
        float maxX = bounds[1];
        float minY = bounds[2];
        float maxY = bounds[3];

        rect.anchorMin = new Vector2(minX, minY);
        rect.anchorMax = new Vector2(maxX, maxY);
    }
}
