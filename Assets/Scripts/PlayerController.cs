using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator playerAnimator;

    public float speed = 2f;

    public float runSpeed = 5f;
    private float jumpHeight = 15f;
    private float gravity = -9.81f;
    private float groundCastDist = 0.05f;
    private bool grounded = false;
    
    
    private Vector3 velocity;
    
    
    
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get transforms
        Transform playerTransform = transform;
        Transform cameraTransform = mainCamera.transform;
        
        // Grounded
        grounded = Physics.Raycast(playerTransform.position, Vector3.down, groundCastDist);
        Debug.DrawRay(playerTransform.position, Vector3.down, grounded ? Color.green : Color.red);

        
        // Ground Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");   
        Vector3 movement = (playerTransform.right * x + playerTransform.forward * z);
        if (movement.magnitude > 0)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false); 
        }
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            controller.Move(movement * runSpeed * Time.deltaTime);
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            controller.Move(movement * speed * Time.deltaTime);  
            playerAnimator.SetBool("isRunning", false);

        }
        
        
        
        // Rotate
        playerTransform.rotation = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up);
        
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight);
        }

        if (grounded)
        {
            playerAnimator.SetBool("isJumping", false);

        }
        else
        {
            playerAnimator.SetBool("isJumping", true); 
        }
        
        controller.Move(velocity * Time.deltaTime);
        
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
