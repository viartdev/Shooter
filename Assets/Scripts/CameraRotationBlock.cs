using UnityEngine;
using UnityEngine.Networking;

public class CameraRotationBlock : NetworkBehaviour
{
    void Update()
    {
        if (isLocalPlayer)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
