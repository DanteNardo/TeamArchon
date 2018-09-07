using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace actionPhase
{
    public class Bullet : MonoBehaviour
    {
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

        }

        // Update is called once per frame
        void Update()
        {
            postion = postion + velocity * Time.deltaTime;

            gameObject.transform.position = postion;
        }
    }
}
