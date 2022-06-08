using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixedTopDownController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool dashing = false;
    [SerializeField] private Transform movePoint;
    //===============================================//
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private Vector3 inputVector;
    private bool isMoving;
    //===============================================//
    public LayerMask obstacleLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
            {
                if (Mathf.Abs(inputVector.x) == 1 || Mathf.Abs(inputVector.x) == -1)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(inputVector.x, 0, 0), .2f, obstacleLayerMask))
                    {
                        movePoint.position += new Vector3(inputVector.x, 0f, 0f);                       
                    }
                }
                else if (Mathf.Abs(inputVector.y) == 1 || Mathf.Abs(inputVector.y) == -1)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, inputVector.y, 0), .2f, obstacleLayerMask))
                    {
                        movePoint.position += new Vector3(0f, inputVector.y, 0f);
                    }
                }              
            }           
        }      
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if (!dashing)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();

            Vector2 playerVelocity = inputVector * moveSpeed;

            myRigidbody.velocity = playerVelocity;

            if (inputVector.x == 1 || inputVector.x == -1)
            {
                
            }
            else if (inputVector.y == 1 || inputVector.y == -1)
            {
               
            }
        }
    }

    public void GridMovement(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inputVector = context.ReadValue<Vector2>();
            isMoving = true;
        }      
        if(context.canceled)
        {
            isMoving = false;
        }
    }

    public void InflictDamage(float damageToInflict)
    {
        GameObject damageNumber = Instantiate(Resources.Load<GameObject>("DamageNumber"), transform.position, Quaternion.identity);
        damageNumber.GetComponent<FloatingText>().textString = "- " + damageToInflict.ToString();
    }
}
