using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the necessary methods to generate move objects for every piece on the board.
/// </summary>
public static class MoveGeneration {
    #region Methods
    /// <summary>
    /// Generates all of the possible moves in the current board position.
    /// Moves are stored in each piece.
    /// </summary>
    public static void GenerateMoves(List<Piece> pieces) {
        // Iterate through the board
        for (int i = 0; i < pieces.Count; i++) {
            // Select piece, if null, go to next piece
            Piece p = pieces[i];
            if (p == null) continue;

            // Clear all the piece's current moves
            p.Moves.Clear();

            // Generate a move list ignoring specific piece requirements
            List<Move> potentialMoves = GenerateMoves(pieces[i]);

            // Add only the valid moves to the piece's move list
            foreach (var move in potentialMoves) {
                if (Rules.Instance.ValidMove(pieces[i].pieceType, move)) {
                    p.Moves.Add(move);
                }
            }
        }
    }

    /// <summary>
    /// Generates a list of move structures.
    /// Uses a prim-like graph solution to finding possible moves.
    /// Cannot move through pieces.
    /// Can land on enemy squares.
    /// Cannot land on friendly squares.
    /// TODO: Add in obstacle checking.
    /// </summary>
    /// <param name="piece">The piece to generate valid moves for</param>
    /// <returns>A list of all this piece's possible moves</returns>
    public static List<Move> GenerateMoves(Piece piece) {
        // Set up variables for movement depth
        int steps = 0;
        int totalSteps = piece.Speed;
        int start = piece.Index;

        // Set up variables for recording all possible tiles to move to
        List<Move> moves = new List<Move>();
        List<int> cells = new List<int>();
        List<int> nextCells = new List<int>();
        int cc = start;

        // Next row
        if (Board.TileExists(cc + Board.Size)) {
            // Check if there is a piece there
            if (GameBoard.Instance.PieceAt(cc + Board.Size) &&
                Piece.IsOtherColor(GameBoard.Instance[cc], GameBoard.Instance[cc + Board.Size])) {
                cells.Add(cc + Board.Size);
            }
            else {
                cells.Add(cc + Board.Size);
            }
        }
        // Previous row
        if (Board.TileExists(cc - Board.Size)) {
            // Check if there is a piece there
            if (GameBoard.Instance.PieceAt(cc - Board.Size) &&
                Piece.IsOtherColor(GameBoard.Instance[cc], GameBoard.Instance[cc - Board.Size])) {
                cells.Add(cc - Board.Size);
            }
            else {
                cells.Add(cc - Board.Size);
            }
        }
        // Next column
        if (Board.TileExists(cc + 1)) {
            // Check if there is a piece there
            if (GameBoard.Instance.PieceAt(cc + 1) &&
                Piece.IsOtherColor(GameBoard.Instance[cc], GameBoard.Instance[cc + 1])) {
                cells.Add(cc + 1);
            }
            else {
                cells.Add(cc + 1);
            }
        }
        // Previous column
        if (Board.TileExists(cc - 1)) {
            // Check if there is a piece there
            if (GameBoard.Instance.PieceAt(cc - 1) &&
                Piece.IsOtherColor(GameBoard.Instance[cc], GameBoard.Instance[cc - 1])) {
                cells.Add(cc - 1);
            }
            else {
                cells.Add(cc - 1);
            }
        }

        // Keep stepping until we have completed every option
        while (steps < totalSteps) {
            // No possible moves
            if (cells.Count == 0) {
                return moves;
            }
            // Select a random current cell
            else if (cells.Count > 1) {
                cc = cells[Random.Range(0, cells.Count)];
            }
            else cc = cells[0];
            
            // If the square contains a piece of the other color,
            // add the valid move, but do not move through it, and set capture to true
            if (Piece.IsOtherColor(piece.pieceType, GameBoard.Instance[cc])) {
                var move = new Move(start, cc, true);
                if (NewMove(moves, move)) {
                    moves.Add(move);
                }
                //Debug.Log("New Move:" + moves[moves.Count - 1].From + " - " + moves[moves.Count - 1].To);
            }
            // Add this cell's neighbors to continue searching for possible moves
            else if (!GameBoard.Instance.PieceAt(cc) && cc != start) {
                var move = new Move(start, cc);
                if (NewMove(moves, move)) {
                    moves.Add(move);
                }
                //Debug.Log("New Move:" + moves[moves.Count - 1].From + " - " + moves[moves.Count - 1].To);

                // Next row
                if (Board.TileExists(cc + Board.Size) && BoardWrapCheck(cc, cc + Board.Size)) {
                    nextCells.Add(cc + Board.Size);
                }
                // Previous row
                if (Board.TileExists(cc - Board.Size) && BoardWrapCheck(cc, cc - Board.Size)) {
                    nextCells.Add(cc - Board.Size);
                }
                // Next column
                if (Board.TileExists(cc + 1) && BoardWrapCheck(cc, cc + 1)) {
                    nextCells.Add(cc + 1);
                }
                // Previous column
                if (Board.TileExists(cc - 1) && BoardWrapCheck(cc, cc - 1)) {
                    nextCells.Add(cc - 1);
                }
            }

            // Remove current cell
            cells.Remove(cc);

            // Go to the next list of cells, update steps and weight
            if (cells.Count == 0) {
                // If there aren't any more cells, break out
                if (nextCells.Count == 0) {
                    return moves;
                }
                steps++;

                // Set new cells list and continue iterating
                foreach (var cell in nextCells)
                    cells.Add(cell);
                nextCells.Clear();
            }
        }

        // Default return
        return moves;
    }

    /// <summary>
    /// Checks if a given move is new and not in the move list.
    /// </summary>
    /// <param name="moveList">The list of moves to check with</param>
    /// <param name="potentialMove">A potentially new move</param>
    /// <returns>Whether or not the move is in the list</returns>
    private static bool NewMove(List<Move> moveList, Move potentialMove) {
        foreach (var move in moveList) {
            if (move.Capture == potentialMove.Capture &&
                move.From == potentialMove.From &&
                move.To == potentialMove.To) {
                return false;
            }
        }

        // Default return
        return true;
    }

    /// <summary>
    /// Returns true if the board does not wrap.
    /// This prevents movement from one edge of the board to the
    /// complete other side of the board.
    /// </summary>
    /// <param name="from">The current square</param>
    /// <param name="to">The next square</param>
    /// <returns>True if the board does not wrap, else false</returns>
    private static bool BoardWrapCheck(int from, int to) {
        return Mathf.Abs(Board.Row(from) - Board.Row(to)) <= 1 &&
               Mathf.Abs(Board.Col(from) - Board.Col(to)) <= 1;
    }
    #endregion
}
