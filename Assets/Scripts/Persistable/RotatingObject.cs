using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class RotatingObject : PersistableObject
    {
        [SerializeField] private Vector3 _angularVelocity;

        private void Update()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime);
        }
    }
}