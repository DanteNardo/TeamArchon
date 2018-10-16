﻿using UnityEngine;

public class Player : MonoBehaviour {
    #region Members
    private ETeam team;
    private int teamPosition;
    private SquadManager squadManager;
    #endregion

    #region Methods
    /// <summary>
    /// Get componenets
    /// </summary>
    private void Awake () {
        squadManager = gameObject.GetComponent<SquadManager>();
        
    }
	
    /// <summary>
    /// Sets and saves player data.
    /// </summary>
    /// <param name="color">The player's team</param>
    /// <param name="position">The player's position on their team</param>
    public void SetPlayer(ETeam color, int position) {
        team = color;
        teamPosition = position;
        
        StrategyGame.Instance.NewPlayer(squadManager, teamPosition);
        squadManager.InstantiatePieces((int)team, teamPosition);
    }
    #endregion
}
