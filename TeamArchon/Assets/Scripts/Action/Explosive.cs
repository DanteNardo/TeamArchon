using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase {
    public class Explosive : MonoBehaviour {

        public bool explodeOnCollision;
        public float speed;
        public float lifespan;
        public float damage;
        public float radius;
        public GameObject explosion;
        private float activeTimer = 0.00f;
        GameObject[] players;
        Rigidbody2D rigid2D;
        // Use this for initialization
        void Start() {
            players = GameObject.FindGameObjectsWithTag("Player");
            rigid2D = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update() {
            Vector3 position = gameObject.transform.position;

            Vector3 velocity = Vector3.Normalize(gameObject.transform.right) * speed * Time.fixedDeltaTime;

            rigid2D.MovePosition(position + velocity);

            
            activeTimer += Time.fixedDeltaTime;
            if (activeTimer > lifespan)
            {
               
                explode();
                
            }
        }

        void explode()
        {
            GameObject explosionSpawned = Instantiate(explosion, gameObject.transform.position, new Quaternion());
            Bounds bounds = explosionSpawned.GetComponent<SpriteRenderer>().sprite.bounds;
            float xSize = bounds.size.x;
            float ySize = bounds.size.y;
            explosionSpawned.transform.localScale = new Vector3(2 / xSize, 2/ySize,0);



            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

                if (distance >= radius&& distance-radius!= 0)
                {
                    player.GetComponent<PlayerStats>().Health -= (damage * Mathf.Max((radius - distance) / radius, 0.1f));
                }else if(distance-radius == 0)
                {
                    player.GetComponent<PlayerStats>().Health -= damage;
                }

            }
            activeTimer = 0.00f;
            gameObject.SetActive(false);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (explodeOnCollision)
            {
                explode();
            }
        }
    }
}
