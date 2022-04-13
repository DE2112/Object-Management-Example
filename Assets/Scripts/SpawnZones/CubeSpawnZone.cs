using UnityEngine;

namespace ObjectManagementExample
{
    public class CubeSpawnZone : SpawnZone
    {
        [SerializeField] private bool _surfaceOnly;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        
        public override Vector3 GetSpawnPoint()
        {
            Vector3 point;
            point.x = Random.Range(-0.5f, 0.5f);
            point.y = Random.Range(-0.5f, 0.5f);
            point.z = Random.Range(-0.5f, 0.5f);
            
            if (_surfaceOnly)
            {
                var axis = Random.Range(0, 3);
                point[axis] = point[axis] < 0f ? -0.5f : 0.5f;
            }
            
            return transform.TransformPoint(point);
        }
    }
}