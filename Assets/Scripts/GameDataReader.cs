using System.IO;
using UnityEngine;

namespace ObjectManagementExample
{
    public class GameDataReader
    {
        private BinaryReader _reader;

        public GameDataReader(BinaryReader reader)
        {
            _reader = reader;
        }

        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        public Quaternion ReadQuaternion()
        {
            Quaternion value;
            value.x = _reader.ReadSingle();
            value.y = _reader.ReadSingle();
            value.z = _reader.ReadSingle();
            value.w = _reader.ReadSingle();
            return value;
        }

        public Vector3 ReadVector()
        {
            Vector3 value;
            value.x = _reader.ReadSingle();
            value.y = _reader.ReadSingle();
            value.z = _reader.ReadSingle();
            return value;
        }
    }
}