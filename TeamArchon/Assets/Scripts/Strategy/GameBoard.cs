using UnityEngine;
using UnityEngine.Networking;

public class GameBoard : NetworkBehaviour {
    #region Members
    public int size = 8;
    public Piece[,] board;
    #endregion

    // Use this for initialization
    void Start () {
        Instantiate();
	}

    /// <summary>
    /// Instantiates all of the game board's pieces.
    /// </summary>
    private void Instantiate() {
        board = new Piece[size, size];
    }

    /// <summary>
    /// Places a piece on the board.
    /// (unsafe and not used for moving just initial placement)
    /// </summary>
    /// <param name="piece">A reference to the piece to move</param>
    public void Place(Piece piece) {
        board[piece.X, piece.Z] = piece;
    }

    /// <summary>
    /// Moves a piece using move object as data.
    /// </summary>
    /// <param name="m">The data necessary for movement</param>
    public void MovePiece(Move m) {
        board[m.TX, m.TZ] = board[m.FX, m.FZ];
        board[m.FX, m.FZ] = null;

        // Call piece movement events
        switch (board[m.TX, m.TZ].pieceType) {
            case EPieceType.Wall:
                EventManager.Instance.TriggerEvent("WallMoved");
                break;
        }
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// General purpose implementation.
    /// </summary>
    /// <param name="x">Index X</param>
    /// <param name="z">Index Z</param>
    /// <returns>True if there is a piece there, else false</returns>
    public bool PieceAt(int x, int z) {
        return board[x, z] != null ? true : false;
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// Checks a square in a direction relative to indeces provided.
    /// </summary>
    /// <param name="x">Index X</param>
    /// <param name="z">Index Z</param>
    /// <param name="direction">The direction we are looking in</param>
    /// <returns>True if there is a piece in the specified direction, else false</returns>
    public bool PieceAt(int x, int z, EDirection direction) {
        switch (direction) {
            case EDirection.Northeast:
                if (!SquareExists(x + 1, z + 1)) return false;
                return board[x + 1, z + 1] != null;

            case EDirection.Northwest:
                if (!SquareExists(x - 1, z + 1)) return false;
                return board[x - 1, z + 1] != null;

            case EDirection.North:
                if (!SquareExists(x, z + 1)) return false;
                return board[x, z + 1] != null;

            case EDirection.Southeast:
                if (!SquareExists(x + 1, z - 1)) return false;
                return board[x + 1, z - 1] != null;

            case EDirection.Southwest:
                if (!SquareExists(x - 1, z - 1)) return false;
                return board[x - 1, z - 1] != null;

            case EDirection.South:
                if (!SquareExists(x, z - 1)) return false;
                return board[x, z - 1] != null;

            case EDirection.East:
                if (!SquareExists(x + 1, z)) return false;
                return board[x + 1, z] != null;

            case EDirection.West:
                if (!SquareExists(x - 1, z)) return false;
                return board[x - 1, z] != null;
        }

        return false;
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// Checks a square in a direction relative to indeces provided.
    /// Only returns true if the pieceType and pieceColor match.
    /// </summary>
    /// <param name="x">Index X</param>
    /// <param name="z">Index Z</param>
    /// <param name="pieceType">The type of the piece we are checking for</param>
    /// <param name="pieceColor">The color of the piece we are checking for</param>
    /// <param name="direction">The direction we are looking in</param>
    /// <returns>True if there is a specific piece in the specified direction, else false</returns>
    public bool PieceAt(int x, int z, EPieceType pieceType, EPieceColor pieceColor, EDirection direction) {
        switch (direction) {
            case EDirection.Northeast:
                if (!SquareExists(x + 1, z + 1)) return false;
                return board[x + 1, z + 1] != null &&
                       board[x + 1, z + 1].pieceType == pieceType &&
                       board[x + 1, z + 1].pieceColor == pieceColor;

            case EDirection.Northwest:
                if (!SquareExists(x - 1, z + 1)) return false;
                return board[x - 1, z + 1] != null &&
                       board[x - 1, z + 1].pieceType == pieceType &&
                       board[x - 1, z + 1].pieceColor == pieceColor;

            case EDirection.North:
                if (!SquareExists(x, z + 1)) return false;
                return board[x, z + 1] != null &&
                       board[x, z + 1].pieceType == pieceType &&
                       board[x, z + 1].pieceColor == pieceColor;

            case EDirection.Southeast:
                if (!SquareExists(x + 1, z - 1)) return false;
                return board[x + 1, z - 1] != null &&
                       board[x + 1, z - 1].pieceType == pieceType &&
                       board[x + 1, z - 1].pieceColor == pieceColor;

            case EDirection.Southwest:
                if (!SquareExists(x - 1, z - 1)) return false;
                return board[x - 1, z - 1] != null &&
                       board[x - 1, z - 1].pieceType == pieceType &&
                       board[x - 1, z - 1].pieceColor == pieceColor;

            case EDirection.South:
                if (!SquareExists(x, z - 1)) return false;
                return board[x, z - 1] != null &&
                       board[x, z - 1].pieceType == pieceType &&
                       board[x, z - 1].pieceColor == pieceColor;

            case EDirection.East:
                if (!SquareExists(x + 1, z)) return false;
                return board[x + 1, z] != null &&
                       board[x + 1, z].pieceType == pieceType &&
                       board[x + 1, z].pieceColor == pieceColor;

            case EDirection.West:
                if (!SquareExists(x - 1, z)) return false;
                return board[x - 1, z] != null &&
                       board[x - 1, z].pieceType == pieceType &&
                       board[x - 1, z].pieceColor == pieceColor;
        }

        return false;
    }

    /// <summary>
    /// Determines whether or not the x and y index exists to prevent out of bounds exceptions
    /// </summary>
    /// <param name="x">X index</param>
    /// <param name="z">Z Index</param>
    /// <returns>Whether the indeces exist</returns>
    public bool SquareExists(int x, int z) {
        return x >= 0 && x < size && z >= 0 && z < size;
    }
}
