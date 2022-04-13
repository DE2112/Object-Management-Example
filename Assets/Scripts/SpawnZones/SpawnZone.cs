using UnityEngine;

namespace ObjectManagementExample
{
    public abstract class SpawnZone : MonoBehaviour
    {
        public abstract Vector3 GetSpawnPoint();
    }
}
