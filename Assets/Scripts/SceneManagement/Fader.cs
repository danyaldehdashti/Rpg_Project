using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void CallFadeActive()
        {
            _animator.enabled = true;
            _animator.Play(0);
        }
    }
}
