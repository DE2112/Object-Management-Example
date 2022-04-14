using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class GameLevel : PersistableObject
    {
        private static GameLevel _current;
        [SerializeField] private SpawnZone _spawnZone;

        public static GameLevel Current => _current;
        public Vector3 SpawnPoint => _spawnZone.GetSpawnPoint();

        private void OnEnable()
        {
            _current = this;
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
        }
    }
}