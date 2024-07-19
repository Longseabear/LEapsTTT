using System;
using UnityEngine;

namespace TTT.Physics
{
    [Serializable]
    public struct RigidbodyState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;

        public RigidbodyState(Rigidbody rb)
        {
            position = rb.position;
            rotation = rb.rotation;
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
        }
        public void Restore(Rigidbody rb)
        {
            rb.position = position;
            rb.rotation = rotation;
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;

            rb.MovePosition(position);
            rb.MoveRotation(rotation);
        }
    }
}
