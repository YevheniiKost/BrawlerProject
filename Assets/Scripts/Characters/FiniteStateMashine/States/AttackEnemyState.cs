using AILogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyState : BaseState<AISharedContent>
{
    public AttackEnemyState(AISharedContent sharedContent) : base(sharedContent)
    {

    }

    public override void Execute()
    {
        if (_sharedContent.Health.GetLifeStatus() == LifeStatus.Alright)
        {
            if (_sharedContent.Combat.Target != null)
            {
                if (Vector3.Distance((_sharedContent.Combat.Target).position, _sharedContent.Identifier.transform.position) > _sharedContent.Combat.AutoattackRange)
                {
                    _sharedContent.Movement.SetTarget(_sharedContent.Combat.Target);
                }
                else
                {
                    _sharedContent.Movement.StopMovement();
                    if (!_sharedContent.Movement.IsCharacterMoving)
                        _sharedContent.Combat.AttackBehavior();
                }
            }
            else
            {
                _stateSwitcher.Switch(typeof(CrystalSearchState));
            }
        }
        else if(_sharedContent.Health.GetLifeStatus() == LifeStatus.NeedHealth)
        {
            _stateSwitcher.Switch(typeof(TacticalRetreatState));
        }
        else
        {
            _stateSwitcher.Switch(typeof(DeadState));
        }
    }
}
