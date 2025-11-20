using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;
    //camera starting position
    private Vector3 CameraInitialPos;

    public GameObject Visor;
    //visor starting position
    private Vector3 VisorInitialPos;


    public GameObject PlayerMesh;
    //initial pos and scales
    private Vector3 PlayerInitialScale = new Vector3(1, 1, 1);
    private Vector3 PlayerInitialPos = new Vector3(0, 0, 0);
    private float PlayerPosCrouch = -0.3f;
    private float PlayerScaleCrouch = 0.7f;

    //crouch heights
    private int ccCrouchHeight = 1;
    private int ccInitialHeight = 2;
    private Vector3 ccCrouchPos = new Vector3(0, -0.5f, 0);
    private Vector3 ccInitialPos = new Vector3(0, 0, 0);

    //bools to view in the inspector (for testing and such)
    public bool isCrouched = false;
    public bool isRunning = false;
    public bool isWalking = true;

    //can the player stand?
    public bool canStand = true;

    //current player speed
    public float Speed;

    [Header("Movement Speeds")]
    public float WalkSpeed = 3f;
    public float RunSpeed = 6f;
    public float CrouchSpeed = 1f;
    public float SpeedIntensityChange = 6;

    [Header("Other Useful Settings")]
    public float MouseSensitivity = 1f;
    public float JumpIntensity = 4;

    private float CrouchHeight = 0.5f;

    //character controller component
    private CharacterController cc;

    //horizontal and vertical movement
    private float H;
    private float V;

    //clamps the camera axis
    private int CameraZClamp = 0;
    private (int, int) CameraYClamp = (-85, 85);

    //mouse axis
    private float mouseX = 0;
    private float mouseY = 0;

    //does the mouse input affect the camera rotation
    private bool InputAllowed = true;
    //vector that determines movement direction
    private Vector3 Movement;

    //gravity intensity and the actual force pushing the player down
    private float GravityIntensity = -9.81f;
    private float GroundedVerticalSpeed = -1f;
    private float VerticalSpeed = -1f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Speed = WalkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraInitialPos = MainCamera.transform.localPosition;
        VisorInitialPos = Visor.transform.localPosition;
    }

    void Crouch()
    {
        //moves the camera, visor/eyes, and scales the player mesh and CharacterController down
        MainCamera.transform.localPosition = Vector3.zero;
        Visor.transform.localPosition = new Vector3(VisorInitialPos.x, MainCamera.transform.localPosition.y, VisorInitialPos.z);
        PlayerMesh.transform.localPosition = new Vector3(PlayerInitialPos.x, PlayerPosCrouch, PlayerInitialPos.z);
        PlayerMesh.transform.localScale = new Vector3(PlayerInitialScale.x, PlayerScaleCrouch, PlayerInitialScale.z);
        cc.height = ccCrouchHeight;
        cc.center = ccCrouchPos;

        //clamps the player speed
        if (Speed <= CrouchSpeed)
        {
            Speed = CrouchSpeed;
        }

        //changes player speed if on the ground (cant accelerate or decelerate in air)
        if (cc.isGrounded)
        {
            if (!(Speed <= CrouchSpeed))
            {
                Speed -= SpeedIntensityChange * Time.deltaTime;
            }
        }

        isCrouched = true;
        isWalking = false;
        isRunning = false;
    }

    void Run()
    {
        //moves the camera, visor/eyes, the player mesh and CharacterController back to their original transforms
        MainCamera.transform.localPosition = CameraInitialPos;
        Visor.transform.localPosition = VisorInitialPos;
        PlayerMesh.transform.localPosition = PlayerInitialPos;
        PlayerMesh.transform.localScale = PlayerInitialScale;
        cc.height = ccInitialHeight;
        cc.center = ccInitialPos;

        if (Speed >= RunSpeed)
        {
            Speed = RunSpeed;
        }

        if (cc.isGrounded)
        {
            if (!(Speed >= RunSpeed))
            {
                Speed += SpeedIntensityChange * Time.deltaTime;
            }
        }

        isCrouched = false;
        isWalking = false;
        isRunning = true;
    }

    void Walk()
    {
        MainCamera.transform.localPosition = CameraInitialPos;
        Visor.transform.localPosition = VisorInitialPos;
        PlayerMesh.transform.localPosition = PlayerInitialPos;
        PlayerMesh.transform.localScale = PlayerInitialScale;
        cc.height = ccInitialHeight;
        cc.center = ccInitialPos;

        if (cc.isGrounded)
        {
            if (Speed > WalkSpeed)
            {
                Speed -= SpeedIntensityChange * Time.deltaTime;
            }
            else if (Speed < WalkSpeed)
            {
                Speed += SpeedIntensityChange * Time.deltaTime;
            }

            if (Speed > (WalkSpeed - 0.1f) && Speed < (WalkSpeed + 0.1f))
            {
                Speed = WalkSpeed;
            }
        }

        isCrouched = false;
        isWalking = true;
        isRunning = false;
    }


    void Update()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");

        if (InputAllowed)
        {
            //turns mouse input into rotation data (rotates camera and player model)
            mouseX += MouseSensitivity * Input.GetAxisRaw("Mouse X");
            mouseY -= MouseSensitivity * Input.GetAxisRaw("Mouse Y");

            //handles the camera rotation clamping
            MainCamera.transform.eulerAngles = new Vector3(mouseY = Mathf.Clamp(mouseY, CameraYClamp.Item1, CameraYClamp.Item2), mouseX, CameraZClamp);
            transform.eulerAngles = new Vector3(0, mouseX, 0);

            
            //player crouch
            if (Input.GetAxisRaw("Crouch") == 1)
            {
                if (canStand)
                {
                    if (Input.GetAxisRaw("Run") == 0)
                    {
                        Crouch();
                    }
                    else
                    {
                        Run();
                    }
                }
            }
            //player run
            else if (Input.GetAxisRaw("Run") == 1)
            {
                if(canStand)
                {
                    Run();
                }
            }
            //player walk
            else
            {
                if(canStand)
                {
                    Walk();
                }
            }

            if(!canStand)
            {
                Crouch();
            }

            //jump & gravity
            if (cc.isGrounded)
            {
                VerticalSpeed = GroundedVerticalSpeed;

                if(Input.GetKey(KeyCode.Space))
                {
                    VerticalSpeed = JumpIntensity;
                }
            }
            else
            {
                VerticalSpeed += GravityIntensity * Time.deltaTime;
            }

            //moves the player using the CharacterController component
            Vector3 HorizontalDirection = transform.right * H;
            Vector3 VerticalDirection = transform.forward * V;

            Vector3 MoveDirection = (HorizontalDirection + VerticalDirection).normalized;
            Movement = MoveDirection * Speed;
            Movement.y = VerticalSpeed;
            cc.Move(Movement * Time.deltaTime);
        }

        //unlocks the mouse
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputAllowed = false;
            Time.timeScale = 0;
        }
        //locks the mouse
        else if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputAllowed = true;
            Time.timeScale = 1;
        }
    }

}
