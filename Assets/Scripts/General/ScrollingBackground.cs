using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x, y;
    [SerializeField] private bool scrollBackground;

    void Update()
    {
        if(scrollBackground)
        {
            image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
        }
    }

    public void ToggleScroll(bool b)
    {
        scrollBackground = b;
    }
}
