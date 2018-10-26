/// <summary>
/// Handles all input on the client side.
/// </summary>
public class InputManager : Singleton<InputManager> {
    #region InputManager Properties
    public Piece Selected { get; set; }
    public bool MoveAttempt { get; private set; }
    public Move InputMove { get; private set; }
    #endregion

    #region InputManager Methods
    /// <summary>
    /// A move has been attempted.
    /// </summary>
    /// <param name="m">The attempted move</param>
    public void AttemptMove(Move m) {
        MoveAttempt = true;
        InputMove = m;
    }

    /// <summary>
    /// Used to clear the data after the move has been attempted.
    /// </summary>
    public void MoveAttemptMade() {
        MoveAttempt = false;
        InputMove = null;
        Selected.spriteRenderer.color = Selected.defaultColor;
        Selected.selected = false;
        Selected = null;
    }
    #endregion
}
