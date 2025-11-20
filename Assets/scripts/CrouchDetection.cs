using UnityEngine;

public class CrouchDetection : MonoBehaviour
{
    public FirstPersonController PlayerScript;

    private void OnTriggerEnter(Collider other)
    {
        if(!(other.gameObject.CompareTag("Player")))
        {
            PlayerScript.canStand = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.gameObject.CompareTag("Player")))
        {
            PlayerScript.canStand = true;
        }
    }
}
