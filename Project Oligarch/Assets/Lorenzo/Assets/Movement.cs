using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private float StartSpeed;

    public PlayerCore PlayerCore;
    public Transform orientation;

    [Tooltip("This should be a smaller number as this is what movespeed is multiplied by to get how far you travel in a slide")]
    public float SlideForce; //slide speed
    [Tooltip("How long a slide will last in seconds")]
    public float SlideTime = 1; //in seconds

    private bool Slide;
    private bool canSlide = true;
    [Tooltip("Slide cooldown in seconds")]
    public float SlideCooldown;

    private float playerHeight = 2f;
    public LayerMask whatIsGrounded;
    private bool grounded;

    private float horizontalInput;
    private float verticalInput;

    private bool Slope;
    private bool Active;

    Vector3 moveDirection;
    Vector3 SlopeForward;

    Rigidbody rb;
   

    RaycastHit hit;
    RaycastHit slopeHit;
    [Tooltip("Largest angle at which our character controller will consider a surface a slope, higher for steeper angles")]
    [Range(0,90)]
    public float MaxSlopeAngle;

    public float jumpForce;
    public float jumpCooldown;
    public float airMulti;
    private bool readyToJump;
    public KeyCode jumpkey = KeyCode.Space;

    private void Start()
    {
        StartSpeed = moveSpeed;
        readyToJump = true;
        Active = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
       
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    /// <summary>
    /// This is getting edge cases to properly calculate physics
    /// </summary>
    private void MyInput()
    {
        if(!Slide) //default input
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        if (Input.GetKey(jumpkey) && readyToJump && (grounded || OnSlope()) && !Slide) //jumping
        {
            readyToJump = false;
            StartCoroutine(Jump());
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(jumpkey) && readyToJump && grounded && Slide)  //cancel slide with jump
        {
            readyToJump = false;
            StartCoroutine(SlideJump());
            Invoke(nameof(ResetJump), jumpCooldown);
            StopCoroutine(SlideFunc());
            transform.localScale = new Vector3(1, 1, 1);  //returns scale to normal
            Slide = false;
        }
        if (Input.GetButtonDown("AbilityMove") && (horizontalInput != 0 || verticalInput != 0) && (grounded || OnSlope()) && canSlide) //slide can only be preformed if you are moving
        {
            StartCoroutine(SlideFunc());
        }
    }
    /// <summary>
    /// Moves Player when on flat ground or on slope
    /// </summary>
    private void MovePlayer()
    {
        if (!Slide && Active && !OnSlope())
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.015f * transform.localScale.y, whatIsGrounded); //ground check raycast
        }
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;//move in direction relative to camera
        if(!OnSlope())
            moveDirection.y = rb.velocity.y;
        if(grounded && !OnSlope())
            rb.velocity = moveDirection.normalized * moveSpeed * ( 1 + PlayerCore.moveMod); //grounded movement
        else if(OnSlope())
        {
            SlopeForward = SlopeDir(moveDirection , slopeHit.normal);
            rb.velocity =  SlopeForward * moveSpeed;
        }
    }
    /// <summary>
    /// Make the Player Jump
    /// </summary>
    private IEnumerator Jump()
        {
            if(OnSlope())
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }
        rb.AddForce ( transform.up * jumpForce , ForceMode.Impulse + PlayerCore.jumpMod );

            yield return new WaitForSeconds(0.5f);
            Active = true;
    }

    private void ResetJump()
        {
            readyToJump = true;
        }

    private void DragDown()
    {
        float gravMod = 25f;
        if (!grounded)
        {
            rb.AddForce(-transform.up * gravMod, ForceMode.Impulse);//snaps to ground
        }
    }
    /// <summary>
    /// Make the Player Slide across a surface, doesn't work in the air currently
    /// </summary>
    private IEnumerator SlideFunc()
    {
        canSlide = false;
        Slide = true;
        transform.localScale = new Vector3(1, 0.5f, 1);//reduce player height
        rb.velocity = new Vector3(rb.velocity.x, -20f, rb.velocity.z);
        moveSpeed *= SlideForce;
        rb.AddForce(moveDirection.normalized, ForceMode.Force);
        yield return new WaitForSeconds(SlideTime);
        Slide = false;
        transform.localScale = new Vector3(1, 1, 1);//return player height
        moveSpeed = StartSpeed;
        yield return new WaitForSeconds(SlideCooldown);
        canSlide = true;
        yield return null;
    }
    /// <summary>
    /// Cancel out of slide with a jump preserving momentum
    /// </summary>
    private IEnumerator SlideJump()
    {
        float reset = airMulti;
        airMulti = 1;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce , ForceMode.Impulse);
        rb.AddForce(moveDirection.normalized * 10f, ForceMode.Force);
        yield return new WaitForSeconds(SlideTime);
        airMulti = reset;
        moveSpeed = StartSpeed;
    }
    /// <summary>
    /// Checks if the Player is on a slope or not
    /// </summary>
    /// <param name="jump"> bool to check if we are jumping or not</param>
    /// <returns>if we are on a slope</returns>
    private bool OnSlope(bool jump = false)
    {
        
        if(jump == true)
        {
            return false;
        }
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, playerHeight * 0.5f + 0.15f)) //slope check
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        return false;

    }
    /// <summary>
    /// Gives us a direction along our slope
    /// </summary>
    private Vector3 SlopeDir(Vector3 moveDir, Vector3 slopeNorm)
    {
        return Vector3.ProjectOnPlane(moveDir, slopeNorm).normalized;
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = Vector3.down * (playerHeight * 0.5f + 0.015f * transform.localScale.y);
        Gizmos.DrawRay(transform.position, dir);
        if(OnSlope())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, SlopeForward);
        }
            

    }
    
}
