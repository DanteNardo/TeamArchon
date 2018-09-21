/// <summary>
/// Stores all relevant movement data for pieces in the game
/// </summary>
public class Move {
    #region Move Properties
    public int From { get; private set; }
    public int To { get; private set; }
    public bool Invalid { get { return From == -1 && To == -1; } }
    #endregion

    #region Move Methods
    public Move(int from, int to) {
        From = from;
        To = to;
    }
    #endregion
}
