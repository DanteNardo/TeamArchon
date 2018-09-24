using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMovement : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigid2D;
	// Use this for initialization
	void Start () {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        

        rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)) * speed*Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.GetComponent<TwoDimensionWeapon>().Fire();
        }

        
	}
}
