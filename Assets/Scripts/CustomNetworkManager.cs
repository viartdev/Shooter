using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var currentPlayerCount = NetworkServer.connections.Count;
        if (currentPlayerCount <= startPositions.Count)
        {
            GameObject player = Instantiate(playerPrefab, startPositions[currentPlayerCount - 1].position,
                Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        }
        else
        {
            conn.Disconnect();
        }
    }


}
