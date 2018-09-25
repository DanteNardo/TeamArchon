using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionWeapon : MonoBehaviour {
    public GameObject Bullet;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire()
    {
        GameObject tempBullet = Instantiate(Bullet, gameObject.transform.position + Vector3.Normalize(gameObject.transform.up)*gameObject.GetComponent<SpriteRenderer>().size.x*0.75f, gameObject.transform.rotation);
    }
}
