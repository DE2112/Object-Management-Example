using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    private const float MIN_SIZE = 0.1f;
    private const float MAX_SIZE = 1f;

    [SerializeField] private PersistentStorage _storage;
    [SerializeField] private PersistableObject _objectPrefab;
    [SerializeField] private float _spawnSphereRadius;
    [SerializeField] private List<PersistableObject> _persistableObjects;

    [Header("Keys")]
    [SerializeField] private KeyCode _spawnKey;
    [SerializeField] private KeyCode _resetKey;
    [SerializeField] private KeyCode _saveKey;
    [SerializeField] private KeyCode _loadKey;
    
    private void Awake()
    {
        _persistableObjects = new List<PersistableObject>();
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
        var persistableObject = Instantiate(_objectPrefab);
        var objectTransform = persistableObject.transform;
        objectTransform.localPosition = Random.insideUnitSphere * _spawnSphereRadius;
        objectTransform.localRotation = Random.rotation;
        objectTransform.localScale = Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE);
        
        _persistableObjects.Add(persistableObject);
    }

    private void Reset()
    {
        foreach (var persistableObject in _persistableObjects)
        {
            Destroy(persistableObject.gameObject);
        }
        _persistableObjects.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(_persistableObjects.Count);
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
            var persistableObject = Instantiate(_objectPrefab);
            persistableObject.Load(reader);
            _persistableObjects.Add(persistableObject);
        }
    }
}
