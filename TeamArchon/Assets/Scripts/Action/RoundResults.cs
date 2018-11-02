﻿// TODO: Implement all data stored at the end of an ActionPhase round
public class RoundResults {
    #region Properties
    public int WinningTeam { get; private set; }
    public float Health { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="winningTeam">The team that won the Action round</param>
    public RoundResults(int winningTeam, float health) {
        WinningTeam = winningTeam;
        Health = health;
    }
    #endregion
}
