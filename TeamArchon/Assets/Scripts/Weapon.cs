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
        public enum weaponType { Pistol, BoltRifle, SubMachineGun };
        public weaponType weapon;

        //bullet prefab
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        private float fireRate;
        private float weaponDamage;
        private float timer;

        private void Start()
        {
           
            if(weapon == weaponType.Pistol)
            {
                fireRate = 2.0f;
                weaponDamage = 10.0f;
            }else if(weapon == weaponType.BoltRifle)
            {
                fireRate = 1.0f;
                weaponDamage = 20.0f;
            }
            else if(weapon == weaponType.SubMachineGun)
            {
                fireRate = 2.0f;
                weaponDamage = 5.0f;
            }
            timer = 1.0f / fireRate;
        }


        void Update()
        {
            timer += Time.deltaTime;

            /*if (Input.GetButtonDown("Fire1"))
            {
                //Fire();
            }*/
        }



        
        public GameObject Fire()
        {
            Debug.Log(timer);
            //get out of the method if the fire call is coming too soon.
            if (timer < 1.0f / fireRate) return null; 
            // Create the Bullet from the Bullet Prefab
            var bullet = Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            // Add velocity to the bullet

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

            timer = 0.0f;

            return bullet;
        }


        public float FireRate {
            get { return fireRate; }

        }

    }
}