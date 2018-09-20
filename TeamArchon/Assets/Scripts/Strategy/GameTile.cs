using UnityEngine;

public class GameTile : MonoBehaviour {
    #region Properties
    public int Row { get; private set; }
    public int Col { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Determines starting row and column.
    /// </summary>
    private void Start() {
        Row = Mathf.FloorToInt(transform.position.z);
		Col = Mathf.FloorToInt(transform.position.x);
    }

    /// <summary>
    /// Generates an attempted move/place if the tile is clicked.
    /// </summary>
    private void OnMouseDown() {
        // Create an attempted move if there is a selected piece
        if (InputManager.Instance.Selected != null) {
            // Generate potential move and save it in InputManager
            Debug.Log("Selected: " + InputManager.Instance.Selected);
            Debug.Log("Tile Row&Col: " + Row + " - " + Col);
            Move move = new Move(InputManager.Instance.Selected.Index, Board.IndexFromRowAndCol(Row, Col));
            Debug.Log("Invalid Move?: " + move.Invalid);
            InputManager.Instance.AttemptMove(move);
        }
    }
    #endregion
}
