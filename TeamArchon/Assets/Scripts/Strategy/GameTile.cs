using UnityEngine;

public class GameTile : GamepadBehavior {
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
    public override void OnClick(GamepadCursor cursor) {
        // Create an attempted move if there is a selected piece
        if (InputManager.Instance.Selected != null) {
            // Generate potential move and save it in InputManager
            Debug.Log("Selected: " + InputManager.Instance.Selected);
            Debug.Log("Piece Row&Col: " + InputManager.Instance.Selected.Z + " - " + InputManager.Instance.Selected.X);
            Debug.Log("Piece Index: " + InputManager.Instance.Selected.Index);
            Debug.Log("Tile Row&Col: " + Row + " - " + Col);

            // Check if this move is a capture
            Move move;
            if (Piece.IsOtherColor(
                GameBoard.Instance[InputManager.Instance.Selected.Index], 
                GameBoard.Instance[Board.IndexFromRowAndCol(Row, Col)])) {
                move = new Move(InputManager.Instance.Selected.Index, Board.IndexFromRowAndCol(Row, Col), true);
            }
            else {
                move = new Move(InputManager.Instance.Selected.Index, Board.IndexFromRowAndCol(Row, Col));
            }
            Debug.Log("New Move: " + move.From + " - " + move.To);
            Debug.Log("Invalid Move?: " + move.Invalid);

            // Tell the input manager (which tells the pieces) a move has been attempted
            InputManager.Instance.AttemptMove(move);
        }
    }
    #endregion
}
