using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer_AIMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 xDir = new Vector3(1, 0, 0);
    //===============================================================//
    private bool facingRight = true;
    //===============================================================//
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();       
        myCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveInDirection(Vector2 dir)
    {
        Vector2 vel = new Vector2(moveSpeed, myRigidbody.velocity.y) * dir;
        myRigidbody.velocity = vel;

        if (dir.x == 1)
        {
            facingRight = true;
            FlipSprite();
        }
        else if (dir.x == -1)
        {
            facingRight = false;
            FlipSprite();
        }
    }

    public void StopMovement()
    {
        myRigidbody.velocity = Vector2.zero;
    }

    public void FlipSprite()
    {
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
            xDir = new Vector3(1, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0f);
            xDir = new Vector3(-1, 0, 0);
        }

    }
}
