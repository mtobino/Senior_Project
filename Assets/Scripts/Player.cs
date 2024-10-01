using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Player: MonoBehaviour
{
    // field to hold game input
    [SerializeField]private GameInput gameInput;
    public HealthManager healthManager;

    [Header("Player Movement")]
    // predetermined movement speed
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float sprintFactor = 2f;

    [Header("Jumping Variables and Controls")]
    // need the rigid body to determine how the player jumps
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float jumpPower = 3f;

    // help control player jumping and prevent double jumping through rapid button presses
    private bool isJumpingCooldown = false;
    [SerializeField] private float jumpCooldownDuration = 0.5f; // Adjust as needed
    //[SerializeField] private float fallSpeed = 1f;
    [Header("To Adjust When Crouching")]
    [SerializeField] private BoxCollider boxCollider;

    // to be used to determine if what the player hit was indeed the ground
    private const int GROUND_LAYER = 3;

    // Hi Matt, I need this variable here so that I can access this class from the ability classes. pls dont delete  -James 
    // Message recieved - Matt :D 
    public static Player instance;

    public Transform playerModel;

    private bool isMoving;

    private bool isCrouching;

    private bool onGround;

    private bool isSprinting;

    private enum MovementState
    {
        WALKING, SPRINTING, CROUCH_WALKING, NOT_MOVING
    }
    private MovementState movementState;

    // Sounds
    [Header("Sounds")]
    [SerializeField] AudioSource runningSound;
    [SerializeField] AudioSource walkingSound;
    [SerializeField] AudioSource crouchingSound;
    [SerializeField] public float soundRange = 60f;
    public bool canHearPlayer = false;

    private HashSet<AudioSource> audios = new HashSet<AudioSource>();
    [Header("Camera Control")]
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;

    [Header("Pause Menu Control")]
    // Give player access to the menu to control with the pause action
    [SerializeField] private PauseMenuController pauseMenu;



    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
        isCrouching = false;
        onGround = true;
        //gameInput = GameInput.instance;
        healthManager = GetComponent<HealthManager>();
        audios.Add(walkingSound);
        audios.Add(runningSound); 
        audios.Add(crouchingSound);
        movementState= MovementState.NOT_MOVING;
        gameInput.OnCrouchAction += Crouch;
        gameInput.OnJumpAction += Jump;
        gameInput.OnSprintStart += StartSprinting;
        gameInput.OnSprintEnd += EndSprinting;
        gameInput.OnMenuToggle += PauseGame;
    }
    
    private void PauseGame(object sender, EventArgs e)
    {
        pauseMenu.PauseMenuPressed();
    }
    
    private void EndSprinting(object sender, EventArgs e)
    {
        isSprinting = false;
    }

    private void StartSprinting(object sender, EventArgs e)
    {
        isSprinting = true;
        //playSprintSound();
    }

    

    private void Jump(object sender, System.EventArgs e)
    {
        if (onGround && !isCrouching && !isJumpingCooldown)
        {
            rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            onGround = false;

            // Start the cooldown timer
            StartCoroutine(JumpCooldownTimer());
        }
        else if (isCrouching)
        {
            toggleCrouch();
        }
    }

    private IEnumerator JumpCooldownTimer()
    {
        isJumpingCooldown = true;
        yield return new WaitForSeconds(jumpCooldownDuration);
        isJumpingCooldown = false;
    }


    private void Crouch(object sender, System.EventArgs e)
    {
        if (isCrouching)
        {
            boxCollider.center = new Vector3(0, 1f, 0);
            boxCollider.size = new Vector3(0.75f, 2f, 0.3f);
        }
        else
        {
            boxCollider.center = new Vector3(0, 0.75f, 0);
            boxCollider.size = new Vector3(0.75f, 1.25f, 0.3f);
        }
        toggleCrouch();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        // hideCursor();    // FOR DEVELOPMENT MODE
        UpdateMovementState();
        PlayAppropiateSound();
    }

     private void HandleMovement()
     {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = Vector3.zero;
        float currSpeed = CalculateCurrentSpeed();

        if (firstPersonCamera != null && firstPersonCamera.gameObject.activeInHierarchy)
        {
            Vector3 forward = firstPersonCamera.transform.forward;
            Vector3 right = firstPersonCamera.transform.right;
            forward.y = 0; 
            right.y = 0; 
            moveDir = (forward * inputVector.y + right * inputVector.x).normalized;

            // Get the camera's rotation but only on the y-axis
            float cameraYRotation = firstPersonCamera.transform.eulerAngles.y;

            // Set the body model's rotation to match the camera's y-axis rotation
            playerModel.rotation = Quaternion.Euler(0, cameraYRotation, 0);
        }
        else
        {
            moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

            if (inputVector.y == 0)
            {
                moveDir = transform.right * inputVector.x;
            }
            else if (inputVector.x == 0 && inputVector.y < 0) 
            {
                moveDir = -transform.forward;
            }

            if (inputVector != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotateSpeed);
            }
        }

        float moveDistance = currSpeed * Time.deltaTime;

        if (moveDir != Vector3.zero)
        {
            transform.position += moveDir * moveDistance;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
    }

    private float CalculateCurrentSpeed()
    {
        if (isSprinting && !isCrouching)
        {
            return moveSpeed * sprintFactor;
        }
        else if (isCrouching)
        {
            return moveSpeed / 2;
        }
        else
        {
            return moveSpeed;
        }
    }

    private Vector3 CollisionDetectionAndDirection(Vector3 moveDir, float moveDistance)
    {
        float playerRadius = 0.4f;
        float playerHeight = 0.2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                return moveDirX;
            }
            else
            {
                // Attempt only Z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    return moveDirZ;
                }
            }
        }
        return canMove ? moveDir : Vector3.zero;
    }

    private void HandleFalling()
    {
         
        // If the player has any upwards speed
        if(rigidBody.velocity.y >= 0)
        {
            /*  if rigid body has velocity in the y direction greater than essentially 0, that means it is not on the ground. 
                therefore if we not it then we can control the variable to auto toggle when the rigid body no longer has
                vertical momentum.
            */
            onGround = !(Math.Abs(rigidBody.velocity.y) > Mathf.Epsilon);
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == GROUND_LAYER) // Ground Layer doesn't want to be ground so I hard coded it to 3 
        {
            onGround = true;
        }
        else
        {
            onGround= false;
        }
        
    }


    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }
    private void toggleCrouch()
    {
        isCrouching= !isCrouching;
    }

    public bool OnGround()
    {
        return onGround;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public GameInput GetGameInput()
    {
        return gameInput;
    }

    public bool paused()
    {
        return pauseMenu.isActiveAndEnabled;
    }

    private void playSprintSound()
    {
        if (runningSound.isPlaying)
            return;

        runningSound.Play();

        var sound = new Sound(transform.position, soundRange);
        sound.soundType = Sound.SoundType.PlayerRunning;
        Sounds.makeSound(sound);
        canHearPlayer = true;

        print($"Sound with position: {sound.pos} and range {sound.range} created");
    }
    private void UpdateMovementState()
    {
        if (isMoving && isSprinting && !isCrouching)
        {
            movementState = MovementState.SPRINTING;
        }
        else if (isMoving && isCrouching)
        {
            movementState = MovementState.CROUCH_WALKING;
        }
        else if (isMoving)
        {
            movementState = MovementState.WALKING;
        }
        else
        {
            movementState = MovementState.NOT_MOVING;
        }
    }
    private void PlayAppropiateSound()
    {
        Sound sound;
        switch(movementState)
        {
            case MovementState.WALKING:
                if (walkingSound.isPlaying)
                {
                    return;
                }
                StopAllOtherPlayerSounds(walkingSound);
                walkingSound.Play();
                sound = new Sound(transform.position, soundRange / 2);
                sound.soundType = Sound.SoundType.PlayerRunning;
                Sounds.makeSound(sound);
                canHearPlayer = true;
                break;

            
            case MovementState.SPRINTING:
                if (runningSound.isPlaying)
                    return;
                StopAllOtherPlayerSounds(runningSound);
                runningSound.Play();
                sound = new Sound(transform.position, soundRange);
                sound.soundType = Sound.SoundType.PlayerRunning;
                Sounds.makeSound(sound);
                canHearPlayer = true;
                break;
            
            case MovementState.CROUCH_WALKING:
                if (crouchingSound.isPlaying)
                    return;
                StopAllOtherPlayerSounds(crouchingSound);
                crouchingSound.Play();
                sound = new Sound(transform.position, soundRange / 4);
                sound.soundType = Sound.SoundType.PlayerRunning;
                Sounds.makeSound(sound);
                canHearPlayer = true;
                break;
            
            case MovementState.NOT_MOVING:
                StopAllSounds();
                break;
            
            default:
                Debug.LogError("Error: Should not be getting here :P");
                break;


        }
        

    }
    private void StopAllOtherPlayerSounds(AudioSource audio)
    {
       foreach(AudioSource source in audios)
        {
            if (!audio.Equals(source)) 
            {
                source.Stop(); 
            }
        }
    }
    private void StopAllSounds()
    {
        foreach(AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    private void hideCursor()
    {
        /* IF/ELSE STATEMENT FOR DEVELOPMENT MODE ONLY
        if(Input.GetKeyDown(KeyCode.LeftAlt) && !cursorHidden)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cursorHidden = true;
        }
        else if(Input.GetKeyDown(KeyCode.LeftAlt) && cursorHidden)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cursorHidden = false;
        }
        */

        /*  FOR PRODUCTION  */
    }
} //lalalala
