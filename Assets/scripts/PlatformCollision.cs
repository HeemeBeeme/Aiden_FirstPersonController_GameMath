using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    public GameObject Player;
    private Transform Platform;
    private Vector3 lastPlatformPosition;
    private bool onPlatform = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Platform"))
        {
            Platform = other.transform;
            lastPlatformPosition = Platform.position;
            onPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Platform = null;
            onPlatform = false;
        }
    }

    private void FixedUpdate()
    {
        if(onPlatform)
        {
            Vector3 platformDelta = Platform.position - lastPlatformPosition;
            Player.transform.position += platformDelta;
            lastPlatformPosition = Platform.position;
        }
    }
}
