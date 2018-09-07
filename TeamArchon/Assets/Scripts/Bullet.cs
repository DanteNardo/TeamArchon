using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class Bullet : MonoBehaviour
    {
        private float timer;
        public float speed;
        //velocity of the bullet
        private Vector3 velocity;
        //position of the bullet
        private Vector3 postion;
        // Use this for initialization
        void Start()
        {
            postion = gameObject.transform.position;
            velocity = speed * gameObject.transform.up;
            timer = 0.0000f;

        }

        // Update is called once per frame
        void Update()
        {
            postion = postion + velocity * Time.deltaTime;

            gameObject.transform.position = postion;

            timer += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (timer >= 0.1f)
            {
                if (collision.other.tag == "Player")
                {
                    collision.other.GetComponent<PlayerStats>().Health -= 10.0f;
                    Debug.Log("HIT!");
                }
            }
        }
    }
}
