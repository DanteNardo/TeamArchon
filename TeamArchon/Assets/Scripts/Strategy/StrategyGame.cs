using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that handles the game state over time.
/// </summary>
public class StrategyGame : Singleton<StrategyGame> {
    #region Members
    public int playerCount = 0;
    private int currentPlayer = 0;
    private int movedPieceCount = 0;

    // Squad Loadouts
    public GameObject[] lightPrefabs;
    public GameObject[] darkPrefabs;

    private UnityAction pieceMoved;
    #endregion

    #region Properties
    public ETeam TurnState { get; private set; }
    public List<Piece> Pieces { get; private set; } = new List<Piece>();
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the events.
    /// </summary>
    private void Start() {
        // Create piece movement listeners
        pieceMoved = new UnityAction(OnPieceMoved);

        // Start listening to piece movement listeners
        EventManager.Instance.StartListening("PieceMoved", pieceMoved);
    }

    /// <summary>
    /// Iterates the entire game to the next turn.
    /// </summary>
    public void NextTurn() {
        // Reset movement booleans
        movedPieceCount = 0;

        // Iterate to next player's turn
        IteratePlayer();
    }

    /// <summary>
    /// Iterates to the next player.
    /// </summary>
    private void IteratePlayer() {
        currentPlayer = currentPlayer + 1 < playerCount ? currentPlayer + 1 : 0;
    }

    /// <summary>
    /// Called on piece move events.
    /// </summary>
    private void OnPieceMoved() {
        movedPieceCount++;
        NextTurn();
    }

    /// <summary>
    /// Determines the team for the new StrategyPlayer and
    /// gives it a list of prefabs that determine its squad.
    /// </summary>
    /// <param name="squad">The new strategy player's squadmanager</param>
    /// <param name="prefabs">The player's squad pieces</param>
    /// <param name="color">The team the player is on</param>
    public void NewPlayer(SquadManager squad, GameObject[] prefabs, int color) {
        Debug.Log("================== NEW PLAYER! ==================");
        Debug.Log("Player Count: " + playerCount);

        // Determine which type of player to add (light or dark)
        squad.team = color;

        Debug.Log("Team: " + playerCount % 2);
        Debug.Log("Team: " + (ETeam)squad.team);

        // Set list of prefabs for squad based on team
        squad.prefabs = prefabs;

        Debug.Log("Prefabs Length: " + squad.prefabs.Length);
    }

    /// <summary>
    /// Increases the playercount on the server.
    /// </summary>
    public void IncreasePlayerCount() {
        Debug.Log("Player Count: " + playerCount);
        playerCount++;
    }
    #endregion
}
