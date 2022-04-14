using UnityEngine;

namespace ObjectManagementExample
{
    public abstract class SpawnZone : PersistableObject
    {
    public abstract Vector3 GetSpawnPoint();
    }
}
