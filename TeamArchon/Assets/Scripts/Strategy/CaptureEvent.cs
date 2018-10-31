using UnityEngine.Events;

/// <summary>
/// An overridden implementation of UnityEvents that requires
/// a Move variable as a paramter when invoking the event.
/// </summary>
[System.Serializable]
public class CaptureEvent : UnityEvent<Move> { }

/// <summary>
/// This class is used to send data from the Strategy portion
/// of the game to the Action portion of the game. In order to
/// determine the Action portion we need to know the two pieces
/// involved and whose turn it is.
/// </summary>
public class CaptureData {
    #region Properties
    public Move CaptureMove { get; private set; }
    public EPieceType LightPiece { get; private set; }
    public EPieceType DarkPiece { get; private set; }
    public int Turn { get; private set; }
    public GameTile Tile { get { return CaptureMove.Tile; } }
    #endregion

    #region Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="captureMove">The move that generated this capture</param>
    /// <param name="light">The light piece in the capture event</param>
    /// <param name="dark">The dark piece in the capture event</param>
    /// <param name="turn">Which team's turn is it</param>
    public CaptureData(Move captureMove, EPieceType light, EPieceType dark, int turn) {
        CaptureMove = captureMove;
        LightPiece = light;
        DarkPiece = dark;
        Turn = turn;
    }
    #endregion
}
