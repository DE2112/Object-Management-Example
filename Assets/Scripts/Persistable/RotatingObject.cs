using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class RotatingObject : PersistableObject
    {
        [SerializeField] private Vector3 _angularVelocity;

        private void FixedUpdate()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime);
        }
    }
}