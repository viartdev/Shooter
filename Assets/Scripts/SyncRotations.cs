using UnityEngine;
using UnityEngine.Networking;

public class SyncRotations : NetworkBehaviour
{
    [SyncVar] private Quaternion _rotation;
   // [SyncVar] private Vector2 _position;

    void Update()
    {
        if (isLocalPlayer)
        {
            _rotation = this.transform.rotation;
          //  _position = this.transform.position;
        }
        if (!isLocalPlayer)
        {
            this.transform.rotation = _rotation;
           // this.transform.position = _position;
        }
    }
}
