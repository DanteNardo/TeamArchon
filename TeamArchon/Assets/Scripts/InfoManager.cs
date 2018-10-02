using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfoManager : NetworkBehaviour {

    public List<GameObject> players;

	// Use this for initialization
	void Start () {
		
	}

    public void addPlayer(GameObject player)
    {
        players.Add(player);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
