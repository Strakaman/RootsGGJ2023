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
    protected CombatStateMachine combatStateMachine;
    public CinemachineInputProvider cameraControls;
    public CinemachineFreeLook cineMachineFreeLook;
    protected Health myHealth;
    public bool isDead = false;
    // Start is called before the first frame update
    private void Awake()
    {
        myHealth = GetComponent<Health>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        combatStateMachine = GetComponent<CombatStateMachine>();
        myHealth.AddDeathListener(Die);
        myHealth.AddHealthLostListener(HealthLost);
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
        if (cineMachineFreeLook != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                cineMachineFreeLook.m_YAxis.m_InvertInput = !cineMachineFreeLook.m_YAxis.m_InvertInput;
            }
        }
        if (isDead) { return; }
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
        if (!combatStateMachine.IsAttacking())
        {
            characterController.Move(moveVector);
        }

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    TakeHit(3);
        //}
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

    public void TakeHit(int damage)
    {
        myHealth.TakeDamage(damage);
    }

    public void Die()
    {
        isDead = true;
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
        AudioManager.instance.PlayVoiceLine("PlayerDeath", 1);
        GameManager.instance.PlayerDeath();
        StartCoroutine(DeathAnimation());
    }

    public void HealthLost()
    {
        AudioManager.instance.PlayVoiceLine("PlayerHit", 1);
    }

    public void ResetPlayer()
    {
        animator.SetTrigger("Respawn");
        myHealth.InitializeHealth();
        SpawnManager.instance.SpawnPlayer(this);
        gameObject.tag = "Player";
        gameObject.layer = 8;
        AudioManager.instance.PlayVoiceLine("MatchStart");
        isDead = false;
    }

    private IEnumerator DeathAnimation()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.7f);
        float elapsedTime = 0;
        float waitTime = 1f;
        while (elapsedTime < waitTime)
        {
            float yPos = Mathf.Lerp(946.8974f, 945.3f, (elapsedTime / waitTime));
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Destroy(this.gameObject);
    }
}
