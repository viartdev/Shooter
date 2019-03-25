using UnityEngine;
using UnityEngine.Networking;

public class ShootBullets : NetworkBehaviour
{

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform spawnPosition;

    private PlayerController player;

    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (this.isLocalPlayer && Input.GetKeyDown(KeyCode.Mouse0) && player.Ammo>0 )
        {
            player.Ammo -= 1;
            this.CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        bullet.GetComponent<BulletController>().BulletType = player.weapon.weaponType;
        bullet.GetComponent<BulletController>().BulletDamage = player.weapon.Damage;
        NetworkServer.Spawn(bullet);
    }
}
