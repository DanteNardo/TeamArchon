﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#region Master Game Enumerators
public enum ETeam {
    Light,
    Dark,
    None
}

public enum EGameState {
    Strategy,
    Action
}
#endregion

/// <summary>
/// Stores player information across scenes.
/// </summary>
public struct BasicPlayer
{
    public int team;
    public int teamPos;
    public GameObject[] pawns;
    public BasicPlayer(int setTeam, int setPos)
    {
        team = setTeam;
        teamPos = setPos;
        pawns = new GameObject[4];
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

    public Player[] playOrder;
    public GamepadCursor[] gamepads;
    public int playIndex;

    public List<GameTile> objectiveTiles;
    #endregion

    #region Properties
    public CaptureEvent CaptureAttempted { get; private set; }
    public CaptureData Capture { get; private set; }

    public RoundEvent RoundEnded { get; private set; }
    public RoundData Round { get; private set; }

    public Player CurrentPlayer { get { return playOrder[playIndex]; } }
    public EGameState GameState { get; private set; } = EGameState.Strategy;
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

        // Set up the list of objective tiles
        objectiveTiles = new List<GameTile>();
    }

    /// <summary>
    /// Called when the game is started for the first time. Initilizes players in the world
    /// </summary>
    public void StartGame(int[] teamPos, GameObject[] loadouts) {
       //Load the scene
       setTeamAndPos(teamPos,loadouts);
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
            playerComp.SetPlayer((ETeam)baseList[i].team, baseList[i].teamPos, i, baseList[i].pawns);

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
    void setTeamAndPos(int[] posiitions, GameObject[] pawns)
    {
        int lightTeamPos = 0;
        int darkTeamPos = 0;
        for (int i = 0; i < 8; i++) {
            if (posiitions[i] == 0) {
                baseList.Add(new BasicPlayer(0, lightTeamPos));
                lightTeamPos++;
            }
            else {
                baseList.Add(new BasicPlayer(1, darkTeamPos));
                darkTeamPos++;
            }
            for(int j = 0; j< 4; j++)
            {
                baseList[i].pawns[j] = pawns[i * 4 + j];
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
        switch (StrategyGame.Instance.TurnState) {
            case ETeam.Light:
                Capture = new CaptureData(move, light, dark, GetPieceHealth(move.From), GetPieceHealth(move.To), (int)StrategyGame.Instance.TurnState);
                break;
            case ETeam.Dark:
                Capture = new CaptureData(move, light, dark, GetPieceHealth(move.To), GetPieceHealth(move.From), (int)StrategyGame.Instance.TurnState);
                break;
        }

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
        Round = new RoundData(rr.WinningTeam, rr.Health);

        // Begin switch to Strategy portion
        SwitchToStrategy();
    }

    /// <summary>
    /// Adds the SceneLoaded method to the sceneLoaded event delegate.
    /// Also loads the specific action scene based on the given GameTile.
    /// </summary>
    private void SwitchToAction() {
        // TODO: Prepare for switching

        // Switch to action scene
        Debug.Log("Capture Tile: " + Capture.Tile);
        Debug.Log("Capture Tile Scene: " + Capture.Tile.sceneName);
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene(Capture.Tile.sceneName, LoadSceneMode.Additive);

        // Change game state
        GameState = EGameState.Action;
    }

    /// <summary>
    /// This is called when the scene is loaded. 
    /// It sets the active scene since we are loading the scene in additive mode. 
    /// </summary>
    /// <param name="scene">The loaded scene</param>
    /// <param name="loadMoad">The loading type for the scene</param>
    private void SceneLoaded(Scene scene, LoadSceneMode loadMoad) {
        SceneManager.SetActiveScene(scene);
    }

    /// <summary>
    /// Handles deleting the correct piece after a capture event ended.
    /// In addition, reperforms the move if the moving piece was unable
    /// to move due to a capture event occuring.
    /// </summary>
    private void SwitchToStrategy() {
        // Used to determine deletion later
        ETeam oldTurnState = StrategyGame.Instance.TurnState;
        Piece removePiece = null;
        Piece winPiece = null;
        int removeIndex = -1;
        int winIndex = -1;

        // Light won, light turn, delete To and move
        if ((ETeam)Round.WinningTeam == ETeam.Light &&
            StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            removeIndex = Capture.CaptureMove.To;
            winIndex = Capture.CaptureMove.From;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }
        // Light won, dark turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Light) {
            // Remove capture flag and remove piece
            removeIndex = Capture.CaptureMove.From;
            winIndex = Capture.CaptureMove.To;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, light turn, delete From, do not move
        else if ((ETeam)Round.WinningTeam == ETeam.Dark &&
                 StrategyGame.Instance.TurnState == ETeam.Light) {
            // Remove capture flag and remove piece
            removeIndex = Capture.CaptureMove.From;
            winIndex = Capture.CaptureMove.To;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.From);
        }
        // Dark won, dark turn, delete To and move
        else if ((ETeam)Round.WinningTeam == ETeam.Dark) {
            // Remove capture flag and remove piece
            removeIndex = Capture.CaptureMove.To;
            winIndex = Capture.CaptureMove.From;
            Capture.CaptureMove.ResetCaptureFlag();
            GameBoard.Instance.RemovePiece(Capture.CaptureMove.To);
            GameBoard.Instance.MovePiece(Capture.CaptureMove);
        }

        // Find the piece to remove and the piece to move
        foreach (var piece in StrategyGame.Instance.Pieces) {
            if (piece.Index == removeIndex) {
                removePiece = piece;
            }
            else if (piece.Index == winIndex) {
                winPiece = piece;
            }
        }

        // Remove the piece from the scene and list
        StrategyGame.Instance.Pieces.Remove(removePiece);
        Destroy(removePiece.gameObject);

        // Update the health of the surviving piece
        winPiece.Health = Round.Health;

        // Move the piece visually if necessary
        if ((ETeam)Round.WinningTeam == ETeam.Light && oldTurnState == ETeam.Light) {
            winPiece.Move(Capture.CaptureMove);
        }
        else if ((ETeam)Round.WinningTeam == ETeam.Dark && oldTurnState == ETeam.Dark) {
            winPiece.Move(Capture.CaptureMove);
        }

        // Unload the action scene
        SceneManager.UnloadSceneAsync(Capture.Tile.sceneName);

        // Change game state
        GameState = EGameState.Strategy;
    }

    /// <summary>
    /// Iterates to the next player's turn.
    /// </summary>
    public void NextPlayersTurn() {
        playIndex = playIndex + 1 < playOrder.Length ? playIndex + 1 : 0;
    }

    /// <summary>
    /// Gets a piece health from its index.
    /// </summary>
    /// <param name="index">The index the piece is at</param>
    /// <returns>The health of the piece at the index, or -1</returns>
    private float GetPieceHealth(int index) {
        // Iterate through all pieces
        foreach (Piece piece in StrategyGame.Instance.Pieces) {
            if (piece.Index == index) {
                return piece.Health;
            }
        }

        // Default return : this shouldn't occur
        return -1;
    }

    /// <summary>
    /// Determines whether or not the game is over and who won.
    /// </summary>
    /// <returns>Whether or not the game is over</returns>
    public bool GameOver() {
        // Game is over if one team owns a majority of the objectives
        int lightCount = 0, darkCount = 0;
        foreach (var tile in objectiveTiles) {
            if (tile.Occupation == ETeam.Light) {
                lightCount++;
            }
            else if (tile.Occupation == ETeam.Dark) {
                darkCount++;
            }
        }

        // LIGHT WINS!
        if (lightCount > (objectiveTiles.Count/2) + 1) {
            Debug.Log("===============================================");
            Debug.Log("================== LIGHT WINS! ================");
            Debug.Log("===============================================");
            return true;
        }
        // DARK WINS!
        if (darkCount > (objectiveTiles.Count / 2) + 1) {
            Debug.Log("===============================================");
            Debug.Log("=================== DARK WINS! ================");
            Debug.Log("===============================================");
            return true;
        }

        // Default return
        return false;
    }
    #endregion
}
