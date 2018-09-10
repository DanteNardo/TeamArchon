using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace actionPhase
{
    public class Weapon : MonoBehaviour
    {

        //enumerator for typ of weapons
        //unsued
        public enum weaponType { Pistol };

        //bullet prefab
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public float FireRate 


        void Update()
        {


            if (Input.GetButtonDown("Fire1"))
            {
                //Fire();
            }
        }



        
        public GameObject Fire()
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            // Add velocity to the bullet

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

            return bullet;
        }

    }
}