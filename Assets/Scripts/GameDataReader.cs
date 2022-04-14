using System.IO;
using UnityEngine;

namespace ObjectManagementExample
{
    public class GameDataReader
    {
        private BinaryReader _reader;
        private int _version;
        public int Version => _version;

        public GameDataReader(BinaryReader reader, int version)
        {
            _reader = reader;
            _version = version;
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

        public Color ReadColor()
        {
            Color value;
            value.r = _reader.ReadSingle();
            value.g = _reader.ReadSingle();
            value.b = _reader.ReadSingle();
            value.a = _reader.ReadSingle();
            return value;
        }

        public Random.State ReadRandomState()
        {
            return JsonUtility.FromJson<Random.State>(_reader.ReadString());
        }
    }
}