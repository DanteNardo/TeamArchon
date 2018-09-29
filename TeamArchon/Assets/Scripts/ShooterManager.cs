using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class ShooterManager : NetworkBehaviour
{

    public GameObject player;

    // Use this for initialization
    void Start()
    {


        if (!isLocalPlayer)
        {
            GameObject newPlayer = Instantiate<GameObject>(player, Vector3.zero, Quaternion.identity);
            NetworkConnection localConnection = NetworkServer.connections[0];

            Destroy(localConnection.playerControllers[0].gameObject);

            NetworkServer.ReplacePlayerForConnection(localConnection, newPlayer, localConnection.playerControllers[0].playerControllerId);
        }

    }

   



    // Update is called once per frame
    void Update()
    {

    }
}
