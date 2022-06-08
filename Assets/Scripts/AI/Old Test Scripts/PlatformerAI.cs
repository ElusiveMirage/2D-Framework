using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Attack,
    }

    [SerializeField] private State state;
    [SerializeField] private LayerMask sightLayerMask;
    //===================================================//
    Animator myAnimator;
    Vector3 currentDir;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        currentDir = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Roaming)
        {

        }
    }

    private void Roaming()
    {
        if (CheckLineOfSight(currentDir, 1f))
        {
            currentDir = new Vector3(currentDir.x * -1, currentDir.y, 0);
        }
        if (!CheckLineOfSight(currentDir + new Vector3(0, -1, 0), 2f))
        {
            currentDir = new Vector3(currentDir.x * -1, currentDir.y, 0);
        }

        GetComponent<Platformer_AIMovement>().MoveInDirection(currentDir);
    }

    private bool CheckLineOfSight(Vector2 dir)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dir, sightLayerMask);

        if (raycastHit2D.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckLineOfSight(Vector2 dir, float distance)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dir, distance, sightLayerMask);

        if (raycastHit2D.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = State.Roaming;
        }
    }
}
