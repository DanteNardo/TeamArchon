/// <summary>
/// Stores all relevant movement data for pieces in the game
/// </summary>
public class Move {

    #region Move Members
    public int FX { get; private set; } // From X
    public int FZ { get; private set; } // From Z
    public int TX { get; private set; } // To X
    public int TZ { get; private set; } // To Z
    #endregion

    #region Move Properties
    public bool Invalid {
        get {
            return FX == -1 && FZ == -1 && TX == -1 && TZ == -1;
        }
    }
    #endregion

    #region Move Methods
    public Move(int fx, int fz, int tx, int tz) {
        FX = fx;
        FZ = fz;
        TX = tx;
        TZ = tz;
    }
    #endregion
}
