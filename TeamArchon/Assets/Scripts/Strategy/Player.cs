using UnityEngine;

public class Player : MonoBehaviour
{
    #region Members
    public ETeam team { get; private set; }
    private int teamPosition;
    private SquadManager squadManager;
    private GameObject[] squadPrefabs;
    #endregion

    #region Properties
    public int JoystickValue { get; private set; }
    public GameObject[] SquadPrefabs { get { return squadPrefabs; } }
    #endregion

    #region Methods
    /// <summary>
    /// Get components
    /// </summary>
    private void Awake() {
        squadManager = gameObject.GetComponent<SquadManager>();
    }

    /// <summary>
    /// Sets and saves player data.
    /// </summary>
    /// <param name="color">The player's team</param>
    /// <param name="position">The player's position on their team</param>
    /// <param name="joystick">The number of the joystick that the user is using</param>
    public void SetPlayer(ETeam color, int position, int joystick, GameObject[] prefabs) {
        team = color;
        teamPosition = position;
        JoystickValue = joystick;
        squadPrefabs = prefabs;

        StrategyGame.Instance.NewPlayer(squadManager, SquadPrefabs, (int)team);
        squadManager.InstantiatePieces((int)team, teamPosition);
    }
    #endregion
}
