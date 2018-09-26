using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages a specific set of pieces based on given input.
/// </summary>
public class SquadManager : NetworkBehaviour {
    #region Members
    public GameObject[] prefabs;
    private GameObject[] pieceObjects;
    private Piece[] pieces;

    [SyncVar]
    public bool team;
    #endregion

    #region Methods
    /// <summary>
    /// Initializes important variables.
    /// </summary>
    private void Start() {
        if (isLocalPlayer) {
            gameObject.name = "localController";
            CmdInstantiatePieces();
        }
    }
    
    /// <summary>
    /// Creates all of the pieces from prefabs and saves their data.
    /// </summary>
    [Command]
    private void CmdInstantiatePieces() {
        pieceObjects = new GameObject[prefabs.Length];
        pieces = new Piece[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++) {
            var instance = Instantiate(prefabs[i], Vector3.zero, Quaternion.identity);
            Piece piece = instance.GetComponent<Piece>();
            pieces[i] = piece;

            // Sets the new objects parrent ID to this controllers unique network ID
            piece.parentNetId = netId;

            // Set the parrent on this client
            instance.transform.parent = gameObject.transform;
            GameBoard.Instance.PlacePiece(piece);
            NetworkServer.Spawn(instance);
        }
    }

    public bool checkLocalPlayer() {
        return isLocalPlayer;
    } 
    #endregion
}
