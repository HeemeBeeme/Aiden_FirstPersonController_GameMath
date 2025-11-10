using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{

    public Camera MainCamera;

    //current up and down rotation
    private float CurrentCameraXRotation;

    //current left and right rotation
    private float CurrentCameraYRotation;

    public float mouseXPosition;
    public float mouseYPosition;

    private float mouseX;
    private float mouseY;

    public float CameraXSensitivity = 1f;
    public float CameraYSensitivity = 1f;

    //does the mouse input affect the camera rotation
    private bool CameraActive = true;

    void Start()
    {
        CurrentCameraXRotation = MainCamera.transform.rotation.x;
        CurrentCameraYRotation = MainCamera.transform.rotation.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if(CameraActive)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y") * -1;

            mouseXPosition = CameraXSensitivity * mouseX;
            mouseYPosition = CameraYSensitivity * mouseY;

            MainCamera.transform.Rotate(mouseYPosition, mouseXPosition, 0);
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
