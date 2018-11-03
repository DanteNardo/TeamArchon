using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour {

    public GameObject[] loadout1;
    public GameObject[] loadout2;
    public GameObject[] loadout3;
    public GameObject[] loadout4;
    public GameObject[] loadout5;
    public GameObject[] loadout6;
    public GameObject[] loadout7;
    public GameObject[] loadout8;

    public GameObject[] loadoutArray;
    // Use this for initialization
    void Start () {
        loadoutArray = new GameObject[32];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject[] getLoadouts(int[] teamPosiition, int[] loadoutPosition)
    {
        GameObject[] selectedLoadout = new GameObject[4];

        for(int i =0; i< 8; i++)
        {
            if (teamPosiition[i] == 0)
            {
                switch (loadoutPosition[i])
                {
                    case 0:
                        selectedLoadout = loadout1;
                        break;

                    case 1:
                        selectedLoadout = loadout2;
                        break;

                    case 2:
                        selectedLoadout = loadout3;
                        break;

                    case 3:
                        selectedLoadout = loadout4;
                        break;
                }
                    
                
            }
            else
            {
                switch (loadoutPosition[i])
                {
                    case 0:
                        selectedLoadout = loadout5;
                        break;

                    case 1:
                        selectedLoadout = loadout6;
                        break;

                    case 2:
                        selectedLoadout = loadout7;
                        break;

                    case 3:
                        selectedLoadout = loadout8;
                        break;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                int index = i * 4 + j;
                loadoutArray[index] = selectedLoadout[j];
            }
        }

        

        return loadoutArray;
    }
}
