using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigid2D;
    //public Player player;
    void Start()
    {
        rigid2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        //Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 dir = Vector3.Normalize(new Vector3(hInput.GetAxis("Joy1RightXAxis"),hInput.GetAxis("Joy1RightYAxis"),0))+pos;
        float xTurn = hInput.GetAxis("Joy1RightXAxis");
        float yTurn = hInput.GetAxis("Joy1RightYAxis");


        Debug.Log(xTurn + " , " + yTurn);



        Vector3 lookDirection = new Vector3(xTurn, yTurn, 0);

        gameObject.transform.up = lookDirection;

        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        /*switch (player.JoystickValue)
        {
            case 0:
                xInput = hInput.GetAxis("Joy1LeftXAxis");
                yInput = hInput.GetAxis("Joy1LeftYAxis");
                break;
            case 1:
                xInput = hInput.GetAxis("Joy2LeftXAxis");
                yInput = hInput.GetAxis("Joy2LeftYAxis");
                break;
            case 2:
                xInput = hInput.GetAxis("Joy3LeftXAxis");
                yInput = hInput.GetAxis("Joy3LeftYAxis");
                break;
            case 3:
                xInput = hInput.GetAxis("Joy4LeftXAxis");
                yInput = hInput.GetAxis("Joy4LeftYAxis");
                break;
            case 4:
                xInput = hInput.GetAxis("Joy5LeftXAxis");
                yInput = hInput.GetAxis("Joy5LeftYAxis");
                break;
            case 5:
                xInput = hInput.GetAxis("Joy6LeftXAxis");
                yInput = hInput.GetAxis("Joy6LeftYAxis");
                break;
            case 6:
                xInput = hInput.GetAxis("Joy7LeftXAxis");
                yInput = hInput.GetAxis("Joy7LeftYAxis");
                break;
            case 7:
                xInput = hInput.GetAxis("Joy8LeftXAxis");
                yInput = hInput.GetAxis("Joy8LeftYAxis");
                break;
        }*/

        float xMove = hInput.GetAxis("Joy1LeftXAxis");
        float yMove = hInput.GetAxis("Joy1LeftYAxis");


        rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(xMove, yMove, 0)) * speed*Time.fixedDeltaTime);
      
        if (hInput.GetButton("Joy1A"))
        {
            
            gameObject.GetComponent<TestWeapon>().CmdFire();
            
        }


    }

}
