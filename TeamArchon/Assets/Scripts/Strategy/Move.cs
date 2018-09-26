/// <summary>
/// Stores all relevant movement data for pieces in the game
/// </summary>
public class Move {
    #region Move Properties
    public int From { get; private set; }
    public int To { get; private set; }
    public bool Capture { get; private set; }
    public bool Invalid { get { return From == -1 && To == -1; } }
    #endregion

    #region Move Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="from">The index to move from</param>
    /// <param name="to">The index to move to</param>
    /// <param name="capture">Whether or not this is a capture move</param>
    public Move(int from, int to, bool capture = false) {
        From = from;
        To = to;
        Capture = capture;
    }

    /// <summary>
    /// Used to reset the capture flag after a piece has been removed.
    /// </summary>
    public void ResetCaptureFlag() {
        Capture = false;
    }
    #endregion
}
