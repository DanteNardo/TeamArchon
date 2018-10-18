using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace actionPhase
{
    
    public class PlayerStats : MonoBehaviour
    {
      
        private float health;
        // Use this for initialization
        private float team;
        void Start()
        {
            health = 100.00f;
        }

        // Update is called once per frame
        void Update()
        {
            
             if (health <= 0)
             {
                 gameObject.SetActive(false);
                 ShooterManager.instance.countDeath(gameObject);
             }
            
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        public float Team
        {
            get { return team; }
            set { if (value == 0 || value == 1) team = value; }
        }
    }
}