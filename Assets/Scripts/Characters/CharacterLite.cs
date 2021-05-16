using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLite : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorOverrideController _overrideController;

    private void Start()
    {
        if(_overrideController != null)
        {
            _animator.runtimeAnimatorController = _overrideController;
        }
    }
}
