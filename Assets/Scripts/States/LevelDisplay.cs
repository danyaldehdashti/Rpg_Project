using System;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace States
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaceState _baceState;

        private void Awake()
        {
            _baceState = FindObjectOfType<PlayerController>().GetComponent<BaceState>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}",_baceState.GetLevel());
        }
    }
}
