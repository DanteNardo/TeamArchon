using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShooterMovement : NetworkBehaviour{
    public float speed;
    private Rigidbody2D rigid2D;
	// Use this for initialization
	void Start () {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer)
        {
            return;
        }
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        

        rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)) * speed*Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Fired");
            gameObject.GetComponent<TwoDimensionWeapon>().CmdFire();
        }

        
	}
}
