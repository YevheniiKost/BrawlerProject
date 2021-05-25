using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    [SerializeField] private float _destroyTimer;

    private void Start()
    {
        Destroy(this.gameObject, _destroyTimer);
    }
}
