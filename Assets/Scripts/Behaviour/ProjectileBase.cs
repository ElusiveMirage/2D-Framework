using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask collisionLayerMask;
    //=====================================================//
    private Vector3 direction;
    private GameObject projectileOrigin;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, projectileLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void Initialize(Vector3 dir, Vector3 angle, Vector3 spawnPos, GameObject origin)
    {
        direction = dir;

        transform.eulerAngles = angle;

        transform.position = spawnPos;

        projectileOrigin = origin;
    }

    public void Initialize(Vector3 dir, Vector3 spawnPos, float dmg, GameObject origin)
    {
        direction = dir.normalized;

        transform.eulerAngles = new Vector3(0, 0, Utility.HelperFunctions.GetAngleFromVector(direction));

        damage = dmg;

        transform.position = spawnPos;

        projectileOrigin = origin;
    }

    public void Initialize(Vector3 dir, Vector3 angle, Vector3 spawnPos, float dmg, GameObject origin)
    {
        direction = dir;
        damage = dmg;

        transform.eulerAngles = angle;

        transform.position = spawnPos;

        projectileOrigin = origin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isBreakable = collision.gameObject.GetComponent<IBreakable>();
        if (isBreakable != null)
        {
            isBreakable.BreakObject();
        }

        var isDamageable = collision.gameObject.GetComponent<IDamageable>();
        if (isDamageable != null && collision.gameObject != projectileOrigin)
        {
            isDamageable.InflictDamage(damage);

            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform);
            }

            Destroy(gameObject);
        }
        
        if ((collisionLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
