using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 77f;
    Vector3 velocity;
    public float jumpHeight = 2f;
    public float gravity = -19.81f;

    public bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = .2f;
    public LayerMask groundMask;

    private int leftFingerID, rightFingerID;
    private float halfScreenWidth;

    void Start()
    {
        leftFingerID = -1;
        rightFingerID = -1;
        halfScreenWidth = Screen.width / 2;
    }

    // Update is called once per frame
    void Update()
    {
        GetTouchInput();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Debug.Log("Is Grounded: " + isGrounded + " " + groundCheck.position + " " + groundDistance + " " + groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            //Debug.Log("AHHHHHHHH");
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime * 3f;
        controller.Move(velocity * Time.deltaTime);
    }

    void GetTouchInput()
    {
        if (Input.touchCount > 0)
        {
           // Debug.Log("Currently " + Input.touchCount + " fingers are touching");
        }
    }
}