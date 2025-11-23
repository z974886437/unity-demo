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
    private bool facingRight = true;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGrounded;
    private bool isGrounded;
    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity",rb.linearVelocity.x);
        anim.SetFloat("yVelocity",rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);

    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
            TryToAttack();
    }

    private void TryToAttack()
    {
        if (isGrounded)
            anim.SetTrigger("attack");
           
        
    }
    
    private void TryToJump()
    {
        if(isGrounded && canJump)
          rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    
    private void HandleMovement()
    {
        if(canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void HandleCollision()// 使用射线检测物体是否接触地面
    {
        // 从物体当前位置向下发射射线，检测是否接触到地面
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGrounded);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,transform.position + new Vector3(0,-groundCheckDistance));// 绘制一条从物体位置到物体位置下方某一距离的线
    }
}
