using UnityEngine;

namespace ObjectManagementExample
{
    public class CompositeSpawnZone : SpawnZone
    {
        [SerializeField] private SpawnZone[] _spawnZones;

        public override Vector3 GetSpawnPoint()
        {
            var index = Random.Range(0, _spawnZones.Length);
            return _spawnZones[index].GetSpawnPoint();
        }
    }
}