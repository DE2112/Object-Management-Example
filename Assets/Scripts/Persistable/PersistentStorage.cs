using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour 
{
    private string _saveFilePath;
    
    private void Awake ()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    public void Save(PersistableObject persistableObject)
    {
        using (var writer = new BinaryWriter(File.Open(_saveFilePath, FileMode.Create)))
        {
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject persistableObject)
    {
        using (var reader = new BinaryReader(File.Open(_saveFilePath, FileMode.Open)))
        {
            persistableObject.Load(new GameDataReader(reader));
        }
    }
}
