using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public string textString;
    [SerializeField] private float lifetime;
    [SerializeField] private float awakeTime;

    // Start is called before the first frame update
    void Start()
    {
        awakeTime = Time.time;
        GetComponentInChildren<TextMeshProUGUI>().text = textString;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < awakeTime + lifetime)
        {
            GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0.0f, 1f, false);
        }
        else
        {
            Destroy(gameObject);
        }
        transform.Translate(0, 1f * Time.deltaTime, 0);
    }
}
