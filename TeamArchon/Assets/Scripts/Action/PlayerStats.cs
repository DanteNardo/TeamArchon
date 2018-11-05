using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace actionPhase
{
    public class PlayerStats : MonoBehaviour
    {
        private float health;
        private Transform healthBar;
        private Transform playerTransform;
        public int team;

        // Use this for initialization
        void Start()
        {
            health = 100.00f;
            healthBar = gameObject.transform.GetChild(0).GetComponent<Transform>();
            playerTransform = healthBar.parent;
        }

        // Update is called once per frame
        void Update()
        {
             if (health <= 0)
             {
                 
                 ShooterManager.instance.countDeath(gameObject);
                gameObject.SetActive(false);
            }

            healthBar.parent = null;
            healthBar.localScale = new Vector3(health / 100.0f,1 , 1);
            healthBar.parent = playerTransform;
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Team
        {
            get { return team; }
            set { if (value == 0 || value == 1) team = value; }
        }
    }
}