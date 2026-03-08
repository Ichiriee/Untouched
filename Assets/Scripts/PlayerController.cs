using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{   
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    public float walkSpeed = 5f; 
    public float runSpeed = 8f;
    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
        
        if(_isFacingRight != value){
            transform.localScale *= new Vector2(-1, 1); 
        }
        _isFacingRight = value;
    }}

    public float CurrentMovementSpeed{ get
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                } else {
                    return walkSpeed;
                }
            } else {
                return 0;
            }
        }
    }

    [SerializeField] 
    private bool isMoving = false;
    public bool IsMoving{ get 
    { 
        return isMoving; 
        } 
        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    
    [SerializeField]
    private bool isRunning = false;
    public bool IsRunning{ get 
    { 
        return isRunning; 
        }
        set
        {
            isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // fix: was missing
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMovementSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); 
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 direction)
    {
        if (direction.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (direction.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context){
        if (context.started) {
            IsRunning = true;
        } else if (context.canceled) {
            IsRunning = false;
        }
    }
}