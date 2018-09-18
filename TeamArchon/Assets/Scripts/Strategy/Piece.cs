using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public enum EPieceState {
	Unmoved,
	Moved,
    None
};
public enum EPieceColor {
    White,
    Black,
    None
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
public class Piece : MonoBehaviour {

	#region Piece Members
	public Material material;
	public Color defaultColor;
	public Color selectedColor;
    public EPieceType pieceType;
	public EPieceState pieceState;
    public EPieceColor pieceColor;
	public EDirection direction;
	public bool selected;
	private bool moving;
	#endregion

	#region Piece Properties
	public int X { get; private set; }
	public int Z { get; private set; }
    public List<Move> Moves { get; private set; }
    #endregion

    #region Piece Methods

    /// <summary>
    /// Get components and set initial position
    /// </summary>
    private void Start() {
        Moves = new List<Move>();
		material = GetComponent<MeshRenderer>().material;
        X = Mathf.FloorToInt(transform.position.x);
		Z = Mathf.FloorToInt(transform.position.y);
		pieceState = EPieceState.Unmoved;
	}

    /// <summary>
    /// Moves pieces when they are selected and the player tries to move them
    /// </summary>
    private void Update() {
		if (selected && pieceState == EPieceState.Unmoved && InputManager.Instance.MoveAttempt) {
            Move m = InputManager.Instance.InputMove;
            if (Rules.Instance.ValidMove(pieceType, pieceColor, m)) {
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
		// Select the piece if it is possible
		if (!selected && pieceState == EPieceState.Unmoved) {
            selected = true;
            material.color = selectedColor;
            InputManager.Instance.Selected = this;
        }

        // Deselect the piece if it is selected
		else if (pieceState == EPieceState.Unmoved) {
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
                Moves.Clear();
                X = (int)transform.position.x;
                Z = (int)transform.position.z;
				pieceState = EPieceState.Moved;
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
        foreach (var move in Moves) {
            if (m.From == move.From && m.To == move.To) {
                return true;
            }
        }

        // Default return
        return false;
    }
	#endregion
}
