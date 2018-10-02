using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace actionPhase
{
    public class ShooterMovement : NetworkBehaviour
    {

        [SyncVar]
        public NetworkInstanceId parentNetId;

        public float speed;
        private Rigidbody2D rigid2D;
        bool localPiece;
        public bool disableInput;
        public override void OnStartClient()
        {
            GameObject parent = ClientScene.FindLocalObject(parentNetId);
            
            transform.SetParent(parent.transform);

            localPiece = transform.parent.GetComponent<SquadManager>().CheckLocalPlayer();

            disableInput = true;
            
        }

        // Use this for initialization
        void Start()
        {
            rigid2D = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log(!localPiece || disableInput);
            
            if (!localPiece || disableInput)
            {
                return;
            }
           
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);



            rigid2D.MovePosition(gameObject.transform.position + Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)) * speed);

            if (Input.GetButton("Fire1"))
            {
               
                gameObject.GetComponent<TwoDimensionWeapon>().CmdFire();
            }


        }
    }
}
