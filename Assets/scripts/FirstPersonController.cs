using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;

    //current player speed
    public float Speed;

    [Header("Movement Speeds")]
    public float WalkSpeed = 3f;
    public float RunSpeed = 5f;
    public float CrouchSpeed = 1f;
    public float SpeedIntensityChange = 4;

    [Header("Other Useful Settings")]
    public float MouseSensitivity = 2f;
    public float JumpIntensity = 4;
    public float CrouchHeight = 0.5f;


    //character controller component
    private CharacterController cc;

    //horizontal and vertical movement
    private float H;
    private float V;

    //clamps the cameras Z axis
    private int CameraZClamp = 0;

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

            MainCamera.transform.eulerAngles = new Vector3(mouseY = Mathf.Clamp(mouseY, -75, 75), mouseX, CameraZClamp);
            transform.eulerAngles = new Vector3(0, mouseX, 0);

            //player crouch
            if (Input.GetAxisRaw("Crouch") == 1)
            {
                if (Input.GetAxisRaw("Run") == 0)
                {
                    MainCamera.transform.localPosition = Vector3.zero;

                    if(Speed <= CrouchSpeed)
                    {
                        Speed = CrouchSpeed;
                    }
                    else
                    {
                        Speed -= SpeedIntensityChange * Time.deltaTime;
                    }
                }
                else
                {
                    MainCamera.transform.localPosition = new Vector3(0, CrouchHeight, 0);

                    if (Speed >= RunSpeed)
                    {
                        Speed = RunSpeed;
                    }
                    else
                    {
                        Speed += SpeedIntensityChange * Time.deltaTime;
                    }
                }
                
            }
            //player run
            else if (Input.GetAxisRaw("Run") == 1)
            {
                if(Input.GetAxisRaw("Crouch") == 0)
                {
                    if (Speed >= RunSpeed)
                    {
                        Speed = RunSpeed;
                    }
                    else
                    {
                        Speed += SpeedIntensityChange * Time.deltaTime;
                    }
                }
            }
            //player walk
            else
            {
                MainCamera.transform.localPosition = new Vector3(0, CrouchHeight, 0);

                if(Speed > WalkSpeed - 0.1f && Speed < WalkSpeed + 0.1f)
                {
                    Speed = WalkSpeed;
                }
                else if(Speed > WalkSpeed)
                {
                    Speed -= SpeedIntensityChange * Time.deltaTime;
                }
                else if(Speed < WalkSpeed)
                {
                    Speed += SpeedIntensityChange * Time.deltaTime;
                }
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
