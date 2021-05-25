using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JiseleMeteor : MonoBehaviour
{
    [SerializeField] private Transform _meteor;
    [SerializeField] private Transform _base;
    [SerializeField] private float _meteorSpeed;
    [SerializeField] private Vector3 _meteorStartPosition;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem _burningArea;

    private int _meteorExplodeDamage;
    private float _meteorExplodeRadius;
    private int _areaDamage;
    private float _areaDamageRate;
    private float _areaRadius;
    private float _areaLifeTime;

    private CharacterIdentifier _caster;

    public void GetData(int explosionDamage, int areaDamage, float explosionRadius, float areaDamageRate, float areaRadius,float areaLifeTime, CharacterIdentifier caster)
    {
        _meteor.transform.localPosition = _meteorStartPosition;

        _meteorExplodeRadius = explosionRadius;
        _meteorExplodeDamage = explosionDamage;
        _areaDamage = areaDamage;
        _areaDamageRate = areaDamageRate;
        _areaRadius = areaRadius;
        _areaLifeTime = areaLifeTime;
        _caster = caster;

        StartMeteorFalling();
    }

    private void StartMeteorFalling()
    {
        _meteor.DOBlendableLocalMoveBy(_meteorStartPosition * -1, 1 / _meteorSpeed).SetEase(Ease.InQuad).OnComplete(Explosion);
        
    }


    private void Explosion()
    {
        Destroy(_meteor.gameObject);
        _explosion.Play();
        _burningArea.Play();
        ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.Jisele03Hit);
        if (_caster.IsControlledByThePlayer)
            EventAggregator.Post(this, new ShakeCamera { Intencity = 5, Time = .5f });
        var col = Physics.OverlapSphere(transform.position, _meteorExplodeRadius);
        for (int i = 0; i < col.Length; i++)
        {
            if(col[i].TryGetComponent(out CharacterIdentifier enemy))
            {
                if(enemy.Team != _caster.Team)
                {
                    enemy.GetComponent<CharacterHealth>().ModifyHealth(-_meteorExplodeDamage, _caster);
                }
            }
        }
        _base.GetComponent<JiseleMeteorDamageArea>()?.StartBurning(_caster, _areaLifeTime, _areaDamage, _areaDamageRate, _areaRadius);
    }
}
