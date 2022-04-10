using System;
using UnityEngine;

namespace ObjectManagementExample
{
    public class Shape : PersistableObject
    {
        private static readonly int colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock sharedPropertyBlock;
        
        [SerializeField] private int _shapeId = int.MinValue;
        [SerializeField] private int _materialId;
        [SerializeField] private Color _color;
        [SerializeField] private MeshRenderer _renderer;

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
        public Color Color => _color;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void SetMaterial(Material material, int materialId)
        {
            _renderer.material = material;
            _materialId = materialId;
        }

        public void SetColor(Color color)
        {
            _color = color;
            if (sharedPropertyBlock == null) {
                sharedPropertyBlock = new MaterialPropertyBlock();
            }
            sharedPropertyBlock.SetColor(colorPropertyId, color);
            _renderer.SetPropertyBlock(sharedPropertyBlock);
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            writer.Write(_color);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }
    }
}
