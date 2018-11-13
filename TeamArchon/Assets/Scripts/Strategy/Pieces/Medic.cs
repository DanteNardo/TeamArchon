using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Piece {
    #region Members
    public int healingFactor;
    #endregion

    #region Methods
    /// <summary>
    /// Calls base piece logic and special medic logic.
    /// </summary>
    private void Update() {
        // Update using the base piece code
        UpdatePiece();

        // Check if a player is attempting to use this piece's special
        if (selected && !specialMode && InputManager.Instance.SpecialAttempt) {
            BeginSpecialMode();
        }

        // Check if the special attempt has ended
        if (selected && SpecialMode && !InputManager.Instance.SpecialAttempt) {
            EndSpecialMode();
        }

        // During the special mode the Medic can heal an adjacent unit
        if (specialMode) {
            HighlightAdjacentSquares();
        }
    }

    /// <summary>
    /// Highlights all of the adjacent squares you can heal.
    /// </summary>
    private void HighlightAdjacentSquares() {
        // Clear old moves
        SpecialMoves.Clear();

        // Add the tiles that are up one, down one, to the left one, and to the right one
        if (Board.TileExists(Index + 1) && MoveGeneration.BoardWrapCheck(Index, Index + 1)) {
            SpecialMoves.Add(new Move(Index, Index + 1));
        }
        if (Board.TileExists(Index - 1) && MoveGeneration.BoardWrapCheck(Index, Index - 1)) {
            SpecialMoves.Add(new Move(Index, Index - 1));
        }
        if (Board.TileExists(Index + Board.Size) && MoveGeneration.BoardWrapCheck(Index, Index + Board.Size)) {
            SpecialMoves.Add(new Move(Index, Index + Board.Size));
        }
        if (Board.TileExists(Index - Board.Size) && MoveGeneration.BoardWrapCheck(Index, Index - Board.Size)) {
            SpecialMoves.Add(new Move(Index, Index - Board.Size));
        }
    }

    /// <summary>
    /// Override the generic piece special ability to work like the heal.
    /// </summary>
    public override void ActivateSpecialAbility() {
        HealPiece(SpecialTarget.Index);
        EndSpecialMode();
    }

    /// <summary>
    /// The medic's piece healing ability.
    /// </summary>
    /// <param name="index">The index where the piece you want to heal is</param>
    public void HealPiece(int index) {
        // Heal the piece at the given index
        Piece piece;
        if ((piece = StrategyGame.Instance.GetPiece(index)) != null) {
            piece.Heal(healingFactor);
        }
    }
    #endregion
}
