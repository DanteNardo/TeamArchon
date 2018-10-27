using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour {


    public GameObject masterGame;
    //2 arrays one to check if all players have locked in a value and the other to store the value
    public int[] selectedVal;
    public bool[] lockedIn;

    // Use this for initialization
    void Start() {
        selectedVal = new int[8];
        lockedIn = new bool[8];
    }

    // Update is called once per frame
    void Update() {
       
    }

    //lock in a specific vallue
    public void lockInValue(int pos, int value)
    {
        selectedVal[pos] = value;
        lockedIn[pos] = true;
        checkForAllLock();
    }

    //unlock a specific value
    public void unlockValue(int pos)
    {
        lockedIn[pos] = false;
    }

    /// <summary>
    /// checks if all players locked in, and if so sends the info to the masterGameManager
    /// </summary>
    private void checkForAllLock()
    {
        for(int i =0; i < 8; i++)
        {
            if (!lockedIn[i])
            {
                return;
            }
        }
        Debug.Log("All Locked In");
        masterGame.GetComponent<MasterGame>().StartGame(selectedVal);
        this.gameObject.SetActive(false);
    }
}
