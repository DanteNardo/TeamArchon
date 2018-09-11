using UnityEngine;

/// <summary>
/// A Singleton that stores the pieces on the board and handles the board manipulation.
/// </summary>
public class BoardManager : Singleton<BoardManager> {
    #region Board Members
	public int boardSize = 8;
    public int pieceCount = 2;
	public Piece[,] gameBoard;
    public GameObject lightTilePrefab;
    public GameObject darkTilePrefab;
    public GameObject[,] boardTiles;
    private MoveGeneration moveGenerator;
    #endregion

    #region Board Methods
    /// <summary>
    /// Initializes the Board data.
    /// </summary>
	private void Start () {
        // Create MoveGenerator
        moveGenerator = new MoveGeneration();

        // Generate the digital board
		gameBoard = new Piece[boardSize, boardSize];

        // Generate movement
        moveGenerator.GenerateMoves();

        // Generate the gameobject board
        boardTiles = new GameObject[boardSize, boardSize];
        GenerateBoard();
	}

    /// <summary>
    /// Generates all of the Tile GameObjects.
    /// </summary>
    private void GenerateBoard() {
        // Create a new empty child that stores all of the board tiles
        GameObject tileContainer = new GameObject();
        tileContainer.transform.parent = gameObject.transform;
        tileContainer.name = "TileContainer";

        // Iterate through the entire board
        bool light = true;
        for (int i = 0; i < boardSize; i++) {

            // Switch between light and dark tiles to offset each row from the previous
            light = !light;

            for (int j = 0; j < boardSize; j++) {

                // Generate world space coordinate position
                Vector3 position = new Vector3(i, -0.55f, j);

                // Generate a light or dark tile
                if (light) {
                    GameObject lightTile = Instantiate(lightTilePrefab, position, Quaternion.identity);
                    lightTile.transform.parent = tileContainer.transform;
                    boardTiles[i, j] = lightTile;
                }
                else {
                    GameObject darkTile = Instantiate(darkTilePrefab, position, Quaternion.identity);
                    darkTile.transform.parent = tileContainer.transform;
                    boardTiles[i, j] = darkTile;
                }

                // Switch between light and dark tiles
                light = !light;
            }
        }
    }

    /// <summary>
    /// Called to reset all piece states.
    /// </summary>
    public void ResetPieceStates() {
        // Iterate through board
        for (int i = 0; i < boardSize; i++) {
            for (int j = 0; j < boardSize; j++) {
                gameBoard[i, j].pieceState = EPieceState.Unmoved;
            }
        }
    }
    
    /// <summary>
    /// Places a piece on the board.
    /// (unsafe and not used for moving just initial placement)
    /// </summary>
    /// <param name="piece">A reference to the piece to move</param>
	public void Place(Piece piece) {
		gameBoard[piece.X, piece.Z] = piece;
	}
    
    /// <summary>
    /// Moves a piece using move object as data.
    /// </summary>
    /// <param name="m">The data necessary for movement</param>
	public void MovePiece(Move m) {
        gameBoard[m.TX, m.TZ] = gameBoard[m.FX, m.FZ];
        gameBoard[m.FX, m.FZ] = null;

        // Call piece movement events
        switch (gameBoard[m.TX, m.TZ].pieceType) {
            case EPieceType.Wall:
                EventManager.Instance.TriggerEvent("WallMoved");
                break;
            case EPieceType.Gladiator:
                EventManager.Instance.TriggerEvent("GladiatorMoved");
                break;
            case EPieceType.Minotaur:
                EventManager.Instance.TriggerEvent("MinotaurMoved");
                break;
        }

        // Calculate all new possible moves
        moveGenerator.GenerateMoves();
    }

    /// <summary>
    /// Returns whether or not a piece is at the location specified.
    /// General purpose implementation.
    /// </summary>
    /// <param name="x">Index X</param>
    /// <param name="z">Index Z</param>
    /// <returns>True if there is a piece there, else false</returns>
    public bool PieceAt(int x, int z) {
        return gameBoard[x, z] != null ? true : false;
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
                return gameBoard[x + 1, z + 1] != null;

            case EDirection.Northwest:
                if (!SquareExists(x - 1, z + 1)) return false;
                return gameBoard[x - 1, z + 1] != null;

            case EDirection.North:
                if (!SquareExists(x, z + 1)) return false;
                return gameBoard[x, z + 1] != null;

            case EDirection.Southeast:
                if (!SquareExists(x + 1, z - 1)) return false;
                return gameBoard[x + 1, z - 1] != null;

            case EDirection.Southwest:
                if (!SquareExists(x - 1, z - 1)) return false;
                return gameBoard[x - 1, z - 1] != null;

            case EDirection.South:
                if (!SquareExists(x, z - 1)) return false;
                return gameBoard[x, z - 1] != null;

            case EDirection.East:
                if (!SquareExists(x + 1, z)) return false;
                return gameBoard[x + 1, z] != null;

            case EDirection.West:
                if (!SquareExists(x - 1, z)) return false;
                return gameBoard[x - 1, z] != null;
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
                return gameBoard[x + 1, z + 1] != null &&
                       gameBoard[x + 1, z + 1].pieceType == pieceType &&
                       gameBoard[x + 1, z + 1].pieceColor == pieceColor;

            case EDirection.Northwest:
                if (!SquareExists(x - 1, z + 1)) return false;
                return gameBoard[x - 1, z + 1] != null &&
                       gameBoard[x - 1, z + 1].pieceType == pieceType &&
                       gameBoard[x - 1, z + 1].pieceColor == pieceColor;

            case EDirection.North:
                if (!SquareExists(x, z + 1)) return false;
                return gameBoard[x, z + 1] != null &&
                       gameBoard[x, z + 1].pieceType == pieceType &&
                       gameBoard[x, z + 1].pieceColor == pieceColor;

            case EDirection.Southeast:
                if (!SquareExists(x + 1, z - 1)) return false;
                return gameBoard[x + 1, z - 1] != null &&
                       gameBoard[x + 1, z - 1].pieceType == pieceType &&
                       gameBoard[x + 1, z - 1].pieceColor == pieceColor;

            case EDirection.Southwest:
                if (!SquareExists(x - 1, z - 1)) return false;
                return gameBoard[x - 1, z - 1] != null &&
                       gameBoard[x - 1, z - 1].pieceType == pieceType &&
                       gameBoard[x - 1, z - 1].pieceColor == pieceColor;

            case EDirection.South:
                if (!SquareExists(x, z - 1)) return false;
                return gameBoard[x, z - 1] != null &&
                       gameBoard[x, z - 1].pieceType == pieceType &&
                       gameBoard[x, z - 1].pieceColor == pieceColor;

            case EDirection.East:
                if (!SquareExists(x + 1, z)) return false;
                return gameBoard[x + 1, z] != null &&
                       gameBoard[x + 1, z].pieceType == pieceType &&
                       gameBoard[x + 1, z].pieceColor == pieceColor;

            case EDirection.West:
                if (!SquareExists(x - 1, z)) return false;
                return gameBoard[x - 1, z] != null &&
                       gameBoard[x - 1, z].pieceType == pieceType &&
                       gameBoard[x - 1, z].pieceColor == pieceColor;
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
        return x >= 0 && x < boardSize && z >= 0 && z < boardSize;
    }
    #endregion
}
