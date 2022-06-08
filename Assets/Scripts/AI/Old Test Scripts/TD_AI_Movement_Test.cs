using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_AI_Movement_Test : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    //================================================//
    private Transform followTarget;
    //================================================//
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCollider2D;
    List<Node2D> aStarPath = new List<Node2D>();

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

    private void FollowEntity()
    {
        var dir = followTarget.position - transform.position;
        transform.up = Vector3.MoveTowards(transform.up, dir, rotationSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, moveSpeed * Time.deltaTime);
    }

    private void SetTarget(Transform target)
    {
        followTarget = target;
    }

    public void GetAstarPath()
    {
        if(GetComponent<PathfindingTest>())
        {
            aStarPath = GetComponent<AStarPathfinding>().CalculatePath(transform.position, GetComponent<PathfindingTest>().endPos);
        }    
    }
}
