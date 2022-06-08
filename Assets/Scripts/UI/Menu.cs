using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T>:Menu where T : Menu<T>
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    public virtual void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    public static void Open()
    {
        if(MenuManager.Instance != null && Instance != null)
        {
            MenuManager.Instance.OpenMenu(Instance);
        }
    }
}

public abstract class Menu : MonoBehaviour
{
    public virtual void Back()
    {
        MenuManager.Instance.CloseMenu();
    }
}

