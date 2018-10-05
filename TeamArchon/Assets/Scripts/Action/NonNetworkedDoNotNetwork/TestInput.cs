using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigid2D;
    void Start()
    {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //Debug.Log("");


        rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)) * speed*Time.deltaTime);
        Debug.Log(Input.GetButton("Fire1"));
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("fired");
            gameObject.GetComponent<TestWeapon>().CmdFire();
            
        }


    }

}
