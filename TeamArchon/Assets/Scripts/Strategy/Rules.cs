using UnityEngine;

/// <summary>
/// A Singleton that generates legal moves based on a Board position.
/// </summary>
public class Rules : Singleton<Rules> {
    #region Methods
    /// <summary>
    /// Checks if a move is valid on the board (based on game rules)
    /// </summary>
    /// <param name="type">The type of the piece</param>
    /// <param name="color">The color of the piece</param>
    /// <param name="m">The move data</param>
    /// <returns>True if the move is valid, else false</returns>
    public bool ValidMove(EPieceType type, EPieceColor color, Move m) {
        // Checks if the move generated is strictly invalid
        if (m.Invalid) return false;

        // Calls the correct child function based on the piece type
        switch (type) {
            // TODO: Remove this and add actual checks
            default:
            return true;
        }

        return false;
    }
    #endregion
}
