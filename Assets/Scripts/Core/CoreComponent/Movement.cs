using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; }
    
    public Vector2 CurrentVelocity { get; private set; }
    
    private Vector2 workSpaceVector;

    protected override void Awake()
    {
        base.Awake();
        
        FacingDirection = 1;
        RB = GetComponentInParent<Rigidbody2D>();
    }

    public void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

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
    
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #endregion
}
