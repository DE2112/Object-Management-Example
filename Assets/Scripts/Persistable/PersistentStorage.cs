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
            using (var reader = new BinaryReader(File.Open(_saveFilePath, FileMode.Open)))
            {
                persistableObject.Load(new GameDataReader(reader, -reader.ReadInt32()));
            }
        }
    }
}
