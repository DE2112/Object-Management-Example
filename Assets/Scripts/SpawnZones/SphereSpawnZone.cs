using UnityEngine;

namespace ObjectManagementExample
{
    public class SphereSpawnZone : SpawnZone
    {
        [SerializeField] private bool _surfaceOnly;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
        
        public override Vector3 GetSpawnPoint()
        {
            return transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
        }
    }
}