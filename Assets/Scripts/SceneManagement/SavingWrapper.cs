using System;
using System.Collections;
using Saving;
using UnityEngine;

namespace SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private SavingSystem _savingSystem;

        private const string DefaultSaveFile = "newSaveFile";
        
        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        private void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        private void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }

        public void CallSaveAndLoad()
        {
            StartCoroutine(SaveAndLoad());
        }

        private void Delete()
        {
            _savingSystem.Delete(DefaultSaveFile);
        }

        private IEnumerator SaveAndLoad()
        {
            Save();

            yield return new WaitForSeconds(1f);
            
            Load();
        }
    }
}
