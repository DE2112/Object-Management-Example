using System.IO;
using UnityEngine;

namespace ObjectManagementExample
{
    public class PersistentStorage : MonoBehaviour
    {
        private string _saveFilePath;

        private void Awake()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile");
        }

        public void Save(PersistableObject persistableObject, int version)
        {
            using (var writer = new BinaryWriter(File.Open(_saveFilePath, FileMode.Create)))
            {
                writer.Write(-version);
                persistableObject.Save(new GameDataWriter(writer));
            }
        }

        public void Load(PersistableObject persistableObject)
        {
            var data = File.ReadAllBytes(_saveFilePath);
            var reader = new BinaryReader(new MemoryStream(data));
            persistableObject.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
    }
}
