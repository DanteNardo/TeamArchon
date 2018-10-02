using System.Collections.Generic;
using UnityEngine.Events;

#region Game Enumerators
public enum EGameState {
    MainMenu,
    Play,
    Pause,
    GameOver,
    None
}
#endregion

/// <summary>
/// A component that handles the game state over time.
/// </summary>
public class StrategyGame : Singleton<StrategyGame> {
    #region Members
    public int playerCount = 0;
    private int currentPlayer = 0;
    private int movedPieceCount = 0;
    private List<Player> players;

    private UnityAction pieceMoved;
    #endregion

    #region Properties
    public ETeam TurnState { get; private set; }
    public EGameState GameState { get; private set; }
    public List<Player> Players { get { return players; } }
    public List<Piece> Pieces { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the events.
    /// </summary>
    private void Start() {
        // Create player list
        players = new List<Player>(playerCount);

        // Create piece list
        Pieces = new List<Piece>();

        // Create piece movement listeners
        pieceMoved = new UnityAction(OnPieceMoved);

        // Start listening to piece movement listeners
        EventManager.Instance.StartListening("PieceMoved", pieceMoved);
    }

    /// <summary>
    /// Updates game state and calls appropriate functions accordingly.
    /// </summary>
    private void Update() {
        switch (GameState) {
            case EGameState.Play:
                
                break;
        }
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
    #endregion
}
