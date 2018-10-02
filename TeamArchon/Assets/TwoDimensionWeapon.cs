using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TwoDimensionWeapon : NetworkBehaviour{
    public GameObject pistolBulletPrefab;
    public GameObject machineGunBulletPrefab;
    public enum Weapon { Pistol, MachineGun };
    public Weapon weaponType;
    private GameObject finalPrefab;
    private float fireRate;
    private List<GameObject> bulletPool;
    private float fireTimer;
	// Use this for initialization
	void Start () {
        fireTimer = 0.0f;
        bulletPool = new List<GameObject>();
        ChangeWeapon(weaponType);
	}
	
	// Update is called once per frame
	void Update () {
        
        fireTimer += Time.deltaTime;
	}

    [ClientRpc]
    public void RpcEnable(GameObject bullet, GameObject targetObj)
    {
        bullet.transform.position = targetObj.transform.position + Vector3.Normalize(targetObj.transform.up) * targetObj.GetComponent<SpriteRenderer>().size.x * 0.75f;
        bullet.transform.rotation = targetObj.transform.rotation;
        bullet.transform.Rotate(new Vector3(0, 0, 90));
        bullet.SetActive(true);
        
    }

    [Command]
    public void CmdFire()
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
                    RpcEnable(bullet, gameObject);
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
                tempBullet.tag = gameObject.tag;
                bulletPool.Add(tempBullet);
                NetworkServer.Spawn(tempBullet);
            }
            fireTimer = 0.0f;
        }
       //tempBullet.SetActive(false, 5.0f);
    }
    public void ChangeWeapon(Weapon weaponType)
    {
        this.weaponType = weaponType;
        if(this.weaponType == Weapon.Pistol)
        {
            fireRate = 2.0f;
            finalPrefab = pistolBulletPrefab;
        }else if(weaponType == Weapon.MachineGun)
        {
            fireRate = 6.0f;
            finalPrefab = machineGunBulletPrefab;
        }
    }
}
