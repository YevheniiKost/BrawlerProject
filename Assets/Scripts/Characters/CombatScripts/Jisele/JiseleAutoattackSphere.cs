using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiseleAutoattackSphere : MonoBehaviour
{
    private Vector3 _direction = Vector3.zero;
    private int _damage;
    private float _speed;
    private float _distance;
    private CharacterIdentifier _jiesel;

    private float _lifeTime;

    public void SetData(Vector3 direction, int damage, CharacterIdentifier character, float speed, float distance)
    {
        _direction = direction;
        _damage = damage;
        _jiesel = character;
        _speed = speed;
        _distance = distance;

        _lifeTime = _distance / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier enemy))
        {
            if(enemy.Team != _jiesel.Team)
            {
                enemy.GetComponent<CharacterHealth>()?.ModifyHealth(-_damage, _jiesel);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        MovingForvard();

        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void MovingForvard()
    {
        transform.Translate(_direction * (Time.deltaTime * (_speed)));
    }
}
