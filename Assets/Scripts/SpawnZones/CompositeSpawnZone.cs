using UnityEngine;

namespace ObjectManagementExample
{
    public class CompositeSpawnZone : SpawnZone
    {
        [SerializeField] private bool _sequential;
        [SerializeField] private SpawnZone[] _spawnZones;
        private int _nextSequentialIndex;

        public override Vector3 GetSpawnPoint()
        {
            int index;
            if (_sequential)
            {
                index = _nextSequentialIndex++;
                if (_nextSequentialIndex >= _spawnZones.Length)
                {
                    _nextSequentialIndex = 0;
                }
            }
            else
            {
                index = Random.Range(0, _spawnZones.Length);
            }
            return _spawnZones[index].GetSpawnPoint();
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_nextSequentialIndex);
        }

        public override void Load(GameDataReader reader)
        {
            _nextSequentialIndex = reader.ReadInt();
        }
    }
}