using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;

    //move
    public float movementSpeed = 10f;
    private Rigidbody2D rb;
    private float movementInputDirection;
    private bool isFacingRight = true;
    private Animator anim;
    private bool isWalking;
    private bool isGrounded;

    //jump
    public float jumpForce = 16f;
    public float groundCheckRadius;
    public int amountOfJumps = 1;
    public float jumpTimerSet = 0.15f;
    private bool canNormalJump;
    private bool canWallJump;
    private int amountOfJumpLeft;
    public float variableJumpHeightMultiplier = 0.5f;
    private float jumpTimer;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;

    //wall sliding
    public Transform wallCheck;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    private bool isTouchingWall;
    private bool isWallSliding;

    //move air
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;

    //wall jumping
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    public float wallHopForce;
    public float wallJumpForce;
    private int facingDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection)
        {
            isWallSliding = true;
        }
        else
            isWallSliding = false;
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }

        if (amountOfJumpLeft <= 0)
        {
            canNormalJump = false;
        }
        else canNormalJump = true;
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0) 
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (movementInputDirection != 0)
        {
            isWalking = true;
        }
        else isWalking = false;
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            //wall jump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if(isGrounded)
            {
                NormalJump();
            }
        }

        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpLeft = amountOfJumps;
            amountOfJumpLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        
        
        if (isWallSliding)
        {
            if(rb.velocity.y < wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
