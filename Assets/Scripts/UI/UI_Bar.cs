using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    private void Start()
    {
       
    }

    private void Update()
    {
        float target = minValue / maxValue;
        barImage.fillAmount = Mathf.MoveTowards(barImage.fillAmount, target, 2 * Time.deltaTime);
    }

    public void SetMinValue(float value)
    {
        minValue = value;
    }

    public void SetMaxValue(float value)
    {
        maxValue = value;
    }
}
