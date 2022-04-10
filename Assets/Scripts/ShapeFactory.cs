using UnityEngine;

namespace ObjectManagementExample
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] prefabs;

        public Shape GetShape(int shapeId)
        {
            return Instantiate(prefabs[shapeId]);
        }

        public Shape GetRandomShape()
        {
            return GetShape(Random.Range(0, prefabs.Length));
        }
    }
}