using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiseleFireball : MonoBehaviour
{
    private int _friendlyTeam;
    private int _damage;
    private float _speed;
    private float _distance;
    private Vector3 _direction;
    private CharacterIdentifier _thrower;

    private float _lifeTime;

    private List<CharacterIdentifier> _enemyList = new List<CharacterIdentifier>();

    public void SetData(int damage, float speed, float distance, Vector3 direction, CharacterIdentifier thrower)
    {
        _friendlyTeam = thrower.Team;
        _damage = damage;
        _speed = speed;
        _distance = distance;
        _direction = direction;
        _thrower = thrower;

        _lifeTime = _distance / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.TryGetComponent(out CharacterIdentifier enemy))
        {
            if (enemy.Team != _thrower.Team && !_enemyList.Contains(enemy))
            {
                enemy.GetComponent<CharacterHealth>().ModifyHealth(-_damage, _thrower);
                ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.Jisele02Hit);
                _enemyList.Add(enemy);
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
