using UnityEngine;

/// <summary>
/// A Singleton that stores the pieces on the board and handles the board manipulation.
/// </summary>
public class BoardManager : Singleton<BoardManager> {
    #region Board Members
    public int pieceCount = 2;
    public GameBoard gameBoard;
    public GameObject lightTilePrefab;
    public GameObject darkTilePrefab;
    public GameObject[,] boardTiles;
    #endregion

    #region Board Properties
    public int Size { get { return gameBoard.size; } }
    public Piece[,] Board { get { return gameBoard.board; } }
    #endregion

    #region Board Methods
    /// <summary>
    /// Initializes the Board data.
    /// </summary>
    private void Awake() {
        gameBoard = GetComponent<GameBoard>();
        // Generate the gameobject board
        boardTiles = new GameObject[gameBoard.size, gameBoard.size];
        GenerateBoard();
	}

    /// <summary>
    /// Called to reset all piece states.
    /// </summary>
    public void ResetPieceStates() {
        // Iterate through board
        for (int i = 0; i < gameBoard.size; i++) {
            for (int j = 0; j < gameBoard.size; j++) {
                gameBoard.board[i, j].pieceState = EPieceState.Unmoved;
            }
        }
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
        for (int i = 0; i < gameBoard.size; i++) {

            // Switch between light and dark tiles to offset each row from the previous
            light = !light;

            for (int j = 0; j < gameBoard.size; j++) {

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
    #endregion
}
