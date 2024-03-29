using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObjectManagementExample
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] _prefabs;
        [SerializeField] private Material[] _materials;
        [SerializeField] private bool _recycle;
        private List<Shape>[] _pools;
        private Scene _poolScene;

        public Shape GetShape(int shapeId, int materialId)
        {
            Shape instance;
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }
                
                var pool = _pools[shapeId];
                var lastIndex = pool.Count - 1;
                if (lastIndex >= 0)
                {
                    instance = pool[lastIndex];
                    instance.gameObject.SetActive(true);
                    pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(_prefabs[shapeId]);
                    instance.ShapeId = shapeId;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
                }
            }
            else
            {
                instance = Instantiate(_prefabs[shapeId]);
                instance.ShapeId = shapeId;
            }
            
            instance.SetMaterial(_materials[materialId], materialId);
            return instance;
        }

        public Shape GetRandomShape()
        {
            return GetShape(Random.Range(0, _prefabs.Length), Random.Range(0, _materials.Length));
        }

        private void CreatePools()
        {
            _pools = new List<Shape>[_prefabs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<Shape>();
            }

            if (Application.isEditor)
            {
                _poolScene = SceneManager.GetSceneByName(name);
                if (_poolScene.isLoaded)
                {
                    var rootObjects = _poolScene.GetRootGameObjects();
                    foreach (var rootObject in rootObjects)
                    {
                        var pooledShape = rootObject.GetComponent<Shape>();
                        if (!pooledShape.gameObject.activeSelf)
                        {
                            _pools[pooledShape.ShapeId].Add(pooledShape);
                        }
                    }
                    return;
                }
            }
            
            _poolScene = SceneManager.CreateScene(name);
        }

        public void ReclaimShape(Shape shape)
        {
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }
                _pools[shape.ShapeId].Add(shape);
                shape.gameObject.SetActive(false);
            }
            else
            {
                Destroy(shape.gameObject);
            }
        }
    }
}