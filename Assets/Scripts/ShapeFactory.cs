using UnityEngine;

namespace ObjectManagementExample
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] prefabs;

        public Shape GetShape(int shapeId)
        {
            var instance = Instantiate(prefabs[shapeId]);
            instance.ShapeId = shapeId;
            return instance;
        }

        public Shape GetRandomShape()
        {
            return GetShape(Random.Range(0, prefabs.Length));
        }
    }
}