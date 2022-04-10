using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class Shape : PersistableObject
    {
        [SerializeField] private int _shapeId = int.MinValue;
        [SerializeField] private int _materialId;

        public int ShapeId
        {
            get => _shapeId;
            set 
            {
                if (_shapeId == int.MinValue && value != int.MinValue)
                {
                    _shapeId = value;
                }
            }
        }

        public int MaterialId => _materialId;

        public void SetMaterial(Material material, int materialId)
        {
            GetComponent<MeshRenderer>().material = material;
            _materialId = materialId;
        }
    }
}
