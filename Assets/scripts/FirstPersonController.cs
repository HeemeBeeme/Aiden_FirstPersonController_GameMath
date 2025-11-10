using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;

    public float PlayerSpeed = 3f;

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

            //moves the player
            Vector3 input = new Vector3(H, 0f, V);
            transform.Translate(input * PlayerSpeed * Time.deltaTime, Space.Self);
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
