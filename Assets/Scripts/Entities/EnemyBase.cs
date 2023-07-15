using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    //TODO : implement state machine for decision making
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private int scoreValue;
    //================================================//
    [SerializeField] private int currentNode;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private UI_Bar HPBar;
    [SerializeField] List<Transform> waypointRoute = new List<Transform>();
    [SerializeField] List<Node2D> aStarPath = new List<Node2D>();
    //================================================//
    private bool isAlive;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        HP = maxHP;

        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive && !GameManager_Example.Instance.playerDied)
        {
            HPBar.SetMaxValue(maxHP);
            HPBar.SetMinValue(HP);

            if (HP > 0)
            {
                if (waypointRoute.Count > 0 && currentWaypoint < waypointRoute.Count)
                {
                    if (aStarPath.Count > 0 && currentNode < aStarPath.Count)
                    {
                        GridMovement();
                    }
                    else if(aStarPath.Count == 0)
                    {
                        GetAstarPath(waypointRoute[currentWaypoint]);
                    }
                }
            }
            else
            {
                Death();
                Destroy(gameObject, 1f);
                isAlive = false;
            }
        }
        
        if(!isAlive)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 0f, Time.deltaTime));
        }      
    }

    private void FixedUpdate()
    {
        if(!GameManager_Example.Instance.playerDied)
        {
            if(isAlive)
            {
                //Physics related stuff here
            }
        }      
    }

    private void GetAstarPath(Transform targetTransform)
    {
        if (GetComponent<AStarPathfinding>())
        {
            aStarPath = GetComponent<AStarPathfinding>().CalculatePath(transform.position, targetTransform.transform.position);
        }
    }

    private void GridMovement()
    {
        if(currentNode < aStarPath.Count)
        {
            if (transform.position != aStarPath[currentNode].worldPos && !aStarPath[currentNode].unitPlaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, aStarPath[currentNode].worldPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentNode++;
            }
        }

        if(currentNode == aStarPath.Count)
        {
            currentWaypoint++;

            if (currentWaypoint < waypointRoute.Count)
            {             
                GetAstarPath(waypointRoute[currentWaypoint]);
                currentNode = 0;
            }
        }
    }

    public void GetGridReference(NodeGrid2D grid)
    {
        GetComponent<AStarPathfinding>().nodeGrid = grid;
    }

    public void InflictDamage(float damageToInflict)
    {
        HP -= damageToInflict;
        GameObject damageNumber = Instantiate(Resources.Load<GameObject>("DamageNumber"), transform.position, Quaternion.identity);
        damageNumber.GetComponent<FloatingText>().textString = "- " + damageToInflict.ToString();
    }

    private void Death()
    {

    }
}
