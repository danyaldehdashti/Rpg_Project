using System;
using Control;
using Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinematics
{
    public class CinematicTrigger : MonoBehaviour,ISaveable
    {
        private bool _alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.transform.GetComponent<PlayerController>())
            {
                GetComponent<PlayableDirector>().Play();
                _alreadyTriggered = true;
            }
        }

        public object CaptureState()
        {
            return _alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            _alreadyTriggered = (bool)state;
        }
    }
}
