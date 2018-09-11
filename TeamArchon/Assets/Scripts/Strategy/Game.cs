﻿using UnityEngine;
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
    public int playerCount = 1;
    private int currentPlayer = 0;
    private int movedWallCount = 0;

    private UnityAction wallMoved;
    private UnityAction gladiatorMoved;
    private UnityAction minotaurMoved;
    #endregion

    #region Properties
    public EGameState GameState { get; private set; }
    public EGamePhase GamePhase { get; private set; }
    public ETurnState TurnState { get; private set; }
    private bool WallsMoved { get; set; } = false;
    private bool MinotaurMoved { get; set; } = false;
    private bool GladiatorMoved { get; set; } = false;
    #endregion

    #region Methods

    /// <summary>
    /// Initializes the events.
    /// </summary>
    private void Start() {

        // Create piece movement listeners
        wallMoved = new UnityAction(OnWallsMoved);
        gladiatorMoved = new UnityAction(OnGladiatorMoved);
        minotaurMoved = new UnityAction(OnMinotaurMoved);

        // Start listening to piece movement listeners
        EventManager.Instance.StartListening("WallMoved", wallMoved);
        EventManager.Instance.StartListening("GladiatorMoved", gladiatorMoved);
        EventManager.Instance.StartListening("MinotaurMoved", minotaurMoved);
    }

    /// <summary>
    /// Updates game state and calls appropriate functions accordingly.
    /// </summary>
    private void Update() {
        switch (GameState) {
            case EGameState.Play:
                if (TurnOver()) {
                    NextTurn();
                }
                break;
        }
    }

    /// <summary>
    /// Used to determine if the current turn is ended.
    /// </summary>
    /// <returns>True if turn is over, else false</returns>
    private bool TurnOver() {
        return WallsMoved && MinotaurMoved && GladiatorMoved;
    }

    /// <summary>
    /// Iterates the entire game to the next turn.
    /// </summary>
    public void NextTurn() {

        // Reset movement booleans
        movedWallCount = 0;
        WallsMoved = false;
        MinotaurMoved = false;
        GladiatorMoved = false;

        // Reset pieces' states
        BoardManager.Instance.ResetPieceStates();

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
    /// Called on wall move events.
    /// </summary>
    private void OnWallsMoved() {
        movedWallCount++;
        if (movedWallCount == 2) {
            WallsMoved = true;
        }
    }

    /// <summary>
    /// Called on gladiator move events.
    /// </summary>
    private void OnGladiatorMoved() {
        GladiatorMoved = true;
    }

    /// <summary>
    /// Called on minotaur move events.
    /// </summary>
    private void OnMinotaurMoved() {
        MinotaurMoved = true;
    }
    #endregion
}