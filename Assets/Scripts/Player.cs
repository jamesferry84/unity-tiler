using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed = 6f;
    [SerializeField] float jumpSpeed = 22f;
    [SerializeField] float climbSpeed = 6f;
    //[SerializeField] Vector2 deathKickback = new Vector2()

    bool isAlive = true;
    float startingGravity;
    bool isColliding = false;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myfeetCollider;
    CircleCollider2D myAttackCollider;

    private void Awake()
    {
        
    }

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        startingGravity = myRigidBody.gravityScale;
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("isAlive", isAlive);
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myfeetCollider = GetComponent<BoxCollider2D>();
        myAttackCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        Attack();
        Jump();
        ClimbLadder();
        FlipSprite();
        Death();

        ProcessHit();
    }

    void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * playerSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    void Jump()
    {
        var groundLayer = LayerMask.GetMask("Ground");
        if (CrossPlatformInputManager.GetButtonDown("Jump") && myfeetCollider.IsTouchingLayers(groundLayer))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    //private void OnTriggerEnter2D(Collider2D otherCollider)
    //{
    //    var otherGameObject = otherCollider.gameObject;
    //    if (otherGameObject.GetComponent<EnemyMovement>())
    //    {
    //        Death();
    //    }
    //}

    void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            myAnimator.SetBool("isAttack", true);
        }
        
    }

    public void SetAttackToFalse()
    {
        myAnimator.SetBool("isAttack", false);
    }

    void ProcessHit()
    {
        if (myAttackCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
          //  Debug.Log("Enemy Hit");
        }
    }


    void Death()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
        {
            isAlive = false;
            myAnimator.SetBool("isAlive", isAlive);
            FindObjectOfType<GameSession>().processPlayerDeath();
        }
    }

    void ClimbLadder()
    {
        var ladderLayer = LayerMask.GetMask("Ladder");

        if (myfeetCollider.IsTouchingLayers(ladderLayer))
        {
            myRigidBody.gravityScale = 0f;
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.velocity = climbVelocity;
        }
        else
        { 
            myRigidBody.gravityScale = startingGravity;
        }
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        //if player moving horizontally
        if (playerHasHorizontalSpeed)
        {
            
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
        }
    }
}
