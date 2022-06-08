using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour, IDamageable
{
    //===============================================//
    [SerializeField] private bool facingRight;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool ungrounded = false;
    [SerializeField] private bool coyotePossible = false;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float timeLeftGrounded;
    //===============================================//
    [Header("Horizontal Movement")]
    //===============================================//
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveAcceleration;
    //===============================================//   
    [Header("Vertical Movement")]
    //===============================================//
    [SerializeField] private bool jumping = false;
    [SerializeField] private int jumpCharges;
    [SerializeField] private int jumpCount;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpFalloff;
    [SerializeField] private float coyoteTime = 0.2f;
    [Header("Dashing")]
    //===============================================//
    [SerializeField] private bool dashing;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashExecutionTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private Vector3 dashDir;
    //===============================================//
    [Header("Input Checks")]
    //===============================================//
    [SerializeField] private bool allowInput = true;
    [SerializeField] private bool dashExecuted = false;
    [SerializeField] private bool jumpExecuted = false;
    [SerializeField] private bool moveRight = false;
    [SerializeField] private bool moveLeft = false;
    //===============================================//
    [SerializeField] private LayerMask collisionLayerMask;
    //===============================================//
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    //===============================================//
    private Vector2 inputAxis = new Vector2(0, 0);
    private Vector2 moveDistance = new Vector2(0, 0);
    //===============================================//

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.4f), 0.2f, collisionLayerMask);

        if (!grounded && !ungrounded)
        {
            ungrounded = true;
            coyotePossible = true;
            timeLeftGrounded = Time.time;
        }

        if (grounded)
        {
            jumpCount = 0;
            ungrounded = false;
        }

        if (coyotePossible)
        {
            if (Time.time > timeLeftGrounded + coyoteTime)
            {
                ungrounded = false;
                coyotePossible = false;
            }
        }

        if (grounded && jumping)
        {
            jumping = false;
            //myAnimator.SetBool("IsFalling", false);
            //myAnimator.SetBool("IsJumping", jumping);
        }

        if (grounded && !jumping)
        {
            //myAnimator.SetBool("IsFalling", false);
        }

        if (!grounded && jumping)
        {
            //myAnimator.SetBool("IsFalling", false);
        }

        if (!grounded && !jumping)
        {
            //myAnimator.SetBool("IsFalling", true);
        }

        HorizontalMovement();
        DashMovement();
        VerticalMovement();
    }

    private void OnDrawGizmos()
    {

    }

    private void DrawCircleGizmo()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -0.4f), 0.2f);
    }

    private void DrawCubeGizmo()
    {
        Gizmos.color = Color.red;
        if(facingRight)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(1f, 0.25f), new Vector3(2, 1.5f, 0));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(-1f, 0.25f), new Vector3(2, 1.5f, 0));
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if(allowInput)
        {
            Vector2 xDir = context.ReadValue<Vector2>();
            if (context.performed && xDir.x > 0)
            {
                inputAxis = context.ReadValue<Vector2>();
                facingRight = true;
                moveRight = true;
                moveLeft = false;
            }
            if (context.performed && xDir.x < 0)
            {
                inputAxis = context.ReadValue<Vector2>();
                facingRight = false;
                moveRight = false;
                moveLeft = true;
            }
            if (context.canceled)
            {
                inputAxis = Vector2.zero;
                moveRight = false;
                moveLeft = false;
            }
        }       
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (allowInput)
        {
            if (context.performed && !jumpExecuted && grounded)
            {
                jumpExecuted = true;
            }
            else if (context.performed && !jumpExecuted && coyotePossible)
            {
                jumpExecuted = true;
            }

            if (context.canceled)
            {
                jumpExecuted = false;
            }
        }       
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (allowInput)
        {
            if (context.performed && !dashing && !dashExecuted && Time.time > dashExecutionTime + dashCooldown)
            {
                dashExecuted = true;
            }
            if (context.canceled)
            {
                dashExecuted = false;
            }
        }
    }

    private void HorizontalMovement()
    {
        if(!dashing)
        {
            var acceleration = grounded ? moveAcceleration : moveAcceleration * 0.5f;

            if (moveRight)
            {
                if (myRigidbody2D.velocity.x < 0)
                {
                    moveDistance.x = 0;
                }
                moveDistance.x = Mathf.MoveTowards(moveDistance.x, 1, acceleration * Time.deltaTime);
            }
            else if (moveLeft)
            {
                if (myRigidbody2D.velocity.x > 0 && moveLeft)
                {
                    moveDistance.x = 0;
                }
                moveDistance.x = Mathf.MoveTowards(moveDistance.x, -1, acceleration * Time.deltaTime);
            }
            else
            {
                moveDistance.x = Mathf.MoveTowards(moveDistance.x, 0, acceleration * 2 * Time.deltaTime);
            }

            Vector3 targetVelocity = new Vector3(moveDistance.x * moveSpeed, myRigidbody2D.velocity.y);
            myRigidbody2D.velocity = Vector3.MoveTowards(myRigidbody2D.velocity, targetVelocity, 100 * Time.deltaTime);

            //myAnimator.SetBool("IsRunning", moveDistance.x != 0 && grounded);
        }      
    }

    private void VerticalMovement()
    {
        if(jumpExecuted && grounded && jumpCount < jumpCharges)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpHeight);
            jumpExecuted = false;
            jumping = true;
            jumpCount++;
            //myAnimator.SetTrigger("JumpTrigger");
            //myAnimator.SetBool("IsJumping", jumping);
        }
        else if (jumpExecuted && coyotePossible && jumpCount < jumpCharges)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpHeight);
            jumpExecuted = false;            
            jumping = true;
            jumpCount++;

            if(jumpCount == jumpCharges)
            {
                coyotePossible = false;
            }    
            //myAnimator.SetTrigger("JumpTrigger");
            //myAnimator.SetBool("IsJumping", jumping);
        }

        if (!grounded)
        {
            if (myRigidbody2D.velocity.y < jumpFalloff || myRigidbody2D.velocity.y > 0)
            {
                if(!dashing)
                {
                   myRigidbody2D.velocity += fallSpeed * Physics.gravity.y * Vector2.up * Time.deltaTime;
                }              
            }
        }
    }

    private void DashMovement()
    {
        if (dashExecuted)
        {
            dashDir = new Vector3(inputAxis.x, inputAxis.y).normalized;

            if(dashDir == Vector3.zero)
            {
                if(facingRight)
                {
                    dashDir = Vector3.right;
                }
                else 
                {
                    dashDir = Vector3.left;
                }
            }

            dashing = true;
            dashExecuted = false;
            allowInput = false;
            myRigidbody2D.gravityScale = 0;
            dashExecutionTime = Time.time;
        }

        if (dashing)
        {
            myRigidbody2D.velocity = dashDir * dashSpeed;
            CreateAfterimage();
            if (Time.time >= dashExecutionTime + dashDuration)
            {
                dashing = false;
                myRigidbody2D.velocity = new Vector3(myRigidbody2D.velocity.x, myRigidbody2D.velocity.y > 3 ? 3 : myRigidbody2D.velocity.y);
                myRigidbody2D.gravityScale = 1;
                allowInput = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(dashing)
        {
            myRigidbody2D.velocity = Vector3.zero;
        }       
    }

    private void CreateAfterimage()
    {
        GameObject afterimage = Instantiate(Resources.Load<GameObject>("PlayerDash/PlayerAfterimage"), transform.position, Quaternion.identity);
        afterimage.GetComponent<SpriteAfterimage>().Initialize(GetComponent<SpriteRenderer>().sprite, GetComponent<SpriteRenderer>().flipX);
    }

    public void InflictDamage(float damageToInflict)
    {
        GameObject damageNumber = Instantiate(Resources.Load<GameObject>("DamageNumber"), transform.position, Quaternion.identity);
        damageNumber.GetComponent<FloatingText>().textString = "- " + damageToInflict.ToString();
    }
}
