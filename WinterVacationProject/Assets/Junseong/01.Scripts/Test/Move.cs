using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJS
{
    public class Move : MonoBehaviour
    {
        public float speed = 5f;
        private Rigidbody rg;

        void Start()
        {
            rg = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            Vector3 velocity = new Vector3(inputX, 0, inputZ);
            velocity *= speed;
            rg.velocity = velocity;
        }
    }
}
