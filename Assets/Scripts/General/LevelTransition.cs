using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string nextLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneLoader.Instance.LoadScene(nextLevel);
    }
}
