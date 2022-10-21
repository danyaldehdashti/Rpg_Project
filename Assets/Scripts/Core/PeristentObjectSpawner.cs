using UnityEngine;

namespace Core
{
    public class PeristentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject savePrefab;

        [SerializeField] private GameObject faderPrefab;

        static bool _hasSpawned = false;

        private void Awake() {
            if (_hasSpawned) return;

            SpawnSavingObject();
            SpawnFaderObject();

            _hasSpawned = true;
        }

        private void SpawnSavingObject()
        {
            GameObject persistentObject = Instantiate(savePrefab);
            DontDestroyOnLoad(persistentObject);
        }
        
        private void SpawnFaderObject()
        {
            GameObject persistentObject = Instantiate(faderPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}