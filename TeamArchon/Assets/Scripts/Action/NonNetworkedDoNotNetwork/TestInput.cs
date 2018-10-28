using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace actionPhase {
    public class TestInput : MonoBehaviour
    {
        public float speed;
        private Rigidbody2D rigid2D;
        public Player player;

        private Vector3 previousUp;
        void Start()
        {
            rigid2D = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            gameObject.transform.up = previousUp;

            //Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            //Vector3 dir = Vector3.Normalize(new Vector3(hInput.GetAxis("Joy1RightXAxis"),hInput.GetAxis("Joy1RightYAxis"),0))+pos;
            float xTurn;
            float yTurn;
            float xMove;
            float yMove;







            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            switch (player.JoystickValue)
            {
                case 0:
                    xTurn = hInput.GetAxis("Joy1RightXAxis");
                    yTurn = hInput.GetAxis("Joy1RightYAxis");

                    xMove = hInput.GetAxis("Joy1LeftXAxis");
                    yMove = hInput.GetAxis("Joy1LeftYAxis");
                    if (hInput.GetButton("Joy1A")|| hInput.GetButton("Joy1RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 1:
                    xTurn = hInput.GetAxis("Joy2RightXAxis");
                    yTurn = hInput.GetAxis("Joy2RightYAxis");

                    xMove = hInput.GetAxis("Joy2LeftXAxis");
                    yMove = hInput.GetAxis("Joy2LeftYAxis");
                    if (hInput.GetButton("Joy2A") || hInput.GetButton("Joy2RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 2:
                    xTurn = hInput.GetAxis("Joy3RightXAxis");
                    yTurn = hInput.GetAxis("Joy3RightYAxis");

                    xMove = hInput.GetAxis("Joy3LeftXAxis");
                    yMove = hInput.GetAxis("Joy3LeftYAxis");
                    if (hInput.GetButton("Joy3A") || hInput.GetButton("Joy3RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 3:
                    xTurn = hInput.GetAxis("Joy4RightXAxis");
                    yTurn = hInput.GetAxis("Joy4RightYAxis");

                    xMove = hInput.GetAxis("Joy4LeftXAxis");
                    yMove = hInput.GetAxis("Joy4LeftYAxis");
                    if (hInput.GetButton("Joy4A")|| hInput.GetButton("Joy4RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 4:
                    xTurn = hInput.GetAxis("Joy5RightXAxis");
                    yTurn = hInput.GetAxis("Joy5RightYAxis");

                    xMove = hInput.GetAxis("Joy5LeftXAxis");
                    yMove = hInput.GetAxis("Joy5LeftYAxis");
                    if (hInput.GetButton("Joy5A")||hInput.GetButton("Joy5RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 5:
                    xTurn = hInput.GetAxis("Joy6RightXAxis");
                    yTurn = hInput.GetAxis("Joy6RightYAxis");

                    xMove = hInput.GetAxis("Joy6LeftXAxis");
                    yMove = hInput.GetAxis("Joy6LeftYAxis");
                    if (hInput.GetButton("Joy6A") || hInput.GetButton("Joy6RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 6:
                    xTurn = hInput.GetAxis("Joy7RightXAxis");
                    yTurn = hInput.GetAxis("Joy7RightYAxis");

                    xMove = hInput.GetAxis("Joy7LeftXAxis");
                    yMove = hInput.GetAxis("Joy7LeftYAxis");
                    if (hInput.GetButton("Joy7A") || hInput.GetButton("Joy7RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                case 7:
                    xTurn = hInput.GetAxis("Joy8RightXAxis");
                    yTurn = hInput.GetAxis("Joy8RightYAxis");

                    xMove = hInput.GetAxis("Joy8LeftXAxis");
                    yMove = hInput.GetAxis("Joy8LeftYAxis");
                    if (hInput.GetButton("Joy8A") || hInput.GetButton("Joy8RightBumper"))
                    {
                        gameObject.GetComponent<TestWeapon>().CmdFire();
                    }
                    break;
                default:
                    xTurn = 0;
                    yTurn = 0;
                    xMove = 0;
                    yMove = 0;
                    break;
            }

            Debug.Log(xTurn + " , " + yTurn);


            rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(xMove, yMove, 0)) * speed * Time.fixedDeltaTime);

            rigid2D.velocity = new Vector3(0.0f,0.0f,0.0f);

            rigid2D.angularVelocity = 0.0f;

            if (yTurn != 0.0f || xTurn != 0.0f)
            {
                Vector3 lookDirection = new Vector3(xTurn, yTurn, 0);

                gameObject.transform.up = lookDirection;

                previousUp = lookDirection;
            }


        }
    }
}
