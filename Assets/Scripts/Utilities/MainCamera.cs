using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
    }
}
