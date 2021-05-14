using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JiseleStaff : MonoBehaviour
{
    [SerializeField] private Transform _circle;
    [SerializeField] private float _rotationSpeed;

    private Vector3 _rotationVector;
    private float _rot;

    private void Start()
    {
        _circle.transform.DOLocalRotate(new Vector3(0, 360, 0), _rotationSpeed).SetLoops(-1);
    }
    private void Update()
    {
        _rot += _rotationSpeed;
        _circle.transform.localRotation = Quaternion.Euler(new Vector3(0,_rot,0));
    }
}
