using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    public void SetRandomColour()
    {
        // create a new random colour
        Color random = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        // set material to the random colour
        GetComponent<Renderer>().material.color = random;
    }
}
