using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;

    public float PlayerWalkSpeed = 3f;
    public float PlayerRunSpeed = 5f;
    public float PlayerCrouchSpeed = 1f;
    public float PlayerSpeed;

    private CharacterController cc;

    //horizontal and vertical movement
    private float H;
    private float V;

    //clamps the cameras Z axis
    private int CameraZClamp = 0;

    //mouse axis
    private float mouseX = 0;
    private float mouseY = 0;

    //camera sensitivity
    public float CameraSensitivity = 2f;

    //does the mouse input affect the camera rotation
    private bool InputAllowed = true;

    public float JumpIntensity = 2;
    private Vector3 Movement;

    private float Gravity = -9.81f;
    private float VerticalSpeed = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        PlayerSpeed = PlayerWalkSpeed;
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
            mouseX += CameraSensitivity * Input.GetAxisRaw("Mouse X");
            mouseY -= CameraSensitivity * Input.GetAxisRaw("Mouse Y");

            MainCamera.transform.eulerAngles = new Vector3(mouseY = Mathf.Clamp(mouseY, -75, 75), mouseX, CameraZClamp);
            transform.eulerAngles = new Vector3(0, mouseX, 0);

            if (Input.GetAxisRaw("Run") == 1)
            {
                PlayerSpeed = PlayerRunSpeed;
            }
            else if (Input.GetAxisRaw("Crouch") == 1)
            {
                PlayerSpeed = PlayerCrouchSpeed;
                MainCamera.transform.localPosition = Vector3.zero;
            }
            else
            {
                PlayerSpeed = PlayerWalkSpeed;
                MainCamera.transform.localPosition = new Vector3(0, 0.5f, 0);
            }

            if (cc.isGrounded)
            {
                VerticalSpeed = -1f;

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    VerticalSpeed = JumpIntensity;
                }
            }
            else
            {
                VerticalSpeed += Gravity * Time.deltaTime;
            }

            //moves the player using the CharacterController component
            Vector3 HorizontalDirection = transform.right * H;
            Vector3 VerticalDirection = transform.forward * V;

            Vector3 MoveDirection = (HorizontalDirection + VerticalDirection).normalized;
            Movement = MoveDirection * PlayerSpeed;
            Movement.y = VerticalSpeed;
            cc.Move(Movement * Time.deltaTime);
        }

        //unlocks the mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputAllowed = false;
        }
        //locks the mouse
        else if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputAllowed = true;
        }
    }

}
