using UnityEngine;

/// <summary>
/// A Singleton that generates legal moves based on a Board position.
/// </summary>
public class Rules : MonoBehaviour {
    #region Members
    public static Rules Instance;
    #endregion

    #region Properties
    public bool MinotaurTurn { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the Rules Singleton.
    /// </summary>
    private void Awake() {
        if (Instance != null) {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
            case EPieceType.Wall:
                return ValidWallMove(color, m);
        }

        return false;
    }

    /// <summary>
    /// Checks if a Wall move is valid.
    /// </summary>
    /// <param name="color">Color of the Wall</param>
    /// <param name="m">The move data</param>
    /// <returns>Tre if the Wall move is valid, else false</returns>
    private bool ValidWallMove(EPieceColor color, Move m) {
        // Check to make sure the space isn't occupied
        return BoardManager.Instance.Board[m.TX, m.TZ] == null;
    }

    /// <summary>
    /// Checks if a piece is surrounded (N, S, E, & W)
    /// </summary>
    /// <param name="piece">The piece to check</param>
    /// <returns>True if the piece is surrounded, else false</returns>
    private bool Surrounded(Piece piece) {
        // Check all four movement possibilities
        return BoardManager.Instance.gameBoard.PieceAt(piece.X, piece.Z, EDirection.North) &&
               BoardManager.Instance.gameBoard.PieceAt(piece.X, piece.Z, EDirection.South) &&
               BoardManager.Instance.gameBoard.PieceAt(piece.X, piece.Z, EDirection.East) &&
               BoardManager.Instance.gameBoard.PieceAt(piece.X, piece.Z, EDirection.West);
    }
    #endregion
}
