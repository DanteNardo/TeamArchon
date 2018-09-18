using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Game Enumerators
public enum EGameState {
    MainMenu,
    Play,
    Pause,
    GameOver,
    None
}
public enum EGamePhase {
    BuildingPhase,
    EscapingPhase,
    None
}
public enum ETurnState {
    WallTurn,
    GladiatorTurn,
    MinotaurTurn,
    None
}
#endregion

/// <summary>
/// A component that handles the game state over time.
/// </summary>
public class Game : MonoBehaviour {
    #region Members
    public int playerCount = 2;
    private int currentPlayer = 0;
    private int movedPieceCount = 0;
    private List<Player> players;
    private List<Piece> pieces;

    public MoveGeneration moveGenerator;

    private UnityAction pieceMoved;
    #endregion

    #region Properties
    public EGameState GameState { get; private set; }
    public EGamePhase GamePhase { get; private set; }
    public ETurnState TurnState { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the events.
    /// </summary>
    private void Start() {
        // Create player list
        players = new List<Player>(playerCount);

        // Create piece list
        pieces = new List<Piece>();

        // Create MoveGenerator
        moveGenerator = new MoveGeneration();

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
        moveGenerator.GenerateMoves(pieces);
        NextTurn();
    }
    #endregion
}
