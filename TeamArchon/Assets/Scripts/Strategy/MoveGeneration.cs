using System.Collections.Generic;

/// <summary>
/// Contains the necessary methods to generate move objects for every piece on the board.
/// </summary>
public class MoveGeneration {
    #region Methods
    /// <summary>
    /// Generates all of the possible moves in the current board position.
    /// Moves are stored in each piece.
    /// </summary>
    public void GenerateMoves(List<Piece> pieces) {
        // Iterate through the board
        for (int i = 0; i < pieces.Count; i++) {
            // Select piece, if none, go to next square
            Piece p = pieces[i];
            if (p == null) continue;

            // Clear all the piece's current moves
            p.Moves.Clear();

            // Generate a move in each direction for this piece
            Move north = GenerateMove(p, EDirection.North);
            Move south = GenerateMove(p, EDirection.South);
            Move east = GenerateMove(p, EDirection.East);
            Move west = GenerateMove(p, EDirection.West);

            // Add the moves to the piece's list of moves if it is valid
            if (Rules.Instance.ValidMove(p.pieceType, p.pieceColor, north)) p.Moves.Add(north);
            if (Rules.Instance.ValidMove(p.pieceType, p.pieceColor, south)) p.Moves.Add(south);
            if (Rules.Instance.ValidMove(p.pieceType, p.pieceColor, east))  p.Moves.Add(east);
            if (Rules.Instance.ValidMove(p.pieceType, p.pieceColor, west))  p.Moves.Add(west);
        }
    }

    /// <summary>
    /// Generates a move for a piece in a direction.
    /// </summary>
    /// <param name="piece">The piece to generate a move for</param>
    /// <param name="direction">The direction the move is in</param>
    /// <returns>The generated move</returns>
    public static Move GenerateMove(Piece piece, EDirection direction) {
        

        // Generates an invalid move
        return new Move(-1, -1);
    }
    #endregion
}
