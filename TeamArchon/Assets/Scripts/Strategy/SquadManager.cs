using UnityEngine;

/// <summary>
/// Manages a specific set of pieces based on given input.
/// </summary>
public class SquadManager : MonoBehaviour {
    #region Members
    public GameObject[] prefabs;
    private GameObject[] pieceObjects;
    private Piece[] pieces;
    public int team;
    private Player player;
    #endregion

    #region Methods
    /// <summary>
    /// Initializes squad pieces for local player.
    /// </summary>
    private void Awake() {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Creates all of the pieces from prefabs and saves their data.
    /// </summary>
    public void InstantiatePieces(int team, int teamPosition) {
        Debug.Log("...Instantiating pieces");
        // Store piece objects and piece prefabs
        pieceObjects = new GameObject[prefabs.Length];
        pieces = new Piece[prefabs.Length];

        // Set the team of this player's squad
        this.team = team;

        if (prefabs.Length == 0) {
            Debug.Log("This bitch empty!");
        }

        // Generate pieces for this player
        for (int i = 0; i < prefabs.Length; i++) {
            Debug.Log("...Instantiating pieces in for loop");
            // Determine the placement for this piece based on amount of players
            int row = 0, col = 0;
            
            // Light team is in rows 0-1
            if ((ETeam)this.team == ETeam.Light) {
                switch (i) {
                    case 0: row = 0; col = 0 + 2 * teamPosition; break;
                    case 1: row = 1; col = 0 + 2 * teamPosition; break;
                    case 2: row = 0; col = 1 + 2 * teamPosition; break;
                    case 3: row = 1; col = 1 + 2 * teamPosition; break;
                }
            }
            // Dark team is in rows 6-7
            else {
                switch (i) {
                    case 0: row = 7; col = 0 + 2 * teamPosition; break;
                    case 1: row = 6; col = 0 + 2 * teamPosition; break;
                    case 2: row = 7; col = 1 + 2 * teamPosition; break;
                    case 3: row = 6; col = 1 + 2 * teamPosition; break;
                }
            }

            // Create real world position
            Vector3 position = new Vector3(col, 0, row);

            // Create piece gameobject
            var instance = Instantiate(prefabs[i], position, Quaternion.identity);
            Piece piece = instance.GetComponent<Piece>();
            pieces[i] = piece;
            piece.player = player;

            // Set the parent for this child object and place the piece
            instance.transform.parent = gameObject.transform;
            GameBoard.Instance.PlacePiece(piece);
        }
        // Finish instantiating player pieces and increase player count
        StrategyGame.Instance.IncreasePlayerCount();
    }
    #endregion
}
