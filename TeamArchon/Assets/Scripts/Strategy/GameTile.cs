using UnityEngine;

public class GameTile : MonoBehaviour {
    #region Methods
    public int Row;
    public int Col;
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
            Debug.Log("Piece Row&Col: " + InputManager.Instance.Selected.Z + " - " + InputManager.Instance.Selected.X);
            Debug.Log("Piece Index: " + InputManager.Instance.Selected.Index);
            Debug.Log("Tile Row&Col: " + Row + " - " + Col);
            Move move = new Move(InputManager.Instance.Selected.Index, Board.IndexFromRowAndCol(Row, Col));
            Debug.Log("New Move: " + move.From + " - " + move.To);
            Debug.Log("Invalid Move?: " + move.Invalid);
            InputManager.Instance.AttemptMove(move);
        }
    }
    #endregion
}
