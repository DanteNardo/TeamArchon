using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIPlayerControls : MonoBehaviour
{

    #region variables
    public int playerNum;
    string joyValue;
    public int startPos;
    public int rows;
    int cursorPos;
    int rowLength;
    public List<GameObject> UIElements;
    public GameObject playerCursor;
    bool choiceLockedIn;
    bool cursorMoved;
    public GameObject UIparrent;

    #endregion

    //an override that locks in a player, only to be used in the editor if 8 controllers aren't avalibe
    public bool lockinOverride;

    // Use this for initialization
    void Start()
    {


        if (rows <= 0)
        {
            rows = 1;
        }

        choiceLockedIn = false;
        //Get the child objects for ui input
        int UICount = gameObject.transform.childCount;
        UIElements = new List<GameObject>(UICount);


        //startpos sets the default postition of the cursor, if its out of bounds set it to 0
        if (startPos < 0 || startPos > UICount)
        {
            startPos = 0;
        }

        //get the number of items in each row
        rowLength = UICount / rows;



        //add each child to the list
        for (int i = 0; i < UICount; i++)
        {
            UIElements.Add(gameObject.transform.GetChild(i).gameObject);
        }


        //place the cursor in the right spot
        cursorPos = startPos;
        updateCursorPos();

        //set the string for each oystic
        switch (playerNum)
        {
            case 0:
                joyValue = "Joy1";
                break;
            case 1:
                joyValue = "Joy2";
                break;
            case 2:
                joyValue = "Joy3";
                break;
            case 3:
                joyValue = "Joy4";
                break;
            case 4:
                joyValue = "Joy5";
                break;
            case 5:
                joyValue = "Joy6";
                break;
            case 6:
                joyValue = "Joy7";
                break;
            case 7:
                joyValue = "Joy8";
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //check input only if the choice isnt locked in
        if (!choiceLockedIn)
        {


            //get input 
            float xInput = 0, yInput = 0;
            xInput = hInput.GetAxis(joyValue + "LeftXAxis");
            yInput = hInput.GetAxis(joyValue + "LeftYAxis");



            //check if the x pos of the controller is greater than .5 for dead zone
            if (xInput > .5f)
            {
                //currsor moved prevents the cursor from  immediately skipping objects
                if (cursorPos < UIElements.Count - 1 && !cursorMoved)
                {
                    cursorPos++;
                    updateCursorPos();
                    cursorMoved = true;
                }
            }

            else if (xInput < -.5f)
            {
                if (cursorPos > 0 && !cursorMoved)
                {
                    cursorPos--;

                    updateCursorPos();
                    cursorMoved = true;
                }
            }//for y input do the same tests except modify by the row value instead of 1
            else if (yInput > .5f)
            {
                if (cursorPos - rowLength >= 0 && !cursorMoved)
                {
                    cursorPos -= rowLength;
                    updateCursorPos();
                }
            }
            else if (yInput < -.5f)
            {
                if (cursorPos + rowLength < UIElements.Count && !cursorMoved)
                {
                    cursorPos += rowLength;
                    updateCursorPos();
                }
            }
            //if there is no input and the cursor was moved allow it to be moved again
            else if (cursorMoved)
            {
                cursorMoved = false;
            }//lock a choice in when a is pressed
            if (hInput.GetButtonDown(joyValue + "A"))
            {
                if(UIparrent.GetComponent<LobbyManager>().canLockIn(playerNum, cursorPos))
                {

                    choiceLockedIn = true;
                    playerCursor.GetComponent<SpriteRenderer>().color = Color.green;
                    UIparrent.GetComponent<LobbyManager>().lockInValue(playerNum, cursorPos);
                }

            }

        }
        else
        {//if b is pressed when youre locked in ulock the input
            if (hInput.GetButtonDown(joyValue + "B"))
            {
                choiceLockedIn = false;
                playerCursor.GetComponent<SpriteRenderer>().color = Color.white;
                UIparrent.GetComponent<LobbyManager>().unlockValue(playerNum);
            }


        }

        if (lockinOverride)
        {
            choiceLockedIn = true;
            playerCursor.GetComponent<SpriteRenderer>().color = Color.green;
            UIparrent.GetComponent<LobbyManager>().lockInValue(playerNum, cursorPos);
        }


    }

    /// <summary>
    /// Simple method to place the cursor at a position
    /// </summary>
    void updateCursorPos()
    {
        playerCursor.transform.position = UIElements[cursorPos].transform.position;


    }
}
