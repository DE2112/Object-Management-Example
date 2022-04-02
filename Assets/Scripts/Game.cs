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
    [SerializeField] private KeyCode _spawnKey;
    [SerializeField] private float _spawnSphereRadius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _spawnSphereRadius);
        Gizmos.color = Color.red;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_spawnKey))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        var objectTransform = Instantiate(_objectPrefab).transform;
        objectTransform.localPosition = Random.insideUnitSphere * _spawnSphereRadius;
        objectTransform.localRotation = Random.rotation;
        objectTransform.localScale = Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE);
    }
}
