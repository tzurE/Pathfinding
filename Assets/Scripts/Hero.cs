using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private GridManager gridManager;
    private Tile currentTile;
    public Rigidbody2D myRigidBody;
    public bool moveToTarget;
    public int nextTileIndex;
    private Animator animator;
    Vector3 fixDist = new Vector3(-0.5f, 0.5f);


    // Start is called before the first frame update
    void Start()
    {
        gridManager = GridManager.Instance;
        currentTile = gridManager.gridArray[4, 11];
        myRigidBody = GetComponent<Rigidbody2D>();
        moveToTarget = false;
        animator = GetComponent<Animator>();
        animator.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToTarget)
        {
            Tile nextTile = gridManager.correctPath[nextTileIndex];
            if (Vector2.Distance(this.transform.position, nextTile.transform.position+fixDist) < 0.2 && nextTile != gridManager.targetTile)
            {
                nextTileIndex++;
            }
            else if (nextTile == gridManager.targetTile)
            {
                moveToTarget = false;
                animator.SetBool("moving", false);
            }
            else
            {
                animator.SetBool("moving", true);
                Vector3 change = (nextTile.transform.position + fixDist) - transform.position;
                myRigidBody.MovePosition(transform.position + change.normalized * 3 * Time.fixedDeltaTime);
                animator.SetFloat("horizontal", change.x);
                animator.SetFloat("vertical", change.y);
            }
        }
    }
}
