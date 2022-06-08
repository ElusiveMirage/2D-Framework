using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect_Base : MonoBehaviour
{
    [SerializeField] private float effectLifetime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, effectLifetime);
    }
}
