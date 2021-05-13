using AILogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchState : BaseState<AISharedContent>
{
    public EnemySearchState(AISharedContent sharedContent) : base(sharedContent)
    {
    }

    public override void Execute()
    {
        if (_sharedContent.Health.GetLifeStatus() == LifeStatus.Alright)
        {
            if (!_sharedContent.MapHelper.IsAllEnemiesDead(_sharedContent.Identifier))
            {
                if (_sharedContent.MapHelper.DistanceToNearestEnemy(_sharedContent.Identifier) > _sharedContent.Combat.EnemyDetectRadius)
                {
                    _sharedContent.Movement.SetTarget(_sharedContent.MapHelper.GetNearestEnemy(_sharedContent.Identifier).transform);
                }
                else
                {
                    _stateSwitcher.Switch(typeof(AttackEnemyState));
                }
            }
            else
            {
                _stateSwitcher.Switch(typeof(IdleState));
            }

        }
        else if (_sharedContent.Health.GetLifeStatus() == LifeStatus.NeedHealth)
        {
            _stateSwitcher.Switch(typeof(TacticalRetreatState));
        }
        else
        {
            _stateSwitcher.Switch(typeof(DeadState));
        }
    }

}
