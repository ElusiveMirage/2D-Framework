using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBase : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float objectLifetime;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask collisionLayerMask;
    //[SerializeField] private Collider2D[] hitEntities = new Collider2D[0];
    //=====================================================//
    private Vector3 direction;
    private GameObject attackOrigin;
    private int objectsHit;
    private int maxObjectsHit;

    //TODO : Merge with projectile base this is fucking stupid why did i do it like this
    // Start is called before the first frame update
    void Start()
    {
        objectsHit = 0;
        Destroy(gameObject, objectLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector3 dir, Vector3 angle, Vector3 spawnPos, GameObject origin)
    {
        direction = dir;

        transform.eulerAngles = angle;

        transform.position = spawnPos;

        attackOrigin = origin;
    }

    public void Initialize(Vector3 dir, Vector3 spawnPos, float dmg, GameObject origin)
    {
        direction = dir.normalized;

        transform.eulerAngles = new Vector3(0, 0, Utility.HelperFunctions.GetAngleFromVector(direction));

        damage = dmg;

        transform.position = spawnPos;

        attackOrigin = origin;
    }

    public void Initialize(Vector3 dir, Vector3 angle, Vector3 spawnPos, float dmg, int maxHits, GameObject origin)
    {
        direction = dir;
        damage = dmg;

        transform.eulerAngles = angle;

        transform.position = spawnPos;

        attackOrigin = origin;

        maxObjectsHit = maxHits;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(objectsHit < maxObjectsHit)
        {
            var isBreakable = collision.gameObject.GetComponent<IBreakable>();
            if (isBreakable != null)
            {
                isBreakable.BreakObject();
            }

            var isDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (isDamageable != null && collision.gameObject != attackOrigin)
            {
                isDamageable.InflictDamage(damage);

                if (hitEffect != null)
                {
                    Instantiate(hitEffect, collision.transform);
                }
                objectsHit++;
            }
        }       
    }
}
