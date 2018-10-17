using UnityEngine;

public class Player : MonoBehaviour
{
    #region Members
    private ETeam team;
    private int teamPosition;
    private SquadManager squadManager;
    string joysticVal;
    #endregion

    #region Methods
    /// <summary>
    /// Get componenets
    /// </summary>
    private void Awake()
    {
        squadManager = gameObject.GetComponent<SquadManager>();

    }

    /// <summary>
    /// Sets and saves player data.
    /// </summary>
    /// <param name="color">The player's team</param>
    /// <param name="position">The player's position on their team</param>
    /// <param name="joystic">The number of the joystic that the user is using</param>
    public void SetPlayer(ETeam color, int position, int joystic)
    {
        team = color;
        teamPosition = position;

        switch (joystic)
        {
            case 0:
                joysticVal = "Joy1";
                break;
            case 1:
                joysticVal = "Joy2";
                break;
            case 2:
                joysticVal = "Joy3";
                break;
            case 3:
                joysticVal = "Joy4";
                break;
            case 4:
                joysticVal = "Joy5";
                break;
            case 5:
                joysticVal = "Joy6";
                break;
            case 6:
                joysticVal = "Joy7";
                break;
            case 7:
                joysticVal = "Joy8";
                break;
            default:
                joysticVal = null;
                break;



        }

        StrategyGame.Instance.NewPlayer(squadManager, (int)team);
        squadManager.InstantiatePieces((int)team, teamPosition);
    }
    #endregion
}
