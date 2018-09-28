using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region Piece Enumerators
public enum EPieceType {
    None,
    LScout,
    LShotgun,
    LSniper,
    LGrenade,
    LMachineGun,
    DScout,
    DShotgun,
    DSniper,
    DGrenade,
    DMachineGun
};
public enum EDirection {
	North,
	South,
	East,
	West,
    Northeast,
    Northwest,
    Southeast,
    Southwest,
    None
};
#endregion

/// <summary>
/// A parent to every possible piece type. Implements generic piece abilities.
/// </summary>
public class Piece : NetworkBehaviour {
	#region Piece Members
	public Material material;
	public Color defaultColor;
	public Color selectedColor;
    public EPieceType pieceType;
	public EDirection direction;
	public bool selected;
    public int speed = 3;
	private bool moving;
    public bool localPiece;
    //Synced across the server for each client
    [SyncVar]
    public NetworkInstanceId parentNetId;
    #endregion

    #region Piece Properties
    public int X { get; set; }
	public int Z { get; set; }
    public int Index {
        get {
            return Board.IndexFromRowAndCol(X, Z);
        }
    }
    public int Speed { get { return speed; } }
    public List<Move> Moves { get; private set; }
    #endregion

    #region Piece Methods
    /// <summary>
    /// Get components and set initial position
    /// </summary>
    private void Awake() {
        Moves = new List<Move>();
		material = GetComponent<MeshRenderer>().material;
        X = Mathf.FloorToInt(transform.position.x);
		Z = Mathf.FloorToInt(transform.position.z);
    }

    /// <summary>
    /// Runs localy whenever a client connects to the server for each piece in the game. Connects each piece to each player.
    /// </summary>
    public override void OnStartClient()
    {
        GameObject parrent = ClientScene.FindLocalObject(parentNetId);
        transform.SetParent(parrent.transform);
        //set if the piece is local
        localPiece = transform.parent.GetComponent<SquadManager>().checkLocalPlayer();
    }

    /// <summary>
    /// Moves pieces when they are selected and the player tries to move them
    /// </summary>
    private void Update() {
        // Check if a player is attempting to move this piece
		if (selected && InputManager.Instance.MoveAttempt) {
            Move m = InputManager.Instance.InputMove;

            // Move the piece if the move is valid
            if (Rules.Instance.ValidMove(pieceType, m) && HasMove(m)) {
                Debug.Log("Valid Move");
                Moves.Clear();
                X = Board.Row(m.To);
                Z = Board.Col(m.To);
                GameBoard.Instance.MovePiece(m);
				StartCoroutine(Moving(m));
			}
            InputManager.Instance.MoveAttemptMade();
		}
	}

    /// <summary>
    /// Select or deselect the piece when it is clicked on (if it can be selected)
    /// </summary>
    private void OnMouseDown() {
        // Only the player that owns this piece can select it
        if (!localPiece) {
            return;
        }

        // Select the piece if it is possible
        if (!selected) {
            selected = true;
            material.color = selectedColor;
            InputManager.Instance.Selected = this;
        }
        // Deselect the piece if it is selected
		else {
            selected = false;
            material.color = defaultColor;
            InputManager.Instance.Selected = null;
        }
	}

    /// <summary>
    /// // Animates the piece from one square to another
    /// </summary>
    /// <param name="m">The move data</param>
    /// <returns>An Enumerator for the coroutine</returns>
    private IEnumerator Moving(Move m) {
		// Initialize variables and set moving to true
		Vector3 from = transform.position;
		Vector3 to = NextPosition(m);
		float time = 0.0f;
		moving = true;

		// Update time and position until animation is complete
		while (true) {
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(from, to, time);

			// Animation complete
			if (transform.position == to) {

                // Clear our moves, set our new position, and change our state
                X = (int)transform.position.x;
                Z = (int)transform.position.z;
				moving = false;
				yield break;
			}
			else yield return null;
		}
    }

    /// <summary>
    /// Converts next board position to world position
    /// </summary>
    /// <param name="m">The move data</param>
    /// <returns>The world position</returns>
    private Vector3 FromPosition(Move m) {
        return new Vector3(Board.Col(m.From), transform.position.y, Board.Row(m.From));
    }

    /// <summary>
    /// Converts next board position to world position
    /// </summary>
    /// <param name="m">The move data</param>
    /// <returns>The world position</returns>
    private Vector3 NextPosition(Move m) {
        return new Vector3(Board.Col(m.To), transform.position.y, Board.Row(m.To));
	}

    /// <summary>
    /// Iterates through the piece's move list to see if it contains the same move data.
    /// </summary>
    /// <param name="m">The move data for comparison</param>
    /// <returns>True if contained, else false</returns>
    private bool HasMove(Move m) {
        Debug.Log("MOVE CHECK: " + m.From + " - " + m.To);
        Debug.Log("Moves:");
        foreach (var move in Moves) {
            Debug.Log("Move: " + move.From + " - " + move.To);
            if (m.From == move.From && m.To == move.To) {
                return true;
            }
        }

        // Default return
        return false;
    }

    /// <summary>
    /// Determines if a piece is light from its type.
    /// </summary>
    /// <param name="type">The type of the piece</param>
    /// <returns>True or false</returns>
    public static bool IsLight(EPieceType type) {
        return type == EPieceType.LSniper ||
               type == EPieceType.LShotgun ||
               type == EPieceType.LScout ||
               type == EPieceType.LMachineGun ||
               type == EPieceType.LGrenade;
    }

    /// <summary>
    /// Determines if a piece is dark from its type.
    /// </summary>
    /// <param name="type">The type of the piece</param>
    /// <returns>True or false</returns>
    public static bool IsDark(EPieceType type) {
        return type == EPieceType.DSniper ||
               type == EPieceType.DShotgun ||
               type == EPieceType.DScout ||
               type == EPieceType.DMachineGun ||
               type == EPieceType.DGrenade;
    }

    /// <summary>
    /// Determines if two pieces are opposite colors.
    /// </summary>
    /// <param name="type1">The type of the first piece</param>
    /// <param name="type2">The type of the second piece</param>
    /// <returns>True or false</returns>
    public static bool IsOtherColor(EPieceType type1, EPieceType type2) {
        return (IsLight(type1) && IsDark(type2)) ||
               (IsDark(type1) && IsLight(type2));
    }
	#endregion
}
