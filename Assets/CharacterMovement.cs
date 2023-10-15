using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;

    public Camera cam;
    public float speed = 6f;
    public float smooth = 0.1f;
    float turnSmooth;
    [SerializeField] float rotationSmoothTime; 
    float currentAngle; 
    float currentAngleVelocity;
    
    //jump
    [Header("Gravity")]
    float velocityY;
    [SerializeField] float gravity = 10f;
    [SerializeField] float gravityMultiplier = 2;
    [SerializeField] float groundedGravity = -0.2f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] int maxJumps = 2;

    [SerializeField] float maxJumpTime = 2f;
    private float jumpStartTime;
    [SerializeField] float maxJumpHeight = 10f; // Maximum jump height
    [SerializeField] float minJumpHeight = 2f; // Minimum jump height



    private int jumpsRemaining;
    private void Start() {
        jumpsRemaining = maxJumps;
    }
    
    private void Awake() {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }
    private void HandleGravityAndJump()
    {
        if (controller.isGrounded)
        {
            jumpsRemaining = maxJumps; // Reset jumps when grounded
            velocityY = groundedGravity;
            if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
            {
                jumpStartTime = Time.time;
                jumpsRemaining--;
            }
        }
        else if (jumpsRemaining > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpsRemaining--;
                jumpStartTime = Time.time;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpsRemaining > 0)
        {
            float jumpTime = Time.time - jumpStartTime;
            float normalizedJumpTime = Mathf.Clamp01(jumpTime / maxJumpTime);
            float jumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, normalizedJumpTime);
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        /*if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
            jumpsRemaining--;
        }*/

        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }
    private void HandleMovement()
    {
        //capturing Input from Player
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);Vector3 rotatedMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(rotatedMovement * speed * Time.deltaTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleGravityAndJump();
    }
}
/*


float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, smooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(direction * speed * Time.deltaTime);
        }


*/