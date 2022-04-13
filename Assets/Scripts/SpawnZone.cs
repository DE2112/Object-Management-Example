using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagementExample
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private bool _surfaceOnly;
        
        private void OnDrawGizmos ()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }

        public Vector3 GetSpawnPoint()
        {
            return transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
        }
    }
}
