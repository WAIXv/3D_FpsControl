using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace namespace_EnemyController
{
    public class EnemyController : MonoBehaviour
    {
        public float speed = 0.5f;
        public float leftLimit;
        public float rightLimit;
        public float reActiveTime = 3f;
        private Vector3 dirMove;

        void Start()
        {
            dirMove = Vector3.forward * speed;
        }


        void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            this.transform.Translate(dirMove, Space.Self);
            if (transform.position.z >= leftLimit)
            {
                dirMove = new Vector3(dirMove.x, dirMove.y, -dirMove.z);
                return;
            }
            if (transform.position.z <= rightLimit)
            {
                dirMove = new Vector3(dirMove.x, dirMove.y, -dirMove.z);
                return;
            }
        }

    }
}

