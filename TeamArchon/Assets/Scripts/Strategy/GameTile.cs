﻿using UnityEngine;

public class GameTile : GamepadBehavior {
    #region Members
    public GameObject overlay;
    public GameObject objectiveImage;
    private bool highlighted;
    public int Row;
    public int Col;
    public string sceneName;
    public bool objective = false;
    #endregion

    #region Properties
    public int Index { // Row & Column as a single integer
        get {
            return Board.IndexFromRowAndCol(Row, Col);
        }
    }
    public ETeam Occupation { get; set; } = ETeam.None;
    #endregion

    #region Methods
    /// <summary>
    /// Determines starting row and column.
    /// </summary>
    private void Start() {
        Row = Mathf.FloorToInt(transform.position.z);
		Col = Mathf.FloorToInt(transform.position.x);

        // Disable objective image if it's unnecessary
        if (!objective) {
            objectiveImage.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the tile's highlighting.
    /// </summary>
    private void Update() {
        if (PotentialMovementSquare()) {
            // If this square is a potential movement square and isn't highlighted start
            if (!highlighted) {
                StartHighlight();
            }
        }
        // If this square has no potential and is highlighted stop
        else if (highlighted) {
            EndHighlight();
        }
    }

    /// <summary>
    /// Generates an attempted move/place if the tile is clicked.
    /// </summary>
    public override void OnClick(GamepadCursor cursor) {
        // Create an attempted move if there is a selected piece
        if (InputManager.Instance.Selected != null) {
            // Generate potential move and save it in InputManager
            Debug.Log("Selected: " + InputManager.Instance.Selected);
            Debug.Log("Piece Row&Col: " + InputManager.Instance.Selected.Z + " - " + InputManager.Instance.Selected.X);
            Debug.Log("Piece Index: " + InputManager.Instance.Selected.Index);
            Debug.Log("Tile Row&Col: " + Row + " - " + Col);

            // Check if this move is a capture
            Move move;
            if (Piece.IsOtherColor(
                GameBoard.Instance[Index],
                GameBoard.Instance[InputManager.Instance.Selected.Index])) {
                move = new Move(InputManager.Instance.Selected.Index, Index, true, this);
            }
            else {
                move = new Move(InputManager.Instance.Selected.Index, Index, false, this);
            }
            Debug.Log("New Move: " + move.From + " - " + move.To);
            Debug.Log("Invalid Move?: " + move.Invalid);

            // Tell the input manager (which tells the pieces) a move has been attempted
            InputManager.Instance.AttemptMove(move);
        }
    }

    /// <summary>
    /// Determines whether or not this tile should be highlighted.
    /// </summary>
    /// <returns></returns>
    private bool PotentialMovementSquare() {
        // Return false if no selected piece
        if (InputManager.Instance.Selected == null)
            return false;

        // See if the selected piece has a move that ends on this tile
        foreach (var move in InputManager.Instance.Selected.Moves) {
            if (move.To == Index) {
                return true;
            }
        }

        // Default return
        return false;
    }

    /// <summary>
    /// Starts the highlighting of this piece.
    /// </summary>
    private void StartHighlight() {
        highlighted = true;
        overlay.SetActive(true);
    }

    /// <summary>
    /// Ends the highlighting of this piece.
    /// </summary>
    private void EndHighlight() {
        highlighted = false;
        overlay.SetActive(false);
    }

    
    #endregion
}
