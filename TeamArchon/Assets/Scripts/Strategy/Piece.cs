using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Piece Enumerators
public enum EPieceType {
    None,
    LPistol,
    LMachineGun,
    LSniper,
    LShotgun,
    DPistol,
    DMachineGun,
    DSniper,
    DShotgun
    
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
[RequireComponent(typeof(SpriteRenderer))]
public class Piece : GamepadBehavior {
	#region Piece Members
	public SpriteRenderer spriteRenderer;
	public Color defaultColor;
	public Color selectedColor;
    public EPieceType pieceType;
	public EDirection direction;
    public int X; // Column
    public int Z; // Row
	public bool selected;
    public int speed = 3;
	private bool moving;
    #endregion

    #region Piece Properties
    public int Index {         // Row & Column as a single integer
        get {
            return Board.IndexFromRowAndCol(Z, X);
        }
    }
    public int Speed { get { return speed; } }
    public float Health = 400;
    public List<Move> Moves { get; private set; }
    public Player player { get; set; }
    public GameTile Tile { get; private set; }
    #endregion

    #region Piece Methods
    /// <summary>
    /// Get components and set initial position
    /// </summary>
    private void Awake() {
        Moves = new List<Move>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        X = Mathf.FloorToInt(transform.position.x);
		Z = Mathf.FloorToInt(transform.position.z);
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
                // If the piece is moved internally, move it visually as well
                if (GameBoard.Instance.MovePiece(m)) {
                    Move(m);
                }
			}
            InputManager.Instance.MoveAttemptMade();
		}

        // Check if we know what GameTile we have
        if (Tile == null) {
            GetGameTile();
        }
	}

    /// <summary>
    /// Select or deselect the piece when it is clicked on (if it can be selected)
    /// </summary>
    public override void OnClick(GamepadCursor cursor) {
        // Only the correct player can click on this piece
        if (cursor.player == player) {
            // Select the piece if it is possible
            if (!selected) {
                // Deselect old piece and select this piece
                if (InputManager.Instance.Selected != null) {
                    Debug.Log("Deselected");
                    InputManager.Instance.Selected.Deselect();
                }
                Select();
            }
            // Deselect the piece if it is selected
            else {
                Deselect();
            }
        }
        // If there is a selected piece then a capture is being attempted
        else if (InputManager.Instance.Selected != null) {
            // Check if this move is a capture
            Move move;
            if (IsOtherColor(
                GameBoard.Instance[InputManager.Instance.Selected.Index],
                GameBoard.Instance[Index])) {
                move = new Move(InputManager.Instance.Selected.Index, Index, true, Tile);
                InputManager.Instance.AttemptMove(move);
            }
        }
	}

    /// <summary>
    /// Select this piece.
    /// </summary>
    public void Select() {
        selected = true;
        spriteRenderer.color = selectedColor;
        InputManager.Instance.Selected = this;
    }

    /// <summary>
    /// Select this piece.
    /// </summary>
    public void Deselect() {
        selected = false;
        spriteRenderer.color = defaultColor;
        InputManager.Instance.Selected = null;
    }

    /// <summary>
    /// Raycasts to find the GameTile beneath this piece.
    /// </summary>
    public void GetGameTile() {
        // Raycast down and hit object that has been clicked on
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1000);

        // Handle hit object if it has a GameTile behavior
        GameTile tile;
        if (hit.collider != null && hit.collider.gameObject != null &&
           (tile = hit.collider.gameObject.GetComponent<GameTile>()) != null) {
            // Reset the objective's occupation status
            if (Tile != null) Tile.Occupation = ETeam.None;
            Tile = tile;

            // Set the new objective's occupation status
            if (IsDark(pieceType)) Tile.Occupation = ETeam.Dark;
            else Tile.Occupation = ETeam.Light;
        }
    }

    /// <summary>
    /// Moves the piece visually.
    /// </summary>
    public void Move(Move m) {
        Debug.Log("Valid Move");
        Tile = null;
        Moves.Clear();
        X = Board.Col(m.To);
        Z = Board.Row(m.To);
        StartCoroutine(Moving(m));
    }

    /// <summary>
    /// Animates the piece from one square to another.
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
                X = Mathf.FloorToInt(transform.position.x);
                Z = Mathf.FloorToInt(transform.position.z);
                moving = false;
                GetGameTile();
                yield break;
			}
			else yield return null;
		}
    }

    /// <summary>
    /// Converts next board position to world position.
    /// </summary>
    /// <param name="m">The move data</param>
    /// <returns>The world position</returns>
    private Vector3 FromPosition(Move m) {
        return new Vector3(Board.Col(m.From), transform.position.y, Board.Row(m.From));
    }

    /// <summary>
    /// Converts next board position to world position.
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
        Debug.Log("Piece Row&Col: " + Z + " - " + X);
        Debug.Log("Current Index: " + Index);
        Debug.Log("Move Row&Col: " + m.From + " - " + m.To);
        Debug.Log("Moves:");
        foreach (var move in Moves) {
            Debug.Log("Move From: " + move.From + " Move To: " + move.To);
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
        return type == EPieceType.LPistol ||
               type == EPieceType.LMachineGun;
    }

    /// <summary>
    /// Determines if a piece is dark from its type.
    /// </summary>
    /// <param name="type">The type of the piece</param>
    /// <returns>True or false</returns>
    public static bool IsDark(EPieceType type) {
        return type == EPieceType.DPistol ||
               type == EPieceType.DMachineGun;
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
