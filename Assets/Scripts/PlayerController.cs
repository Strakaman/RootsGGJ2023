using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    public CharacterController characterController;
    public InputAction jumpAction;
    public float speed;
    public float gravity = 9.81f;
    public float jumpForce;
    protected Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) { getCameraInputVector(); }
        Vector3 moveVector = getCameraInputVector() * Time.deltaTime * speed;
        if (characterController.isGrounded)
        {
            //moveVector.y = -1f;
            
        }
        else
        {
            moveVector.y -= gravity * 2f * Time.deltaTime;
        }
        characterController.Move(moveVector);
    }

    private Vector3 getInputOnlyMovementVector()
    {
        return new Vector3(moveInput.x, 0, moveInput.y);
    }
    private Vector3 getCameraInputVector()
    {
        Vector3 finalVector;

        Camera mainCam = Camera.main;

        Vector3 camForward = mainCam.transform.forward; //vector of where the cam is facing
        Vector3 camRight = mainCam.transform.right; //right angle of where cam is facing

        //remove any y affect on vectors
        camForward.y = 0; 
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        finalVector = (camForward * moveInput.y) + (camRight * moveInput.x);
        //Debug.Log("Forward direction of Camera" + mainCam.transform.forward);
        //Debug.Log("Right direction of Camera" + mainCam.transform.right);


        return finalVector;
    }
    private void LateUpdate()
    {
        UpdateRotation();
        Vector3 worldSpaceVelocity = characterController.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(worldSpaceVelocity);
        animator.SetFloat("Speed", Vector3.Magnitude(localVelocity));
    }

    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        moveInput = callbackContext.ReadValue<Vector2>();
        Debug.Log("Look Axis Changed: " + moveInput);

        //Debug.Log("Movement Input: " + movementVector);
        //transform.Rotate(new Vector3(0, movementVector.y, 0));
        //characterController.Move(new Vector3(movementVector.x, 0, movementVector.y));
    }

    public void UpdateRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
        Debug.LogWarning("Target Rotation: " + targetRotation);
        transform.rotation = targetRotation;
    }

}
