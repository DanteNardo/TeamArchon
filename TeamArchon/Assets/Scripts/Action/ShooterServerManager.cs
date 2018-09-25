 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ShooterServerManager : NetworkBehaviour
{

    public GameObject shooterPlayerPrefab;
    NetworkManager nm;
    bool playersLoaded;
    // Use this for initialization
    void Start()
    {
        nm = GetComponent<NetworkManager>();
        playersLoaded = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (NetworkServer.connections.Count > 0)
        {
            if (playersLoaded == false&& NetworkServer.connections[0].isReady)
            {
                instanciatePlayers();
            }

        }
    }



    void instanciatePlayers()
    {
        Debug.Log(nm.client);
        playersLoaded = true;
        Debug.Log(NetworkServer.connections.Count);

        GameObject newPlayer = Instantiate<GameObject>(shooterPlayerPrefab, Vector3.zero, Quaternion.identity);

        NetworkConnection localConnection;

        if(NetworkServer.connections.Count %2 == 0)
        {
            newPlayer.tag = "Team2";

        }
        else
        {
            newPlayer.tag = "Team1";
        }
        localConnection = NetworkServer.connections[0];

        Destroy(localConnection.playerControllers[0].gameObject);

        NetworkServer.ReplacePlayerForConnection(localConnection, newPlayer, localConnection.playerControllers[0].playerControllerId);

        //GameObject newPlayer = Instantiate<GameObject>(shooterPlayerPrefab, Vector3.zero, Quaternion.identity);



    }
}
