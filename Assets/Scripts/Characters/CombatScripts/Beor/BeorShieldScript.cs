using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorShieldScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticles;

    private int _friendlyTeam;
    private int _damage;
    private float _speed;
    private float _distance;
    private float _stunDuration;
    private Vector3 _direction;
    private CharacterIdentifier _thrower;

    private float _lifeTime;
    private int _contactsCount;

    public void GetData(int damage, float speed, float distance, Vector3 direction, CharacterIdentifier thrower, float duration)
    {
        _friendlyTeam = thrower.Team;
        _damage = damage;
        _speed = speed;
        _distance = distance;
        _direction = direction;
        _thrower = thrower;
        _stunDuration = duration;

        _lifeTime = _distance / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier character))
        {
            if (character.Team != _friendlyTeam)
            {
                character.GetComponent<CharacterHealth>()?.ModifyHealth(-_damage, _thrower);
                ServiceLocator.Resolve<CharacterEffectsManager>()?.StunEffect(character, _stunDuration);

                ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.Beor03Hit);

                EventAggregator.Post(this, new ShakeCamera { Intencity = 2, Time = .3f });
                Instantiate(_hitParticles, other.transform.position, Quaternion.identity);

                _contactsCount++;

                if(_contactsCount >= 2)
                {
                    Destroy(this.gameObject);
                }
                
                var nextTarget = ServiceLocator.Resolve<MapHelper>()?.GetNearestFriends(character);
                if (nextTarget != null)
                {
                    if (Vector3.Distance(nextTarget.transform.position, transform.position) < _distance)
                    {
                        _lifeTime += (_distance / _speed);
                        var newDir = (nextTarget.transform.position - transform.position).normalized;
                        _direction = new Vector3(newDir.x, _direction.y, newDir.z);
                    }
                }
                else
                {
                    Destroy(this.gameObject);
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
