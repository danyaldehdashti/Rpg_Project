using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;


    private void LateUpdate()
    {
        gameObject.transform.position = target.position;
    }
}
