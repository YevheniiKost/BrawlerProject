using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorShieldScript : MonoBehaviour
{
    private int _friendlyTeam;
    private int _damage;
    private float _speed;
    private float _distance;
    private Vector3 _direction;

    private float _lifeTime;
    private int _contactsCount;

    public void GetData(int friendlyTeam, int damage, float speed, float distance, Vector3 direction)
    {
        _friendlyTeam = friendlyTeam;
        _damage = damage;
        _speed = speed;
        _distance = distance;
        _direction = direction;

        _lifeTime = _distance / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier character))
        {
            if (character.Team != _friendlyTeam)
            {
                character.GetComponent<CharacterHealth>()?.ModifyHealth(-_damage);
                _contactsCount++;
                if(_contactsCount >= 2)
                {
                    Destroy(this.gameObject);
                }
                //todo add stun effect
                var nextTarget = ServiceLocator.Resolve<MapHelper>()?.GetNearestFriends(character);

                if(Vector3.Distance(nextTarget.transform.position, transform.position) < _distance)
                {
                    _lifeTime *= 2;
                    var newDir = (nextTarget.transform.position - transform.position).normalized;
                    _direction = new Vector3(newDir.x, _direction.y, newDir.z);   
                }
            } 
        }
    }


    private void Update()
    {
        MovingForvard();

        _lifeTime -= Time.deltaTime;
        if(_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void MovingForvard()
    {
        transform.Translate(_direction * (Time.deltaTime * (_speed)));
    }
}
