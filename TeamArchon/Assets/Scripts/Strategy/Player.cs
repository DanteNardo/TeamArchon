using UnityEngine;

public class Player : MonoBehaviour {
    #region Members
    ETeam team;
    int teamPos;
    SquadManager squadManager;
    #endregion

    #region Methods
    /// <summary>
    /// Get componenets
    /// </summary>
    void Start () {
        squadManager = GetComponent<SquadManager>();
    }
	
    /// <summary>
    /// Sets and saves player data.
    /// </summary>
    /// <param name="color">The player's team</param>
    /// <param name="pos">The player's position</param>
    public void SetPlayer(ETeam color, int pos) {
        team = color;
        teamPos = pos;
        
        StrategyGame.Instance.NewPlayer(squadManager);
        squadManager.InstantiatePieces((int)team, teamPos);
    }
    #endregion
}
