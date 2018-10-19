using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using actionPhase;
namespace actionPhase
{
    public class BulletScript : MonoBehaviour {
        public float speed;
        public float lifespan;
        public float damage;
        private float activeTimer = 0.00f;
        private Rigidbody2D rigid2D;
        private int team;
        // Use this for initialization
        void Start() {
            rigid2D = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate() {
            Vector3 position = gameObject.transform.position;

            Vector3 velocity = Vector3.Normalize(gameObject.transform.right) * speed*Time.fixedDeltaTime;

            rigid2D.MovePosition(position + velocity);

            Debug.Log(activeTimer);
            activeTimer += Time.fixedDeltaTime;
            if (activeTimer > lifespan)
            {
                activeTimer = 0.00f;
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject);
            if ((gameObject.tag == "Team1" && collision.gameObject.tag == "Team2") || (gameObject.tag == "Team2" && collision.gameObject.tag == "Team1"))
            {
                PlayerStats hitStats = collision.gameObject.GetComponent<PlayerStats>();
                if (hitStats != null&&hitStats.Team!= team)
                {
                    hitStats.Health -= damage;
                }
        }
            if (collision.gameObject.tag != "Bullet")
            {
                gameObject.SetActive(false);
            }
        }
        public int Team
        {
            get { return team; }
            set { team = value; }
        }
    }
    
}
