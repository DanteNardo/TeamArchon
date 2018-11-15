using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using actionPhase;
using UnityEngine.UI;
namespace actionPhase
{
    public class ShooterManager : MonoBehaviour
    {


        public GameObject testPlayers;
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

        //stores the ammount of time the scene has been running
        float startTime;
        //modifilble value to store how long an action round should take
        public float roundTime;

        public Text timeText;
        //make sure were not updating text when there isnt any change
        int previousText;

      

        private void Awake()
        {
            instance = this;

            //default time = 30;
            if (roundTime == 0)
            {
                roundTime = 30;
            }
            previousText = (int)roundTime;
            timeText.text = "Time Left: " + previousText;
        }

        // Use this for initialization
        void Start()
        {

            ShooterStart();

        }

        public void ShooterStart()
        {

            if (MasterGame.Instance != null)
            {
                testPlayers.SetActive(false);

                //MasterGame.Instance.

                for (int i = 0; i < MasterGame.Instance.playOrder.Length; i++)
                {
                    players.Add(Instantiate(playerPrefab));
                    players[i].GetComponent<PlayerStats>().Team = (int)MasterGame.Instance.playOrder[i].team;
                    players[i].GetComponent<TestInput>().joyStickValue = MasterGame.Instance.playOrder[i].JoystickValue;
                    if (players[i].GetComponent<PlayerStats>().Team == 1)
                    {
                        players[i].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                        players[i].layer = 12;
                    }
                    else
                    {
                        players[i].layer = 11;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    players.Add(testPlayers.transform.GetChild(i).gameObject);

                    players[i].GetComponent<TestInput>().joyStickValue = i;

                    if (players[i].GetComponent<PlayerStats>().Team == 1)
                    {
                        players[i].GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                        players[i].layer = 12;
                    }
                    else
                    {
                        players[i].layer = 11;
                    }
                }


            }


            startTime = 0;
            ResetScene();
        }






        // Update is called once per frame
        void Update()
        {

           
                startTime += Time.deltaTime;
           
            
           

            
            if(Mathf.Ceil(roundTime - startTime) != previousText && previousText >= 0)
            {
                previousText = Mathf.CeilToInt(roundTime - startTime);
                timeText.text = "Time Left: " + previousText;
            }

            
            
            if (startTime >= roundTime)
            {
                float lightHealth = 0;
                float darkHealth = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    if(players[i].GetComponent<PlayerStats>().team == 0)
                    {
                        lightHealth += players[i].GetComponent<PlayerStats>().Health;
                    }
                    else
                    {
                        darkHealth += players[i].GetComponent<PlayerStats>().Health;
                    }
                    
                }

                if(lightHealth > darkHealth)
                {
                    Debug.Log("_____ Round is over Light wins by time");
                    roundEnd(0);
                    ResetScene();
                }
                else if(darkHealth > lightHealth)
                {
                    Debug.Log("_____ Round is over Dark wins by time ");
                    roundEnd(1);
                    ResetScene();
                }

                timeText.text = "Sudden Death! next hit wins";
            }


            if (killCount0 >= killCountRequired / 2)
            {
                Debug.Log("_____ Round is over Dark wins");
                roundEnd(1);
                ResetScene();
            }
            else if (killCount1 >= killCountRequired / 2)
            {

                Debug.Log("_____ Round is over Light wins");
                roundEnd(0);
                ResetScene();
            }
        }

        void ResetPlayer(GameObject player, TestWeapon.Weapon weaponType, float darkHealth, float lightHealth)
        {
            Debug.Log("dark health " + darkHealth);
            Debug.Log("light health " + lightHealth);

            player.GetComponent<TestWeapon>().ChangeWeapon(weaponType);

            if (player.GetComponent<PlayerStats>().Team == 0)
            {
                player.transform.position = spawnPoints0[spawnCount0].transform.position;
                player.GetComponent<PlayerStats>().Health = lightHealth / 4;
                spawnCount0++;
            }
            else
            {
                player.transform.position = spawnPoints1[spawnCount1].transform.position;
                player.GetComponent<PlayerStats>().Health = darkHealth / 4;
                spawnCount1++;
            }
        }

        public void countDeath(GameObject player)
        {
            if (player.GetComponent<PlayerStats>().Team == 0)
            {
                killCount0++;
            }
            else
            {
                killCount1++;
            }

        }

        private void ResetScene()
        {
            if (players.Count != playerCount)
            {
                for (int i = 0; i < playerCount; i++)
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
                Debug.Log("Piece Type:    " + MasterGame.Instance.Capture.DarkPiece + "    " + player.GetComponent<PlayerStats>().Team);
                TestWeapon.Weapon weaponType;
                if (player.GetComponent<PlayerStats>().Team == 0)
                {
                    if (MasterGame.Instance.Capture.LightPiece == EPieceType.LMachineGun)
                    {
                        weaponType = TestWeapon.Weapon.MachineGun;
                    }
                    else if (MasterGame.Instance.Capture.LightPiece == EPieceType.LPistol)
                    {
                        weaponType = TestWeapon.Weapon.Pistol;
                    }
                    else if (MasterGame.Instance.Capture.LightPiece == EPieceType.LShotgun)
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
                    else if (MasterGame.Instance.Capture.DarkPiece == EPieceType.DPistol)
                    {
                        weaponType = TestWeapon.Weapon.Pistol;
                    }
                    else if (MasterGame.Instance.Capture.DarkPiece == EPieceType.DShotgun)
                    {
                        weaponType = TestWeapon.Weapon.ShotGun;
                    }
                    else
                    {
                        weaponType = TestWeapon.Weapon.SniperRifle;
                    }
                }
                ResetPlayer(player, weaponType, MasterGame.Instance.Capture.DarkHealth, MasterGame.Instance.Capture.LightHealth);

                killCountRequired++;
            }

            
        }

        void roundEnd(int winningTeam)
        {
            float healthTotal = 0.0f;
            foreach (GameObject player in players)
            {
                healthTotal += player.GetComponent<PlayerStats>().Health;
            }
            Debug.Log("ROUND ENDED CALLED ______________ ");
            MasterGame.Instance.RoundEnded.Invoke(new RoundResults(winningTeam, healthTotal));
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Strategy"));
        }


    }
}
