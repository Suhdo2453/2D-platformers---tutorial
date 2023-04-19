using UnityEngine;


public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck
    {
        get => groundCheck;
        private set => groundCheck = value;
    }
    public Transform WallCheck
    {
        get => wallCheck;
        private set => wallCheck = value;
    }
    public Transform LedgeCheck
    {
        get => ledgeCheck;
        private set => ledgeCheck = value;
    }
    public Transform CeilingCheck
    {
        get => ceillingCheck;
        private set => ceillingCheck = value;
    }
    
    public float GroundCheckRadius
    {
        get => groundCheckRadius;
        private set => groundCheckRadius = value;
    }
    
    public float WallCheckDistance
    {
        get => wallCheckDistance;
        private set => wallCheckDistance = value;
    }
    
    public LayerMask WhatIsGround => whatIsGround;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ceillingCheck;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool Ceilling => Physics2D.OverlapCircle(ceillingCheck.position, groundCheckRadius, whatIsGround);

    public bool Ground => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    public bool WallFront => Physics2D.Raycast(wallCheck.position,
        Vector2.right * core.Movement.FacingDirection,
        wallCheckDistance, whatIsGround);

    public bool Ledge => Physics2D.Raycast(ledgeCheck.position,
        Vector2.right * core.Movement.FacingDirection,
        wallCheckDistance, whatIsGround);

    public bool WallBack => Physics2D.Raycast(wallCheck.position,
        Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, whatIsGround);
}