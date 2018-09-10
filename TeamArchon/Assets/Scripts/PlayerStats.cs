using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class PlayerStats : MonoBehaviour
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
            
            if (health <= 0)
            {
                Debug.Log("You dead boi");
                Destroy(gameObject);
                Instantiate(deathCamera);
            }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}