using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using actionPhase;
namespace actionPhase {
    public class ShooterManager : MonoBehaviour
    {

        public GameObject pistol;
        public static ShooterManager instance;
        public GameObject machineGun;
        public List<GameObject> players;
        public List<GameObject> spawnPoints0;
        public List<GameObject> spawnPoints1;
        //using to send who wins after a certain kill count
        int killCount0;
        int killCount1;
        int killCountRequired;
        int spawnCount0;
        int spawnCount1;


        private void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {
            ResetScene();


        }





        // Update is called once per frame
        void Update()
        {
            if (killCount0 >= killCountRequired / 2)
            {
                roundEnd(0);
                ResetScene();
            }
            else if (killCount1 >= killCountRequired / 2)
            {
                roundEnd(1);
                ResetScene();
            }
        }
       
        void ResetPlayer(GameObject player, TwoDimensionWeapon.Weapon weaponType)
        {

            player.GetComponent<TwoDimensionWeapon>().ChangeWeapon(weaponType);
            player.GetComponent<PlayerStats>().Health = 100;
            if(player.GetComponentInParent<SquadManager>().team == 0)
            {
                player.transform.position = spawnPoints0[spawnCount0].transform.position;
                spawnCount0++;
            }
            else
            {
                player.transform.position = spawnPoints1[spawnCount1].transform.position;
                spawnCount1++;
            }
        }

        public void countDeath(GameObject player)
        {
            if (player.GetComponentInParent<SquadManager>().team == 0) { 
                killCount0++;
            }
            else
            {
                killCount1++;
            }
            
        }

        private void ResetScene()
        {
            spawnCount0 = 0;
            spawnCount1 = 0;
            killCount0 = 0;
            killCount1 = 0;
            killCountRequired = 0;

            foreach (GameObject player in players)
            {
                TwoDimensionWeapon.Weapon weaponType;
                if (player.GetComponent<PlayerStats>().Team == 0)
                {
                    if (MasterGame.Instance.Capture.LightPiece == EPieceType.LMachineGun)
                    {
                        weaponType = TwoDimensionWeapon.Weapon.MachineGun;
                    }
                    else
                    {
                        weaponType = TwoDimensionWeapon.Weapon.Pistol;
                    }
                }
                else
                {
                    if (MasterGame.Instance.Capture.LightPiece == EPieceType.DMachineGun)
                    {
                        weaponType = TwoDimensionWeapon.Weapon.MachineGun;
                    }
                    else
                    {
                        weaponType = TwoDimensionWeapon.Weapon.Pistol;
                    }
                }
                ResetPlayer(player, weaponType);

                killCountRequired++;
            }
        }

        void roundEnd(int winningTeam)
        {
            MasterGame.Instance.RoundEnded.Invoke(new RoundResults(winningTeam));
        }


    }
}
