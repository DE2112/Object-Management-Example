using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class GameLevel : PersistableObject
    {
        private static GameLevel _current;
        [SerializeField] private SpawnZone _spawnZone;
        [SerializeField] private PersistableObject[] _persistableObjects;

        public static GameLevel Current => _current;
        public Vector3 SpawnPoint => _spawnZone.GetSpawnPoint();

        private void OnEnable()
        {
            _current = this;
            if (_persistableObjects == null)
            {
                _persistableObjects = new PersistableObject[0];
            }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_persistableObjects.Length);
            foreach (var persistableObject in _persistableObjects)
            {
                persistableObject.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            var count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                _persistableObjects[i].Load(reader);
            }
        }
    }
}