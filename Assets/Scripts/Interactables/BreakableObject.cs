using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IBreakable
{
    [SerializeField] private int dropChance;
    [SerializeField] private GameObject itemDrop;

    private float lifetime = 1f;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BreakObject()
    {
        myAnimator.SetTrigger("Break");
        int r = Random.Range(1, 100);

        if (r <= dropChance)
        {
            Invoke("SpawnItem", lifetime);          
        }

        Destroy(gameObject, lifetime);
    }

    private void SpawnItem()
    {
        Instantiate(itemDrop, transform.position, Quaternion.identity);
    }
}
