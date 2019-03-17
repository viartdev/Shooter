using UnityEngine;
using UnityEngine.Networking;

public class ShootBullets : NetworkBehaviour
{

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform spawnPosition;


    void Update()
    {
        if (this.isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            this.CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        NetworkServer.Spawn(bullet);
    }
}
