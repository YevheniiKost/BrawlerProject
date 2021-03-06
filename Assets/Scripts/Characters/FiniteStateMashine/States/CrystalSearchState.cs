using AILogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSearchState : BaseState<AISharedContent>
{
    public CrystalSearchState(AISharedContent sharedContent) : base(sharedContent)
    {
    }

    public override void Execute()
    {
        if (_sharedContent.Health.GetLifeStatus() == LifeStatus.Alright || _sharedContent.Health.GetLifeStatus() == LifeStatus.FullHealth)
        {
            if (!_sharedContent.Combat.IsEnemyDetected)
            {
                if (_sharedContent.MapHelper.IsSomeOfEnemiesCrystalsActive(_sharedContent.Identifier))
                {
                    _sharedContent.Movement.SetTarget(_sharedContent.MapHelper.LocateNearestActiveEnemyCrystal(_sharedContent.Identifier));
                }
                else if (_sharedContent.MapHelper.IsSomeFriendlyCrystalsActive(_sharedContent.Identifier))
                {
                    _sharedContent.Movement.SetTarget(_sharedContent.MapHelper.LocateNearestActiveFriendlyCrystal(_sharedContent.Identifier));
                }
                else
                {
                    _stateSwitcher.Switch(typeof(AttackEnemyState));
                }
            }
            else
            {
                _stateSwitcher.Switch(typeof(AttackEnemyState));
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
