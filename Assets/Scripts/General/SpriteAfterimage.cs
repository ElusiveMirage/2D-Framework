using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAfterimage : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private float alpha;
    [SerializeField] private float alphaMult;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        alpha *= alphaMult;
        color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, alpha);
        GetComponent<SpriteRenderer>().color = color;
    }

    public void Initialize(Sprite sprite, bool flip)
    {
        GetComponent<SpriteRenderer>().flipX = flip;
        GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(gameObject, lifetime);
    }
}
