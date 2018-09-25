using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace actionPhase
{
    public class PlayerStats : NetworkBehaviour
    {
        public GameObject deathCamera;
        private float health;
        // Use this for initialization
        void Start()
        {
            health = 100.00f;
        }

        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                if (health <= 0)
                {

                    Destroy(gameObject);
                    Instantiate(deathCamera);
                }
            }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}