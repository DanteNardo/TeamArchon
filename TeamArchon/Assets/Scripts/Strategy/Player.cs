using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    ETeam team;
    int teamPos;

	// Use this for initialization
	void Start () {
		
	}
	
    public void setPlayer(ETeam color, int pos)
    {
        team = color;
        teamPos = pos;

        gameObject.GetComponent<SquadManager>().InstantiatePieces((int)team, teamPos);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
