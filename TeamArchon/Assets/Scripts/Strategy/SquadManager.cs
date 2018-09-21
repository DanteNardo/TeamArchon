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
    #endregion

    #region Methods
    /// <summary>
    /// Initializes important variables.
    /// </summary>
    private void Start() {
        //CmdInstantiatePieces();
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
            GameBoard.Instance.PlacePiece(piece);
            NetworkServer.Spawn(instance);
            instance.transform.parent = transform;
        }
    }

    public bool checkLocalPlayer()
    {
        return isLocalPlayer;
    }
    #endregion
}
