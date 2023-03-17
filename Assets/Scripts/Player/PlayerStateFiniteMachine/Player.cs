using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public PlayerInventory Inventory{get; private set; }

    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;
    
    [SerializeField]
    private Transform ledgeCheck;
    
    [SerializeField]
    private Transform ceillingCheck;
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workSpaceVector;
    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();
        Inventory = GetComponent<PlayerInventory>();

        FacingDirection = 1;
        
        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
       // SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpaceVector.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpaceVector = direction * velocity;
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocityX(float velocity)
    {
        workSpaceVector.Set(velocity, CurrentVelocity.y);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    public void SetVelocityY(float velocity)
    {
        workSpaceVector.Set(CurrentVelocity.x, velocity);
        RB.velocity = workSpaceVector;
        CurrentVelocity = workSpaceVector;
    }

    #endregion

    #region Check Functions

    public bool CheckForCeilling()
    {
        return Physics2D.OverlapCircle(ceillingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public  bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workSpaceVector.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;
        
        MovementCollider.size = workSpaceVector;
        MovementCollider.offset = center;
    }
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workSpaceVector.Set((xDist + 0.015f) * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workSpaceVector), Vector2.down,
            ledgeCheck.position.y - wallCheck.position.y +0.015f, playerData.whatIsGround);
        float yDist = yHit.distance;

        workSpaceVector.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return workSpaceVector;
    }

    private void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * -FacingDirection *playerData.wallCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * FacingDirection *playerData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.right * FacingDirection *playerData.wallCheckDistance));
    }

    #endregion
}
