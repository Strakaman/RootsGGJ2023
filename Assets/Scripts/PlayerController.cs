using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    public CharacterController characterController;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * speed;
        if (characterController.isGrounded)
        {
            moveVector.y = -1f;
        }
        else
        {
            moveVector.y -= 10f;
        }
        characterController.Move(moveVector);
    }

    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        moveInput = callbackContext.ReadValue<Vector2>();
        //Debug.Log("Movement Input: " + movementVector);
        //transform.Rotate(new Vector3(0, movementVector.y, 0));
        //characterController.Move(new Vector3(movementVector.x, 0, movementVector.y));
    }
}
