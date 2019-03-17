using UnityEngine;
using UnityEngine.Networking;

public class Respawner : NetworkBehaviour
{
    void Update()
    {
        if (isLocalPlayer)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                transform.position = new Vector3(3, 3);
            }
        }
    }

}
