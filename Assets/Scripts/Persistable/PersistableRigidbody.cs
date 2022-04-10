using UnityEngine;

public class PersistableRigidbody : PersistableObject
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_rigidbody.velocity);
        writer.Write(_rigidbody.angularVelocity);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        _rigidbody.velocity = reader.ReadVector();
        _rigidbody.angularVelocity = reader.ReadVector();
    }
}