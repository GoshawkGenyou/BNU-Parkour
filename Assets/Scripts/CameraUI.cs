using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUI : MonoBehaviour
{

    public RectTransform uiElement;

    // Start is called before the first frame update
    void Start()
    {
        uiElement.anchorMin = new Vector2(1, 1);
        uiElement.anchorMax = new Vector2(1, 1);
        uiElement.pivot = new Vector2(1, 1);
        uiElement.anchoredPosition = new Vector2(-20, -20);
    }

}
