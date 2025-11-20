using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerRespawnPoint;

    private void Start()
    {
        PlayerRespawnPoint.transform.position = Player.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("KillZone"))
        {
            Player.transform.position = PlayerRespawnPoint.transform.position;
        }
    }
}
