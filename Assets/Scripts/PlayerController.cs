using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 1; //midle one, (0-left 2-right)
    public float laneDistance = 3; //distance beetwen lanes
    
    public float jumpForce;
    public float Gravity = -20;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool ableToMakeADoubleJump = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGameStarted)
        {
            return;
        }

        if(forwardSpeed < maxSpeed)
            forwardSpeed += 0.05f * Time.deltaTime;



        direction.z = forwardSpeed;

        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
       
        if(isGrounded)
        {
            direction.y = 0f;
            ableToMakeADoubleJump = true;
            if(SwipeManager.swipeUp)
            {
                Jump();
            }
        } else
        {
            direction.y += Gravity * Time.deltaTime;
            if(ableToMakeADoubleJump & Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
                ableToMakeADoubleJump = false;
            }
        }
        
        if(SwipeManager.swipeRight){
            desiredLane++;
            if(desiredLane == 3)
            { desiredLane = 2;}
        }

        if(SwipeManager.swipeLeft){
            desiredLane--;
            if(desiredLane == -1)
            { desiredLane = 0;}
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 240 * Time.deltaTime);
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else   
            controller.Move(diff);
    }

    private void FixedUpdate() {
        if(!PlayerManager.isGameStarted)
        {
            return;
        }
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.transform.tag=="Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
}
