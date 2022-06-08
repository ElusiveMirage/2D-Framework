using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool facingRight;
    [SerializeField] private bool isInvuln = false;
    [SerializeField] private bool isAlive = true;   
    //===============================================//
    [Header("Input Checks")]
    //===============================================//
    [SerializeField] private bool allowInput = true;
    [SerializeField] private bool dashExecuted = false;
    //===============================================//
    [Header("Dashing")]
    //===============================================//
    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashExecutionTime;
    [SerializeField] private float dashDuration;
    [SerializeField] private Vector3 dashDir;
    //===============================================//
    [SerializeField] private LayerMask obstacleLayerMask;
    //===============================================//
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    void FixedUpdate()
    {
        //movement
        if(!dashing)
        {
            Vector2 playerVelocity = inputVector * moveSpeed;

            myRigidbody.velocity = playerVelocity;

            if(inputVector != Vector3.zero)
            {
                //myAnimator.SetBool("Running", true);
            }
        }

        DashMovement();
    }

    public void Death()
    {
        inputVector = Vector2.zero;
        allowInput = false;
        isAlive = false;
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if (!dashing && isAlive)
        {
            if (context.performed)
            {
                inputVector = context.ReadValue<Vector2>();
            }
            if (context.canceled)
            {
                inputVector = Vector3.zero;
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (allowInput)
        {
            //add float cooldown at end if necessary
            if (context.performed && !dashing && !dashExecuted && Time.time > dashExecutionTime)
            {
                dashExecuted = true;
            }
            if (context.canceled)
            {
                dashExecuted = false;
            }
        }
    }

    private void DashMovement()
    {
        if (dashExecuted)
        {
            dashDir = new Vector3(inputVector.x, inputVector.y).normalized;

            dashing = true;
            dashExecuted = false;
            allowInput = false;
            dashExecutionTime = Time.time;
        }

        if (dashing)
        {
            myRigidbody.velocity = dashDir * dashSpeed;
            CreateAfterimage();
            isInvuln = true;
            if (Time.time >= dashExecutionTime + dashDuration)
            {
                dashing = false;
                isInvuln = false;
                myRigidbody.velocity = Vector3.zero;

                allowInput = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dashing)
        {
            myRigidbody.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    private void DrawCubeGizmo()
    {
        Gizmos.color = Color.red;
        if (facingRight)
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(1f, 0.25f), new Vector3(2, 1.5f, 0));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(-1f, 0.25f), new Vector3(2, 1.5f, 0));
        }
    }

    private void CreateAfterimage()
    {
        GameObject afterimage = Instantiate(Resources.Load<GameObject>("PlayerDash/PlayerAfterimage"), transform.position, Quaternion.identity);
        afterimage.GetComponent<SpriteAfterimage>().Initialize(GetComponent<SpriteRenderer>().sprite, GetComponent<SpriteRenderer>().flipX);
    }


    public void InflictDamage(float damageToInflict)
    {
        if (isInvuln)
            return;

        GameObject damageNumber = Instantiate(Resources.Load<GameObject>("DamageNumber"), transform.position, Quaternion.identity);
        damageNumber.GetComponent<FloatingText>().textString = "- " + damageToInflict.ToString();
    }
}
