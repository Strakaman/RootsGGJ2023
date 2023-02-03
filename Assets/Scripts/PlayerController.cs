using Cinemachine;
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
    public CinemachineInputProvider cameraControls;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraControls != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                cameraControls.enabled = false;
            }
            if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
            {
                cameraControls.enabled = true;
            }
        }
        Vector3 inputVector = getCameraInputVector();
        UpdateRotation(inputVector);
        Vector3 moveVector = inputVector * Time.deltaTime * speed;
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
        Vector3 worldSpaceVelocity = characterController.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(worldSpaceVelocity);
        animator.SetFloat("Speed", Vector3.Magnitude(localVelocity));
    }

    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        moveInput = callbackContext.ReadValue<Vector2>();
        //Debug.Log("Move Axis Changed: " + moveInput);
    }

    public void UpdateRotation(Vector3 movingDirection)
    {
        if (moveInput.Equals(Vector2.zero)) { return; } //stop autosnapping back to 0 when player has given no movement
        Quaternion targetRotation = Quaternion.LookRotation(movingDirection);
        //Debug.Log("Target Rotation: " + targetRotation);
        transform.rotation = targetRotation;
    }

    //attack animation fires a "Hit" event so keep here for now
    void Hit()
    {

    }
}
