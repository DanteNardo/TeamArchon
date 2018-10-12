using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour {

    public GameObject pistolBulletPrefab;
    public GameObject machineGunBulletPrefab;
    public GameObject sniperBulletPrefab;
    public enum Weapon { Pistol, MachineGun, ShotGun, SniperRifle };
    public Weapon weaponType;
    private GameObject finalPrefab;
    private float fireRate;
    private List<GameObject> bulletPool;
    private float fireTimer;
    // Use this for initialization
    void Start()
    {
        fireTimer = 20.0f;
        bulletPool = new List<GameObject>();
        ChangeWeapon(weaponType);
    }

    // Update is called once per frame
    void Update()
    {

        fireTimer += Time.deltaTime;
    }


    public void bulletEnable(GameObject bullet, GameObject targetObj, int spread)
    {
        bullet.transform.position = targetObj.transform.position + Vector3.Normalize(targetObj.transform.up) * targetObj.GetComponent<SpriteRenderer>().size.x * 0.75f;
        bullet.transform.rotation = targetObj.transform.rotation;
        bullet.transform.Rotate(new Vector3(0, 0, 90 + spread));
        bullet.SetActive(true);

    }


    public void CmdFire()
    {
        if (weaponType != Weapon.ShotGun)
        {
            //firerate check
            if (fireTimer >= 1.0f / fireRate)
            {
                //bool for whether to create a new bullet or use a preexisting one
                bool createNew = true;

                foreach (GameObject bullet in bulletPool)
                {
                    //if there is an inactive bullet, grab it, and activate it
                    if (bullet.activeInHierarchy == false)
                    {
                        bulletEnable(bullet, gameObject, 0);
                        createNew = false;
                        //exit for loop so we don't reactivate all inactive bullets
                        break;
                    }

                }
                //if all bullets are active make a new one
                if (createNew)
                {
                    GameObject tempBullet = Instantiate(finalPrefab, gameObject.transform.position + Vector3.Normalize(gameObject.transform.up) * gameObject.GetComponent<SpriteRenderer>().size.x * 0.75f, gameObject.transform.rotation);
                    tempBullet.transform.Rotate(new Vector3(0, 0, 90));

                    //making the bullet be on it's players "team"
                 
                    bulletPool.Add(tempBullet);

                }
                fireTimer = 0.0f;
            }
        }
        else if (weaponType == Weapon.ShotGun)
        {
            
            if (fireTimer >= 1.0f / fireRate)
            {
                //bool for whether to create a new bullet or use a preexisting one
                int newBullets = 3;
                int angleSpread = 15;

                foreach (GameObject bullet in bulletPool)
                {
                    //if there is an inactive bullet, grab it, and activate it
                    if (bullet.activeInHierarchy == false)
                    {
                        bulletEnable(bullet, gameObject, ((-angleSpread * 2) + (angleSpread * newBullets)));
                        newBullets--;
                        //exit for loop so we don't reactivate all inactive bullets
                        if (newBullets <= 0)
                        {
                            break;
                        }
                    }

                }

                //if all bullets are active make a new one
                while (newBullets > 0)
                {
                    Debug.Log(newBullets);
                    GameObject tempBullet = Instantiate(finalPrefab, gameObject.transform.position + Vector3.Normalize(gameObject.transform.up) * gameObject.GetComponent<SpriteRenderer>().size.x * 0.75f, gameObject.transform.rotation);
                    tempBullet.transform.Rotate(new Vector3(0, 0, 90 + ((-angleSpread * 2) + (angleSpread * newBullets))));

                    //making the bullet be on it's players "team"
                    bulletPool.Add(tempBullet);
                    newBullets--;

                }
                fireTimer = 0.0f;
            }
        }

    
    }
 //tempBullet.SetActive(false, 5.0f);
    
    public void ChangeWeapon(Weapon weaponType)
    {
        this.weaponType = weaponType;
        if (this.weaponType == Weapon.Pistol)
        {
            fireRate = 2.0f;
            finalPrefab = pistolBulletPrefab;
        }
        else if (weaponType == Weapon.MachineGun)
        {
            fireRate = 6.0f;
            finalPrefab = machineGunBulletPrefab;
        }
        else if (weaponType == Weapon.ShotGun)
        {
            fireRate = 1.0f;
            finalPrefab = pistolBulletPrefab;
        }else if(weaponType == Weapon.SniperRifle)
        {
            fireRate = 0.5f;
            finalPrefab = sniperBulletPrefab;
        }
    }
}

