using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class Shape : PersistableObject
    {
        [SerializeField] private int _shapeId = int.MinValue;

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
    }
}
