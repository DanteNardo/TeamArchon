using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class TestingBulletStream : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public float firerate;
        // Use this for initialization
        private float timer;
        void Start()
        {
            timer = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1 / firerate) { 
            var bullet = Instantiate(
                  bulletPrefab,
                  bulletSpawn.position,
                  bulletSpawn.rotation);
                timer = 0.0f;
            // Add velocity to the bullet

           

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
            }

        }
    }
}