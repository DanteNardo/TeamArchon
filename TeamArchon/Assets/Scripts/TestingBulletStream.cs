using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class TestingBulletStream : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            var bullet = Instantiate(
                  bulletPrefab,
                  bulletSpawn.position,
                  bulletSpawn.rotation);

            // Add velocity to the bullet

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 6;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }
}