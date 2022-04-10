using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectManagementExample
{
    public class Game : PersistableObject
    {
        private const float MIN_SIZE = 0.1f;
        private const float MAX_SIZE = 1f;
        private const int SAVE_FILE_VERSION = 1;

        [SerializeField] private PersistentStorage _storage;
        [SerializeField] private ShapeFactory _shapeFactory;
        [SerializeField] private float _spawnSphereRadius;
        [SerializeField] private List<Shape> _shapes;

        [Header("Keys")] [SerializeField] private KeyCode _spawnKey;
        [SerializeField] private KeyCode _resetKey;
        [SerializeField] private KeyCode _saveKey;
        [SerializeField] private KeyCode _loadKey;

        private void Awake()
        {
            _shapes = new List<Shape>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _spawnSphereRadius);
            Gizmos.color = Color.red;
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(_spawnKey))
            {
                SpawnObjects();
            }
            else if (Input.GetKeyDown(_resetKey))
            {
                Reset();
            }
            else if (Input.GetKeyDown(_saveKey))
            {
                _storage.Save(this);
            }
            else if (Input.GetKeyDown(_loadKey))
            {
                Reset();
                _storage.Load(this);
            }
        }

        private void SpawnObjects()
        {
            var instance = _shapeFactory.GetRandomShape();
            var instanceTransform = instance.transform;
            instanceTransform.localPosition = Random.insideUnitSphere * _spawnSphereRadius;
            instanceTransform.localRotation = Random.rotation;
            instanceTransform.localScale = Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE);

            _shapes.Add(instance);
        }

        private void Reset()
        {
            foreach (var instance in _shapes)
            {
                Destroy(instance.gameObject);
            }

            _shapes.Clear();
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(-SAVE_FILE_VERSION);
            
            writer.Write(_shapes.Count);
            foreach (var instance in _shapes)
            {
                writer.Write(instance.ShapeId);
                instance.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            var version = -reader.ReadInt();

            if (version > SAVE_FILE_VERSION)
            {
                Debug.LogError("Unsupported save version " + version);
            }
            
            var count = version <= 0 ? -version : reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var shapeId = version > 0 ? reader.ReadInt() : 0;
                var instance = _shapeFactory.GetShape(shapeId);
                instance.Load(reader);
                _shapes.Add(instance);
            }
        }
    }
}
