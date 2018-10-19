using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEngine.UI;

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
    #region Members
    public Camera strategyCamera;
    public Camera shooterCamera;

    public List<BasicPlayer> baseList;
    public List<GameObject> playerList;
    public GameObject playerPrefab;
    public GameObject playerTeamSelector;
    public Player[] playOrder;
    public GamepadCursor[] gamepads;
    public int playIndex;
    #endregion

    #region Properties
    public CaptureEvent CaptureAttempted { get; private set; }
    public CaptureData Capture { get; private set; }

    public RoundEvent RoundEnded { get; private set; }
    public RoundData Round { get; private set; }

    public Player CurrentPlayer { get { return playOrder[playIndex]; } }
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


        // Set the scenemanager to call on gamestart when a scene is loaded
        SceneManager.sceneLoaded += OnGameStart;
        baseList = new List<BasicPlayer>();
        playOrder = new Player[8];
    }

    /// <summary>
    /// Called when the game is started for the first time. Initilizes players in the world
    /// </summary>
    public void StartGame()
    {
       //Load the scene
       setTeamAndPos();
       SceneManager.LoadScene("Scenes/Strategy", LoadSceneMode.Single);
       
    }

    /// <summary>
    /// Initilizes all params set in the first scene. Sets each players team and postiion in the team
    /// </summary>
    /// <param name="scene">The scene that was loaded</param>
    /// <param name="loadMoad">Additive or Single</param>
    void OnGameStart(Scene scene, LoadSceneMode loadMoad)
    {
        // Get references to all of the gamepad cursors
        GameObject[] gamepadObjects = GameObject.FindGameObjectsWithTag("Gamepad");
        gamepads = new GamepadCursor[gamepadObjects.Length];
        for (int i = 0; i < gamepadObjects.Length; i++) {
            gamepads[i] = gamepadObjects[i].GetComponent<GamepadCursor>();
        }

        // Instantiate variables for player creation
        playIndex = 0;

        // Instantiate each player in the scene
        for (int i = 0; i < baseList.Count; i++) {

            int playPos = (1 + baseList[i].teamPos) * 2 + baseList[i].team - 2;
            int playerNum = baseList[i].teamPos + (4 * baseList[i].team);

            GameObject tempObj = Instantiate(playerPrefab);
            var playerComp = tempObj.GetComponent<Player>();
            tempObj.name = "Player" + playerNum.ToString();
            playerComp.SetPlayer((ETeam)baseList[i].team, baseList[i].teamPos, i);

            // Save the Player script
            playOrder[playPos] = playerComp;

            // Save the correct gamepad to the player
            foreach (var gamepad in gamepads) {
                if (gamepad.name == "Player1Cursor" && playerNum == 0) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player2Cursor" && playerNum == 1) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player3Cursor" && playerNum == 2) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player4Cursor" && playerNum == 3) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player5Cursor" && playerNum == 4) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player6Cursor" && playerNum == 5) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player7Cursor" && playerNum == 6) {
                    gamepad.player = playerComp;
                    continue;
                }
                else if (gamepad.name == "Player8Cursor" && playerNum == 7) {
                    gamepad.player = playerComp;
                    continue;
                }
            }
        }

        SceneManager.sceneLoaded -= OnGameStart;
    }
   
    /// <summary>
    /// Loops through all user positions on the lobby scene and adds them to the list of basic players
    /// </summary>
    void setTeamAndPos()
    {
        int lightTeamPos = 0;
        int darkTeamPos = 0;
        //Debug.Log(playerTeamSelector.transform.childCount);
        for(int i = 0; i < playerTeamSelector.transform.childCount; i++)
        {
            // If the dropdown = 0 add a new light player otherwise add a new dark player
            if(playerTeamSelector.transform.GetChild(i).GetComponent<Dropdown>().value == 0)
            {
                baseList.Add(new BasicPlayer(0, lightTeamPos));
                lightTeamPos++;
            }
            else
            {
                baseList.Add(new BasicPlayer(1, darkTeamPos));
                darkTeamPos++;
            }
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
        SceneManager.LoadScene("Scenes/NonNetworkedShooterTest", LoadSceneMode.Additive);
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

        // Unload the action scene
        SceneManager.UnloadSceneAsync("Scenes/NonNetworkedShooterTest");
    }

    /// <summary>
    /// Iterates to the next player's turn.
    /// </summary>
    public void NextPlayersTurn() {
        playIndex = playIndex + 1 < playOrder.Length ? playIndex + 1 : 0;
    }
    #endregion
}
