/// <summary>
/// Contains the necessary methods to generate move objects for every piece on the board.
/// </summary>
public class MoveGeneration {
    #region Methods
    /// <summary>
    /// Generates all of the possible moves in the current board position.
    /// Moves are stored in each piece.
    /// </summary>
    public void GenerateMoves() {
        // Iterate through the board
        for (int x = 0; x < BoardManager.Instance.Size; x++) {
            for (int z = 0; z < BoardManager.Instance.Size; z++) {

                // Select piece, if none, go to next square
                Piece p = BoardManager.Instance.Board[x, z];
                if (p == null) continue;

                // If the piece is a minotaur and it is not their turn, do not generate moves
                if (p.pieceType == EPieceType.Minotaur && !Rules.Instance.MinotaurTurn) continue;

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
    }

    /// <summary>
    /// Generates a move for a piece in a direction.
    /// </summary>
    /// <param name="piece">The piece to generate a move for</param>
    /// <param name="direction">The direction the move is in</param>
    /// <returns>The generated move</returns>
    public static Move GenerateMove(Piece piece, EDirection direction) {
        switch (direction) {
            case EDirection.North:
                if (BoardManager.Instance.gameBoard.TileExists(piece.X, piece.Z + 1)) {
                    return new Move(piece.X, piece.Z, piece.X, piece.Z + 1);
                }
                break;

            case EDirection.South:
                if (BoardManager.Instance.gameBoard.TileExists(piece.X, piece.Z - 1)) {
                    return new Move(piece.X, piece.Z, piece.X, piece.Z - 1);
                }
                break;

            case EDirection.East:
                if (BoardManager.Instance.gameBoard.TileExists(piece.X + 1, piece.Z)) {
                    return new Move(piece.X, piece.Z, piece.X + 1, piece.Z);
                }
                break;

            case EDirection.West:
                if (BoardManager.Instance.gameBoard.TileExists(piece.X - 1, piece.Z)) {
                    return new Move(piece.X, piece.Z, piece.X - 1, piece.Z);
                }
                break;
        }

        // Generates an invalid move
        return new Move(-1, -1, -1, -1);
    }
    #endregion
}
