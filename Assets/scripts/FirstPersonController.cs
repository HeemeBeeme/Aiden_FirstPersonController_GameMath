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

    private float H;
    private float V;

    //sets the Z axis indefinitely
    private int CameraZClamp = 0;

    //mouse x axis
    private float mouseX = 0;
    //mouse y axis
    private float mouseY = 0;

    //camera sensitivity
    public float CameraSensitivity = 2f;

    //does the mouse input affect the camera rotation
    private bool InputAllowed = true;

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

            //moves the player
            Vector3 input = new Vector3(H, 0f, V);
            transform.Translate(input * PlayerSpeed * Time.deltaTime, Space.Self);

            if (cc.isGrounded && Input.GetAxisRaw("Jump") == 1)
            {
                
            }
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
