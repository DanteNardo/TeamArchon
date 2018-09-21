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
        //Debug.Log(gameObject.name);
        if (isLocalPlayer)
        {
            gameObject.name = "localController";
            Debug.Log(transform.childCount);
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
            GameBoard.Instance.PlacePiece(piece);
            NetworkServer.Spawn(instance);

            RpcSetChild(this.gameObject, instance);
            //instance.transform.parent = transform;
        }
    }

    [ClientRpc]
    public void RpcSetChild(GameObject parrent, GameObject child)
    {
        
        if (child.transform.parent != parrent.transform)
        {
            child.transform.parent = parrent.transform;
        }
        
    }

    

    public bool checkLocalPlayer()
    {
        return isLocalPlayer;
    }

    
    #endregion
}
