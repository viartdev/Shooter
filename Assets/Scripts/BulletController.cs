using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour
{
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private float respawnTime = 2.0f;
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
            col.gameObject.SetActive(false);
           // RespawnPlayer(col.gameObject);
        }

        Destroy(this.gameObject);
    }

    private void RespawnPlayer(GameObject player)
    {
      //  StartCoroutine(RespawnForTime(player, respawnTime));
    }

  /*  IEnumerator RespawnForTime(GameObject player, float time)
    {
        yield return new WaitForSeconds(time);
        player.SetActive(true);
        player.transform.position = new Vector3(3, 3);
    }*/


}
