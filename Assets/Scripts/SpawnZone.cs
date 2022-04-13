using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagementExample
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private float _spawnRadius;

        public Vector3 GetSpawnPoint()
        {
            return transform.TransformPoint(Random.insideUnitSphere * _spawnRadius);
        }
    }
}
