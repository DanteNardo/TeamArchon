using UnityEngine.Events;

/// <summary>
/// An overridden implementation of UnityEvents that requires
/// a RoundResults variable as a paramter when invoking the event.
/// </summary>
[System.Serializable]
public class RoundEvent : UnityEvent<RoundResults> { }

/// <summary>
/// Holds all of the data necessary to send back to the Strategy phase.
/// </summary>
public class RoundData {
    #region Properties
    public int WinningTeam { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="winningTeam">The team that won the Action phase</param>
    public RoundData(int winningTeam) {
        WinningTeam = winningTeam;
    }
    #endregion
}
