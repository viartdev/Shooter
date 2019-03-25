using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private float speed = 0.3f;
    public Weapon.WeaponType BulletType { get; set; }
    public int BulletDamage { get; set; }
    private Quaternion _rotation;
    private Vector3 _position;
    

    void Start()
    {
        _rotation = this.transform.rotation;
    }

    void FixedUpdate()
    {
        _position = this.transform.position;
        float angle = _rotation.eulerAngles.z;
        float x = Mathf.Sin(-1 * angle * Mathf.PI / 180) * speed;
        float y = Mathf.Cos(-1 * angle * Mathf.PI / 180) * speed;
        Vector3 endPos = _position;
        endPos.x += x;
        endPos.y += y;
        this.transform.position = endPos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            var player = col.gameObject.GetComponent<PlayerController>();
            player.GetDamage(BulletDamage);
        }

        Destroy(this.gameObject);
    }



}
