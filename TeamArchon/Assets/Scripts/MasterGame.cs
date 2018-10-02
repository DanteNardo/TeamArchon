using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
#region Master Game Enumerators
public enum ETeam {
    Light,
    Dark,
    None
}
#endregion

/// <summary>
/// Master Game Manager that exists between Strategy and Action portion.
/// This handles switching between the two phases of the game.
/// </summary>
public class MasterGame : Singleton<MasterGame> {
    #region Properties
    
    public CaptureEvent CaptureAttempted { get; private set; }
    public CaptureData Capture { get; private set; }

    public RoundEvent RoundEnded { get; private set; }
    public RoundData Round { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the capture and round events and listeners.
    /// </summary>
    private void Start() {
        
    


        // Create capture attempt listeners
        CaptureAttempted = new CaptureEvent();

        // Start listening to capture attempts
        CaptureAttempted.AddListener(OnCaptureAttempted);

        // Create round event listeners
        RoundEnded = new RoundEvent();

        // Start listening to round ends
        RoundEnded.AddListener(OnRoundEnded);
    }

    //temp func to test scene change
    public void captureEvent()
    {
        SwitchToAction();
    }

    /// <summary>
    /// Invoked whenever a capture is attempted during the Strategy phase.
    /// Creates the Capture data and switches to the Action portion of the game.
    /// </summary>
    /// <param name="move">The move that generated this capture attempt</param>
    private void OnCaptureAttempted(Move move) {

        
        EPieceType light = EPieceType.None;
        EPieceType dark = EPieceType.None;

        // Determine pieces with heuristic where From is always the current team's moving piece
        switch (StrategyGame.Instance.TurnState) {
            case ETeam.Light:
                light = GameBoard.Instance[move.From];
                dark = GameBoard.Instance[move.To];
                break;
            case ETeam.Dark:
                dark = GameBoard.Instance[move.From];
                light = GameBoard.Instance[move.To];
                break;
        }

        // Create final capture data for action portion
        Capture = new CaptureData(move, light, dark, (int)StrategyGame.Instance.TurnState);

        // Begin switch to Action portion
        SwitchToAction();
    }

    /// <summary>
    /// Invoked when a round ends during the Action phase.
    /// Creates the Round data and switches to the Strategy portion of the game.
    /// </summary>
    /// <param name="rr">The round results from the Action phase</param>
    private void OnRoundEnded(RoundResults rr) {
        // TODO: Finish implementation when round results are expanded
        Round = new RoundData(rr.WinningTeam);

        // Begin switch to Strategy portion

        
        SwitchToStrategy();
    }

    private void SwitchToAction() {
        // TODO: Switch to action scene
        NetworkManager.singleton.ServerChangeScene("2dShooter");
    }


    private void SwitchToStrategy() {
        // TODO: Switch to strategy scene
        NetworkManager.singleton.ServerChangeScene("Strategy");
        // TODO: Remove the piece from StrategyGame list

        // Light won, light turn, delete To and move
        if ((ETeam)Round.WinningTeam == ETeam.Light &&
            StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }
        // Light won, dark turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Light) {
            // Remove capture flag and remove piece
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, light turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Dark &&
                 StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, dark turn, delete To and move
        else if ((ETeam)Round.WinningTeam == ETeam.Light) {
            // Remove capture flag and remove piece
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }
    }
    #endregion
}
