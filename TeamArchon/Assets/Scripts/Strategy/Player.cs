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
    private void Start () {
        squadManager = gameObject.GetComponent<SquadManager>();
        Debug.Log(gameObject);
    }
	
    /// <summary>
    /// Sets and saves player data.
    /// </summary>
    /// <param name="color">The player's team</param>
    /// <param name="pos">The player's position</param>
    public void SetPlayer(ETeam color, int pos) {
        //Debug.Log(color + "   " + pos);
        team = color;
        teamPos = pos;
        Debug.Log(squadManager);
        StrategyGame.Instance.NewPlayer(squadManager, (int) color);
        squadManager.InstantiatePieces((int)team, teamPos);
    }
    #endregion
}
