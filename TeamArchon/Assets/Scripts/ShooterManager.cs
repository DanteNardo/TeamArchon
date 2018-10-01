using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class ShooterManager : NetworkBehaviour
{

    public GameObject pistol;
    public static ShooterManager instance;
    public GameObject machineGun;
    //using to send who wins after a certain kill count
    int killCount1;
    int killCount2;
    int killCountRequired;


    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        killCount1 = 0;
        killCount2 = 0;
        killCountRequired = 0;


        if (isServer)
        {
            SpawnPlayer(pistol);
        }
        /*
        if (!isLocalPlayer)
        {
            GameObject newPlayer = Instantiate<GameObject>(pistol, Vector3.zero, Quaternion.identity);
            NetworkConnection localConnection = NetworkServer.connections[0];

            Destroy(localConnection.playerControllers[0].gameObject);

            NetworkServer.ReplacePlayerForConnection(localConnection, newPlayer, localConnection.playerControllers[0].playerControllerId);

            //Spawn Player for the future
            //SpawnPlayer("Team1", pistol);
        }*/

    }

   



    // Update is called once per frame
    void Update()
    {
        if (killCount1 >= killCountRequired / 2)
        {
            //End round with team 1 winning
        }
        else if(killCount2>= killCountRequired/2)
        {
            //end round with team 2 winning

        }
    }
    [Server]
    void SpawnPlayer( GameObject playerType)
    {
        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            GameObject newPlayer = Instantiate<GameObject>(playerType, Vector3.zero, Quaternion.identity);
            //newPlayer.tag = team;
            NetworkConnection localConnection = NetworkServer.connections[i];

            //Destroy(localConnection.playerControllers[0].gameObject);

            NetworkServer.ReplacePlayerForConnection(localConnection, newPlayer, localConnection.playerControllers[0].playerControllerId);
            NetworkServer.Spawn(newPlayer);
            killCountRequired++;
        }
    }

    public void countDeath(string team)
    {
        if (team == "Team1") killCount1++;
        if (team == "Team2") killCount2++;
    }


}
