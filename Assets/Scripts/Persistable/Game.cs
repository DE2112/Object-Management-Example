using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ObjectManagementExample;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
    private const float MIN_SIZE = 0.1f;
    private const float MAX_SIZE = 1f;

    [SerializeField] private PersistentStorage _storage;
    [SerializeField] private ShapeFactory _shapeFactory;
    [SerializeField] private float _spawnSphereRadius;
    [SerializeField] private List<Shape> _instances;

    [Header("Keys")]
    [SerializeField] private KeyCode _spawnKey;
    [SerializeField] private KeyCode _resetKey;
    [SerializeField] private KeyCode _saveKey;
    [SerializeField] private KeyCode _loadKey;
    
    private void Awake()
    {
        _instances = new List<Shape>();
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
        
        _instances.Add(instance);
    }

    private void Reset()
    {
        foreach (var instance in _instances)
        {
            Destroy(instance.gameObject);
        }
        _instances.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(_instances.Count);
        foreach (var persistableObject in _instances)
        {
            persistableObject.Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        var count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            var instance = _shapeFactory.GetShape(0);
            instance.Load(reader);
            _instances.Add(instance);
        }
    }
}
