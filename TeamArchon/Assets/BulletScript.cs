using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public float speed;
    public float lifespan;
    private float activeTimer = 0.00f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = gameObject.transform.position;

        Vector3 velocity = Vector3.Normalize(gameObject.transform.right) * speed*Time.deltaTime;

        position += velocity;
        gameObject.transform.position = position;

        activeTimer += Time.deltaTime;
        if(activeTimer> lifespan)
        {
            activeTimer = 0.00f;
            gameObject.SetActive(false);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "RedBullet" && collision.gameObject.tag == "BluePlayer")
        {

        }
        else if (gameObject.tag == "RedBullet" && collision.gameObject.tag == "BluePlayer")
        {

        }
        gameObject.SetActive(false);
    }
}
