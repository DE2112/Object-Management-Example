using UnityEngine;

namespace ObjectManagementExample
{
    public class GameLevel : MonoBehaviour
    {
        [SerializeField] private SpawnZone _spawnZone;
        
        private void Start()
        {
            Game.Instance.SpawnZoneOfLevel = _spawnZone;
        }
    }
}