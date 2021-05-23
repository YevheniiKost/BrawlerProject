using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private Tweener _tweener;
    void Start()
    {
        _tweener = transform.DOLocalRotate(new Vector3(0, 360, 0), _rotationSpeed).SetLoops(-1);
    }

    private void OnDestroy()
    {
        _tweener.Kill(false);
    }

}
