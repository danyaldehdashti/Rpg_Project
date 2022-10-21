using System;
using System.Collections;
using Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        
        
        [SerializeField] private int sceneToLoad;

        [SerializeField] private Transform spawnPoint;

        [SerializeField] private DestinationIdentifier destination;

        [SerializeField] private float waitTime;
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>())
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene To Load Not Set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            
            fader.CallFadeActive();

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            PlayerController playerController = FindObjectOfType<PlayerController>();

            playerController.enabled = false;   
            
            savingWrapper.CallSaveAndLoad();
            
            PlayerController newPlayerController = FindObjectOfType<PlayerController>();

           newPlayerController.enabled = false;
            
            yield return new WaitForSeconds(waitTime);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            newPlayerController.enabled = true;
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);


            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portal)
        {
            GameObject player = FindObjectOfType<PlayerController>().gameObject;

            player.GetComponent<NavMeshAgent>().enabled = false;

            player.GetComponent<NavMeshAgent>().Warp(player.transform.position = portal.spawnPoint.position);

            player.transform.rotation = portal.spawnPoint.rotation;
            
            player.GetComponent<NavMeshAgent>().enabled = true;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }
            
            return null;
        }
    }
    
}
