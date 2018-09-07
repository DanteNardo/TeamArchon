using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class PlayerStats : MonoBehaviour
    {
        private float health;
        // Use this for initialization
        void Start()
        {
            health = 100.00f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}