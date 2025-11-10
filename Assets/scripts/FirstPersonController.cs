using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;

    //current up and down rotation
    private float CurrentCameraXRotation = 0;

    //current left and right rotation
    private float CurrentCameraYRotation = 0;

    //sets the Z axis indefinitely
    private int CameraZClamp = 0;

    //mouse x axis
    private float mouseX = 0;
    //mouse y axis
    private float mouseY = 0;

    //camera sensitivity
    public float CameraSensitivity = 1f;

    //does the mouse input affect the camera rotation
    private bool CameraActive = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if(CameraActive)
        {
            mouseX += CameraSensitivity * Input.GetAxisRaw("Mouse X");
            mouseY -= CameraSensitivity * Input.GetAxisRaw("Mouse Y");

            MainCamera.transform.eulerAngles = new Vector3(mouseY = Mathf.Clamp(mouseY, -75, 75), mouseX, CameraZClamp);
            transform.eulerAngles = new Vector3(0, mouseX, 0);

            CurrentCameraXRotation = MainCamera.transform.localRotation.eulerAngles.x;
            CurrentCameraYRotation = MainCamera.transform.localRotation.eulerAngles.y;
        }

        //unlocks the mouse
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameraActive = false;
        }
        //locks the mouse
        else if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CameraActive = true;
        }
    }
}
