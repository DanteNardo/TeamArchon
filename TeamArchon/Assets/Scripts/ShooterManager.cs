using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using actionPhase;
namespace actionPhase {
    public class ShooterManager : MonoBehaviour
    {

        public GameObject playerPrefab;
        public static ShooterManager instance;
        public int playerCount;
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
            for(int i = 0; i< MasterGame.Instance.playOrder.Length; i++){
                players.Add(Instantiate(playerPrefab));
                players[i].GetComponent<PlayerStats>().Team = (int)MasterGame.Instance.playOrder[i].team;
                players[i].GetComponent<TestInput>().player = MasterGame.Instance.playOrder[i];
                if(players[i].GetComponent<PlayerStats>().Team == 0)
                {
                    players[i].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                }
            }

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
       
        void ResetPlayer(GameObject player, TestWeapon.Weapon weaponType)
        {

            player.GetComponent<TestWeapon>().ChangeWeapon(weaponType);
            player.GetComponent<PlayerStats>().Health = 100;
            if(player.GetComponent<PlayerStats>().Team == 0)
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
            if (player.GetComponent<PlayerStats>().Team == 0) { 
                killCount0++;
            }
            else
            {
                killCount1++;
            }
            
        }

        private void ResetScene()
        {
            if(players.Count != playerCount)
            {
                for(int i = 0; i<playerCount; i++)
                {
                    players.Add(Instantiate(playerPrefab));
                }
            }

            spawnCount0 = 0;
            spawnCount1 = 0;
            killCount0 = 0;
            killCount1 = 0;
            killCountRequired = 0;

            foreach (GameObject player in players)
            {
                TestWeapon.Weapon weaponType;
                if (player.GetComponent<PlayerStats>().Team == 0)
                {
                    if (MasterGame.Instance.Capture.LightPiece == EPieceType.LMachineGun)
                    {
                        weaponType = TestWeapon.Weapon.MachineGun;
                    }
                    else if(MasterGame.Instance.Capture.LightPiece == EPieceType.LPistol)
                    {
                        weaponType = TestWeapon.Weapon.Pistol;
                    }else if(MasterGame.Instance.Capture.LightPiece == EPieceType.LShotgun)
                    {
                        weaponType = TestWeapon.Weapon.ShotGun;
                    }
                    else
                    {
                        weaponType = TestWeapon.Weapon.SniperRifle;
                    }
                }
                else
                {
                    if (MasterGame.Instance.Capture.DarkPiece == EPieceType.DMachineGun)
                    {
                        weaponType = TestWeapon.Weapon.MachineGun;
                    }
                    else if(MasterGame.Instance.Capture.DarkPiece == EPieceType.DPistol)
                    {
                        weaponType = TestWeapon.Weapon.Pistol;
                    }else if(MasterGame.Instance.Capture.DarkPiece == EPieceType.DShotgun)
                    {
                        weaponType = TestWeapon.Weapon.ShotGun;
                    }
                    else
                    {
                        weaponType = TestWeapon.Weapon.SniperRifle;
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
