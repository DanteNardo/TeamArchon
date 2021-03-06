﻿using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages a specific set of pieces based on given input.
/// </summary>
public class SquadManager : MonoBehaviour {
    #region Members
    public GameObject[] prefabs;
    private GameObject[] pieceObjects;
    private Piece[] pieces;

    public GameObject shooterPrefab;
    public int team;
    #endregion

    #region Methods
    /// <summary>
    /// Initializes squad pieces for local player.
    /// </summary>
    private void Start() {
        StrategyGame.Instance.NewPlayer(this);
        InstantiatePieces();
    }

    /// <summary>
    /// Creates all of the pieces from prefabs and saves their data.
    /// </summary>
    private void InstantiatePieces() {
        Debug.Log("...Instantiating pieces");
        // Store piece objects and piece prefabs
        pieceObjects = new GameObject[prefabs.Length];
        pieces = new Piece[prefabs.Length];

        if (prefabs.Length == 0) {
            Debug.Log("This bitch empty!");
        }

        // Generate pieces for this player
        for (int i = 0; i < prefabs.Length; i++) {
            Debug.Log("...Instantiating pieces in for loop");
            // Determine the placement for this piece based on amount of players
            // TODO: Make this not assume there will always be four pieces
            int row = 0, col = 0;
            
            // Light team is in rows 0-1
            if ((ETeam)team == ETeam.Light) {
                switch (i) {
                    case 0: row = 0; col = 0 + 2 * StrategyGame.Instance.playerCount; break;
                    case 1: row = 1; col = 0 + 2 * StrategyGame.Instance.playerCount; break;
                    case 2: row = 0; col = 1 + 2 * StrategyGame.Instance.playerCount; break;
                    case 3: row = 1; col = 1 + 2 * StrategyGame.Instance.playerCount; break;
                }
            }
            // Dark team is in rows 6-7
            else {
                switch (i) {
                    case 0: row = 7; col = 0 + 2 * StrategyGame.Instance.playerCount; break;
                    case 1: row = 6; col = 0 + 2 * StrategyGame.Instance.playerCount; break;
                    case 2: row = 7; col = 1 + 2 * StrategyGame.Instance.playerCount; break;
                    case 3: row = 6; col = 1 + 2 * StrategyGame.Instance.playerCount; break;
                }
            }

            // Create real world position
            Vector3 position = new Vector3(col, 0, row);

            // Create piece gameobject
            var instance = Instantiate(prefabs[i], position, Quaternion.identity);
            Piece piece = instance.GetComponent<Piece>();
            pieces[i] = piece;

            // Set the parent on this client
            instance.transform.parent = gameObject.transform;

            GameBoard.Instance.PlacePiece(piece);
        }
        // Finish instantiating player pieces and increase player count
        StrategyGame.Instance.IncreasePlayerCount();
    }

    /// <summary>
    /// Checks if this is a local player
    /// </summary>
    /// <returns>True if this is a local player, else false</returns>
    public bool CheckLocalPlayer() {
        return true;
    } 
    #endregion
}
