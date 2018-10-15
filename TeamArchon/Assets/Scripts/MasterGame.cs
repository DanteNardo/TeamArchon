using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#region Master Game Enumerators
public enum ETeam {
    Light,
    Dark,
    None
}
#endregion

/// <summary>
/// Stores player information across scenes.
/// </summary>
public struct BasicPlayer
{
    public int team;
    public int teamPos;

    public BasicPlayer(int setTeam, int setPos)
    {
        team = setTeam;
        teamPos = setPos;
    }

    public void print()
    {
        Debug.Log("Team " + (ETeam)team + " Pos " + teamPos);
    }
}

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

    public Camera strategyCamera;
    public Camera shooterCamera;

    public List<BasicPlayer> baseList;

    public List<GameObject> playerList;

    public GameObject playerPrefab;
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


        baseList = new List<basicPlayer>();
        for(int i = 0; i < 8; i++)
        {

            baseList.Add(new basicPlayer(i % 2, (int)i / 2));
            //baseList[i].print();
        }
        startGame();
    }

    /// <summary>
    /// Creates the players and sets their data.
    /// </summary>
    public void StartGame() {
        // Set the data for each player when the game starts
        for(int i = 0; i < baseList.Count; i++) {
            GameObject tempObj = Instantiate(playerPrefab);
            tempObj.GetComponent<Player>().SetPlayer((ETeam)baseList[i].team, baseList[i].teamPos);
        }
    }
    
    /// <summary>
    /// Invoked whenever a capture is attempted during the Strategy phase.
    /// Creates the Capture data and switches to the Action portion of the game.
    /// </summary>
    /// <param name="move">The move that generated this capture attempt</param>
    private void OnCaptureAttempted(Move move) {
        // Initialize variables
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
        // TODO: Prepare for switching

        // Switch to action scene
        SceneManager.LoadScene("ActionScene");
    }
    
    /// <summary>
    /// Handles deleting the correct piece after a capture event ended.
    /// In addition, reperforms the move if the moving piece was unable
    /// to move due to a capture event occuring.
    /// </summary>
    private void SwitchToStrategy() {
        // Used to determine deletion later
        int pieceIndex = 0;

        // Light won, light turn, delete To and move
        if ((ETeam)Round.WinningTeam == ETeam.Light &&
            StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            pieceIndex = Capture.CaptureMove.To;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }
        // Light won, dark turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Light) {
            // Remove capture flag and remove piece
            pieceIndex = Capture.CaptureMove.From;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, light turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Dark &&
                 StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            pieceIndex = Capture.CaptureMove.From;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, dark turn, delete To and move
        else if ((ETeam)Round.WinningTeam == ETeam.Light) {
            // Remove capture flag and remove piece
            pieceIndex = Capture.CaptureMove.To;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }

        // Remove the piece from StrategyGame list of pieces
        foreach (var piece in StrategyGame.Instance.Pieces) {
            if (Board.IndexFromRowAndCol(piece.Z, piece.X) == pieceIndex) {
                StrategyGame.Instance.Pieces.Remove(piece);
                continue;
            }
        }

        // Switch to strategy scene
        SceneManager.LoadScene("StrategyScene");
    }
    #endregion
}
