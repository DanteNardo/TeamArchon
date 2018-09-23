using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionWeapon : MonoBehaviour {
    public GameObject bulletPrefab;
    public float fireRate;
    private List<GameObject> bulletPool;
    private float fireTimer;
	// Use this for initialization
	void Start () {
        fireTimer = 0.0f;
        bulletPool = new List<GameObject>();	
	}
	
	// Update is called once per frame
	void Update () {
        
        fireTimer += Time.deltaTime;
	}

    public void Fire()
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
                    bullet.transform.position = gameObject.transform.position + Vector3.Normalize(gameObject.transform.up) * gameObject.GetComponent<SpriteRenderer>().size.x * 0.75f;
                    bullet.transform.rotation = gameObject.transform.rotation;
                    bullet.transform.Rotate(new Vector3(0, 0, 90));
                    bullet.SetActive(true);
                    createNew = false;
                    //exit for loop so we don't reactivate all inactive bullets
                    break;
                }

            }
            //if all bullets are active make a new one
            if (createNew)
            {
                GameObject tempBullet = Instantiate(bulletPrefab, gameObject.transform.position + Vector3.Normalize(gameObject.transform.up) * gameObject.GetComponent<SpriteRenderer>().size.x * 0.75f, gameObject.transform.rotation);
                tempBullet.transform.Rotate(new Vector3(0, 0, 90));

                if (gameObject.tag == "RedPlayer")
                {
                    tempBullet.tag = "RedBullet";

                }
                else
                {
                    tempBullet.tag = "BlueBullet";
                }
                bulletPool.Add(tempBullet);
            }
            fireTimer = 0.0f;
        }
       //tempBullet.SetActive(false, 5.0f);
    }
}
