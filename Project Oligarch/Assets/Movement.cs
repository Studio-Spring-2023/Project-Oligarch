using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private float StartSpeed;

    public Transform orientation;

    public float groundDrag;

    public float slowDown;

    public float SlideForce; //slide speed

    public float SlideTime = 1; //in seconds

    private bool Slide;

    public float playerHeight;
    public LayerMask whatIsGrounded;
    public bool grounded;

    public float horizontalInput;
    public float verticalInput;

    public bool Slope;
    public bool Active;

    Vector3 moveDirection;
    Vector3 SlopeForward;

    Rigidbody rb;

    RaycastHit hit;
    RaycastHit slopeHit;
    public float SlopeAngle;

    public float jumpForce;
    public float jumpCooldown;
    public float airMulti; //how fast you move in air
    public bool readyToJump;
    public KeyCode jumpkey = KeyCode.Space;
    void Start() //initialize variables
    {
        StartSpeed = moveSpeed;
        readyToJump = true;
        Active = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!Slide && Active)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.015f * transform.localScale.y, whatIsGrounded); //ground check raycast
        }

        MyInput();
        //SpeedControl();

        //if(grounded) //drag
        //{
        //    rb.drag = groundDrag;
        //}
        //else
        //{
        //    rb.drag = 0;
        //}
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if(!Slide) //default input
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        if (Input.GetKey(jumpkey) && readyToJump && grounded && !Slide && OnSlope())
        {
            Active = false;
            grounded = false;
            readyToJump = false;
            Debug.Log("Slope Jump");
            rb.useGravity = true;
            OnSlope(true);
            StartCoroutine(Jump());
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(jumpkey) && readyToJump && grounded && !Slide)
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
        if (Input.GetButtonDown("AbilityMove") && (horizontalInput != 0 || verticalInput != 0)) //slide can only be preformed if you are moving
        {
            StartCoroutine(SlideFunc());
            DragDown();
        }
    }

    private void MovePlayer()
    {
        Vector3 Moving = new Vector3(horizontalInput, 0, verticalInput); //this is just to check if we are moving
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;//move in direction relative to camera
       // moveDirection.y = rb.velocity.y;
        if(grounded && !OnSlope())
            rb.velocity = moveDirection.normalized * moveSpeed; //grounded movement

        else if(OnSlope())
        {
            //Debug.Log("On Slope");
            SlopeForward = SlopeDir(moveDirection , slopeHit.normal);
            rb.velocity =  SlopeForward * moveSpeed;
        }
           
       // if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A)  && !Input.GetKey(KeyCode.S)  && !Input.GetKey(KeyCode.D) && grounded) 
        //   if(Moving == Vector3.zero && grounded)
        //{
            
        //    if(!Slide)
        //    {
        //        //rb.velocity = new Vector3(0,0,0);
        //    }
        //     //newton's law of inertia, 8f can be adjusted for a bit of momentum to the stop
            
        //}
    }

    private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    private IEnumerator Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

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
    
    private IEnumerator SlideFunc()
    {
        
        Slide = true;
        //reduce player height
        transform.localScale = new Vector3(1, 0.5f, 1);
        //DragDown();
        rb.velocity = new Vector3(rb.velocity.x, -20f, rb.velocity.z);
        moveSpeed *= SlideForce;
        //test heigher gravity in air
        rb.AddForce(moveDirection.normalized, ForceMode.Force);
        yield return new WaitForSeconds(SlideTime);
        Slide = false;
        transform.localScale = new Vector3(1, 1, 1);
        moveSpeed = StartSpeed;
        //return player height
        yield return null;

    }
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

    private bool OnSlope(bool jump = false)
    {
        if(jump == true)
        {
            //moveDirection.y = rb.velocity.y;
            return false;
        }
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < SlopeAngle && angle != 0;
        }

        return false;

    }

    private Vector3 SlopeDir(Vector3 moveDir, Vector3 slopeNorm)
    {
        return Vector3.ProjectOnPlane(moveDir, slopeNorm).normalized;
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = Vector3.down * (playerHeight * 0.5f + 0.015f * transform.localScale.y);
        if(OnSlope())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, SlopeForward);
        }
            

    }
    
}
