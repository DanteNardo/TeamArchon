using UnityEngine;

/// <summary>
/// A Singleton that generates the board's tiles.
/// </summary>
public class TileManager : Singleton<TileManager> {
    #region TileManager Members
    public int pieceCount = 2;
    public GameObject lightTilePrefab;
    public GameObject darkTilePrefab;
    public GameObject[,] boardTiles;
    #endregion

    #region Board Methods
    /// <summary>
    /// Initializes the Tile data.
    /// </summary>
    private void Awake() {
        // Call base Singleton awake functionality.
        base.Awake();

        // Generate the tile gameobjects for the board
        boardTiles = new GameObject[Board.Size, Board.Size];
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
        for (int i = 0; i < Board.Size; i++) {

            // Switch between light and dark tiles to offset each row from the previous
            light = !light;

            for (int j = 0; j < Board.Size; j++) {

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
