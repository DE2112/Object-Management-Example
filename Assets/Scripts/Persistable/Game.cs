using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ObjectManagementExample
{
    public class Game : PersistableObject
    {
        private const float MIN_SIZE = 0.1f;
        private const float MAX_SIZE = 1f;
        private const int SAVE_FILE_VERSION = 3;
        private const float SPAWN_PERIOD = 1f;
        private const float DESTRUCTION_PERIOD = 1f;

        private static Game _instance;
        
        [SerializeField] private PersistentStorage _storage;
        [SerializeField] private ShapeFactory _shapeFactory;
        [SerializeField] private List<Shape> _shapes;
        private float _spawnSpeed, _destructionSpeed;
        private float _spawnTimer, _destructionTimer;
        [SerializeField] private int _levelCount;
        private int _loadedLevelBuildIndex;
        [SerializeField] private SpawnZone _spawnZoneOfLevel;
        private Random.State _mainRandomState;
        [SerializeField] private bool _reseedOnBool;

        [Header("Keys")]
        [SerializeField] private KeyCode _spawnKey;
        [SerializeField] private KeyCode _resetKey;
        [SerializeField] private KeyCode _saveKey;
        [SerializeField] private KeyCode _loadKey;
        [SerializeField] private KeyCode _destroyKey;

        public static Game Instance
        {
            get => _instance;
            set => _instance = value;
        }
        
        public float SpawnSpeed
        {
            get => _spawnSpeed;
            set => _spawnSpeed = value;
        }

        public float DestructionSpeed
        {
            get => _destructionSpeed;
            set => _destructionSpeed = value;
        }

        public SpawnZone SpawnZoneOfLevel
        {
            get => _spawnZoneOfLevel;
            set => _spawnZoneOfLevel = value;
        }

        private void OnEnable()
        {
            _instance = this;
        }

        private void Start()
        {
            _mainRandomState = Random.state;
            _shapes = new List<Shape>();

            if (Application.isEditor)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var loadedScene = SceneManager.GetSceneAt(i);
                    if (loadedScene.name.Contains("Level "))
                    {
                        SceneManager.SetActiveScene(loadedScene);
                        _loadedLevelBuildIndex = loadedScene.buildIndex;
                        return;
                    }
                }
            }
            
            Reset();
            StartCoroutine(LoadLevel(1));
        }

        private void Update()
        {
            ProcessInput();

            _spawnTimer += Time.deltaTime * _spawnSpeed;
            while (_spawnTimer >= SPAWN_PERIOD)
            {
                _spawnTimer -= SPAWN_PERIOD;
                CreateRandomShape();
            }
            
            _destructionTimer += Time.deltaTime * _destructionSpeed;
            while (_destructionTimer >= DESTRUCTION_PERIOD)
            {
                _destructionTimer -= DESTRUCTION_PERIOD;
                DestroyRandomShape();
            }
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(_spawnKey))
            {
                CreateRandomShape();
            }
            else if (Input.GetKeyDown(_destroyKey))
            {
                DestroyRandomShape();
            }
            else if (Input.GetKeyDown(_resetKey))
            {
                Reset();
            }
            else if (Input.GetKeyDown(_saveKey))
            {
                _storage.Save(this, SAVE_FILE_VERSION);
            }
            else if (Input.GetKeyDown(_loadKey))
            {
                Reset();
                _storage.Load(this);
            }
            else
            {
                for (int i = 1; i <= _levelCount; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        Reset();
                        StartCoroutine(LoadLevel(i));
                        return;
                    }
                }
            }
        }

        private IEnumerator LoadLevel(int levelBuildIndex)
        {
            enabled = false;
            if (_loadedLevelBuildIndex > 0)
            {
                yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
            }
            yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
            _loadedLevelBuildIndex = levelBuildIndex;
            enabled = true;
        }

        private void CreateRandomShape()
        {
            var instance = _shapeFactory.GetRandomShape();
            var instanceTransform = instance.transform;
            instanceTransform.localPosition = _spawnZoneOfLevel.GetSpawnPoint();
            instanceTransform.localRotation = Random.rotation;
            instanceTransform.localScale = Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE);
            instance.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
            _shapes.Add(instance);
        }

        private void DestroyRandomShape()
        {
            if (_shapes.Count > 0)
            {
                var index = Random.Range(0, _shapes.Count);
                _shapeFactory.ReclaimShape(_shapes[index]);
                var lastIndex = _shapes.Count - 1;
                _shapes[index] = _shapes[lastIndex];
                _shapes.RemoveAt(lastIndex);
            }
        }

        private void Reset()
        {
            Random.state = _mainRandomState;
            var seed = Random.Range(0, int.MaxValue) ^ (int)Time.unscaledTime;
            Random.InitState(seed);
            
            foreach (var instance in _shapes)
            {
                _shapeFactory.ReclaimShape(instance);
            }

            _shapes.Clear();
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(_shapes.Count);
            writer.Write(Random.state);
            writer.Write(_loadedLevelBuildIndex);
            foreach (var instance in _shapes)
            {
                writer.Write(instance.ShapeId);
                writer.Write(instance.MaterialId);
                instance.Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            var version = reader.Version;

            if (version > SAVE_FILE_VERSION)
            {
                Debug.LogError("Unsupported save version " + version);
            }
            
            var count = version <= 0 ? -version : reader.ReadInt();

            if (version >= 3)
            {
                var state = reader.ReadRandomState();
                if (!_reseedOnBool)
                {
                    Random.state = state;
                }
            }
            
            StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
            for (int i = 0; i < count; i++)
            {
                var shapeId = version > 0 ? reader.ReadInt() : 0;
                var materialId = version > 0 ? reader.ReadInt() : 0;
                var instance = _shapeFactory.GetShape(shapeId, materialId);
                instance.Load(reader);
                _shapes.Add(instance);
            }
        }
    }
}
