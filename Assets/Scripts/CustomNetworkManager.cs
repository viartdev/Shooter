using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject gameManager = GameObject.Find("BoardLayout");
        BoardManager bm = gameManager.GetComponent<BoardManager>();
        Vector3 spawnPos = bm.GetRandomPosition();
        var currentPlayerCount = NetworkServer.connections.Count;
        if (currentPlayerCount <= startPositions.Count)
        {
            GameObject player = Instantiate(playerPrefab, spawnPos,
                Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        }
        else
        {
            conn.Disconnect();
        }
    }


}
