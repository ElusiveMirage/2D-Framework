using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_Example : MonoBehaviour
{
    public float HP;
    public float maxHP;

    private static PlayerData_Example _instance;

    public static PlayerData_Example Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //resource regen example
        //if (Time.time > lastRegenTick + regenTick)
        //{
        //    if (HP + HP_Regen < maxHP)
        //    {
        //        HP += HP_Regen;
        //    }
        //    else
        //    {
        //        HP = maxHP;
        //    }

        //    lastRegenTick = Time.time;
        //}
    }   
}
