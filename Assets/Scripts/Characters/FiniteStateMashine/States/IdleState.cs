using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AILogic;

public class IdleState : BaseState<AISharedContent>
{
    private int _indexIfPath;
    public IdleState(AISharedContent sharedContent) : base(sharedContent)
    {
    }

    public override void Execute()
    {
        if (_sharedContent.Health.PercentOfHealth() > _sharedContent.Health.PercentOfHealthToStartRetreat)
        {
            if (_sharedContent.Combat.IsEnemyDetected)
            {
                _stateSwitcher.Switch(typeof(AttackEnemyState));
            }
            else if (!_sharedContent.Combat.IsEnemyDetected && _sharedContent.MapHelper.IsSomeOfEnemiesCrystalsActive(_sharedContent.Identifier))
            {
                _stateSwitcher.Switch(typeof(CrystalSearchState));
            } else if(!_sharedContent.Combat.IsEnemyDetected && !_sharedContent.MapHelper.IsSomeOfEnemiesCrystalsActive(_sharedContent.Identifier))
            {
                _stateSwitcher.Switch(typeof(EnemySearchState));
            }
        }
        else
        {
            _stateSwitcher.Switch(typeof(TacticalRetreatState));
        }
    }
}
