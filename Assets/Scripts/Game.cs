using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    private const float MIN_SIZE = 0.1f;
    private const float MAX_SIZE = 1f;
    
    [SerializeField] private Transform _objectPrefab;
    [SerializeField] private float _spawnSphereRadius;
    [SerializeField] private List<Transform> _objectTransforms;
    
    [Header("Keys")]
    [SerializeField] private KeyCode _spawnKey;
    [SerializeField] private KeyCode _reloadKey;

    private void Awake()
    {
        _objectTransforms = new List<Transform>();
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
            SpawnObject();
        }
        else if (Input.GetKeyDown(_reloadKey))
        {
            Reload();
        }
    }
    
    private void SpawnObject()
    {
        var objectTransform = Instantiate(_objectPrefab).transform;
        objectTransform.localPosition = Random.insideUnitSphere * _spawnSphereRadius;
        objectTransform.localRotation = Random.rotation;
        objectTransform.localScale = Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE);
        _objectTransforms.Add(objectTransform);
    }

    private void Reload()
    {
        foreach (var objectTransform in _objectTransforms)
        {
            Destroy(objectTransform.gameObject);
        }
        _objectTransforms.Clear();
    }
}
