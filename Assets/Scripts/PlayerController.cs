using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementVector = callbackContext.ReadValue<Vector2>();
        Debug.Log("Movement Input: " + movementVector);
        //transform.Rotate(new Vector3(0, movementVector.y, 0));
        //characterController.Move(new Vector3(movementVector.x, 0, movementVector.y));
    }
}
