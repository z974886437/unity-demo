using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8;
    private float xInput;

    [Header("Collision details")] 
    [SerializeField] private float groundCheckDistance;
    
    [SerializeField] private bool facingRight = true;

    private int a;
    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    private void HandleAnimations()
    {
        bool isMoving = rb.linearVelocity.x != 0;
        anim.SetBool("isMoving", isMoving);

    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void HandleFlip()
    {
        if(rb.linearVelocity.x > 0 && facingRight == false) // 如果当前角色向右移动且面朝左，则执行翻转
            Flip();
        else if(rb.linearVelocity.x < 0 && facingRight == true)// 如果当前角色向左移动且面朝右，则执行翻转
            Flip();
    }

    private void Flip()//翻动
    {
        transform.Rotate(0,180,0);// 通过旋转模型180度，达到水平翻转的效果
        facingRight = !facingRight; // 切换角色朝向状态
    }
}
