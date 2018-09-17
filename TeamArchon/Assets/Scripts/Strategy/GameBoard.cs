using UnityEngine.Networking;

public class GameBoard : NetworkBehaviour {
    #region Members
    /// <summary>
    /// This single variable is a list of ints with a length equal to
    /// the variable size ^ 2. If you are looking for the piece at row
    /// five, column eight the index should be equal to (5 * size) + 8.
    /// You can use the Row and Col functions to get the row or column
    /// from an index in the list. Every variable in the list is an
    /// enumerator that has been cast to an integer.
    /// </summary>
    SyncListInt board;

    public int size = 8;
    #endregion

    #region Methods
    /// <summary>
    /// Creates important information for initialization.
    /// </summary>
    void Awake () {
        Instantiate();
	}

    /// <summary>
    /// Instantiates all of the game board's pieces.
    /// </summary>
    private void Instantiate() {
        board = new SyncListInt();
        for (int i = 0; i < size*size; i++) {
            board.Add((int)EPieceType.None);
        }
    }

    #region Utility Methods
    /// <summary>
    /// Converts the serialized integer data to an actual enum.
    /// </summary>
    /// <param name="typeAsInt">The integer that needs to be interpreted</param>
    /// <returns>The type of the piece represented by this integer</returns>
    public EPieceType PTAsEnum(int typeAsInt) {
        return (EPieceType)typeAsInt;
    }

    /// <summary>
    /// Converts the enumerator to an integer for serialization.
    /// </summary>
    /// <param name="typeAsEnum">The enum that needs to be cast to an integer for serialization/networking</param>
    /// <returns>The integer that represents an enum</returns>
    public int PTAsInt(EPieceType typeAsEnum) {
        return (int)typeAsEnum;
    }

    /// <summary>
    /// Returns the row from 0 -> size-1 that the index is in.
    /// </summary>
    /// <param name="index">The index in the board list</param>
    /// <returns>The row as an integer of the index</returns>
    public int Row(int index) {
        return index / size;
    }

    /// <summary>
    /// Returns the column from 0 -> size-1 that the index is in.
    /// </summary>
    /// <param name="index">The index in the board list</param>
    /// <returns>The column as an integer of the index</returns>
    public int Col(int index) {
        return index % size;
    }

    /// <summary>
    /// Converts two integers into a single index for the SyncListInt.
    /// </summary>
    /// <param name="row">The row placement of the piece</param>
    /// <param name="column">The column placement of the piece</param>
    /// <returns>A single integer that represents the row and column</returns>
    public int IndexFromRowAndCol(int row, int column) {
        return (row * size) + column;
    }

    /// <summary>
    /// Determines whether or not the x and y index exists to prevent out of bounds exceptions
    /// </summary>
    /// <param name="index">The index to check if a tile exists for</param>
    /// <returns>Whether the tile exist</returns>
    public bool TileExists(int index) {
        return Row(index) >= 0 && Row(index) < size && Col(index) >= 0 && Col(index) < size;
    }
    #endregion

    #region Piece Manipulation Methods
    /// <summary>
    /// Places a piece on the board.
    /// </summary>
    /// <param name="piece">A reference to the piece to move</param>
    public void PlacePiece(Piece piece) {
        board[IndexFromXZ(piece.X, piece.Z)] = PTAsInt(piece.pieceType);
    }

    /// <summary>
    /// Moves a piece using move object as data.
    /// </summary>
    /// <param name="m">The data necessary for movement</param>
    public void MovePiece(Move m) {
        board[m.To] = board[m.From];
        board[m.From] = 0;

        // Call piece movement events
        EventManager.Instance.TriggerEvent("PieceMoved");
    }

    /// <summary>
    /// Returns whether or not a piece is at the index specified.
    /// General purpose implementation.
    /// </summary>
    /// <param name="index">The index to check for a piece</param>
    /// <returns>True if there is a piece there, else false</returns>
    public bool PieceAt(int index) {
        return board[index] != PTAsInt(EPieceType.None) ? true : false;
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// Checks a square in a direction relative to the index provided.
    /// </summary>
    /// <param name="index">The origin index to check for a piece from</param>
    /// <param name="direction">The direction we are looking in</param>
    /// <returns>True if there is a piece in the specified direction, else false</returns>
    public bool PieceAt(int index, EDirection direction) {
        // Save the row and column for processing and prepare new index
        int row = Row(index);
        int col = Col(index);
        int newIndex;

        // Find the new index and check if a piece is there
        switch (direction) {
            case EDirection.North:
                newIndex = IndexFromRowAndCol(row + 1, col);
                break;
            case EDirection.South:
                newIndex = IndexFromRowAndCol(row - 1, col);
                break;
            case EDirection.East:
                newIndex = IndexFromRowAndCol(row, col + 1);
                break;
            case EDirection.West:
                newIndex = IndexFromRowAndCol(row, col - 1);
                break;
            case EDirection.Northeast:
                newIndex = IndexFromRowAndCol(row + 1, col + 1);
                break;
            case EDirection.Northwest:
                newIndex = IndexFromRowAndCol(row + 1, col - 1);
                break;
            case EDirection.Southeast:
                newIndex = IndexFromRowAndCol(row - 1, col + 1);
                break;
            case EDirection.Southwest:
                newIndex = IndexFromRowAndCol(row - 1, col - 1);
                break;
            default:
                newIndex = index;
                break;
        }
        
        if (!SquareExists(newIndex)) return false;
        return board[newIndex] != PTAsInt(EPieceType.None);
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// Checks a square in a direction relative to the index provided.
    /// Only returns true if the pieceType and pieceColor match.
    /// </summary>
    /// <param name="index">The index to check for a piece at</param>
    /// <param name="pieceType">The type of the piece we are checking for</param>
    /// <param name="direction">The direction we are looking in</param>
    /// <returns>True if there is a specific piece in the specified direction, else false</returns>
    public bool PieceAt(int index, EPieceType pieceType, EDirection direction) {
        // Save the row and column for processing and prepare new index
        int row = Row(index);
        int col = Col(index);
        int newIndex = -1;

        // Find the new index and check if a piece is there
        switch (direction) {
            case EDirection.North:
                newIndex = IndexFromRowAndCol(row + 1, col);
                break;
            case EDirection.South:
                newIndex = IndexFromRowAndCol(row - 1, col);
                break;
            case EDirection.East:
                newIndex = IndexFromRowAndCol(row, col + 1);
                break;
            case EDirection.West:
                newIndex = IndexFromRowAndCol(row, col - 1);
                break;
            case EDirection.Northeast:
                newIndex = IndexFromRowAndCol(row + 1, col + 1);
                break;
            case EDirection.Northwest:
                newIndex = IndexFromRowAndCol(row + 1, col - 1);
                break;
            case EDirection.Southeast:
                newIndex = IndexFromRowAndCol(row - 1, col + 1);
                break;
            case EDirection.Southwest:
                newIndex = IndexFromRowAndCol(row - 1, col - 1);
                break;
            default:
                newIndex = index;
                break;
        }

        if (!SquareExists(newIndex)) return false;
        return board[newIndex] != PTAsInt(EPieceType.None) &&
               board[newIndex] == PTAsInt(pieceType);
    }
    #endregion
    #endregion
}
