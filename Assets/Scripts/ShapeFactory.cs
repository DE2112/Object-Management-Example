using UnityEngine;

namespace ObjectManagementExample
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] _prefabs;
        [SerializeField] private Material[] _materials; 

        public Shape GetShape(int shapeId, int materialId)
        {
            var instance = Instantiate(_prefabs[shapeId]);
            instance.ShapeId = shapeId;
            instance.SetMaterial(_materials[materialId], materialId);
            return instance;
        }

        public Shape GetRandomShape()
        {
            return GetShape(Random.Range(0, _prefabs.Length), Random.Range(0, _materials.Length));
        }
    }
}