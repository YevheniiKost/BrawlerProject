using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AILogic;

public class TacticalRetreatState : BaseState<AISharedContent>
{
    public TacticalRetreatState(AISharedContent sharedContent) : base(sharedContent)
    {

    }

    public override void Execute()
    {
        if (_sharedContent.Health.GetLifeStatus() == LifeStatus.NeedHealth)
        {
            if (_sharedContent.Identifier.Team == 0)
                _sharedContent.Movement.SetTarget(_sharedContent.MapHelper.RedCharacterSpawner);
            else if (_sharedContent.Identifier.Team == 1)
                _sharedContent.Movement.SetTarget(_sharedContent.MapHelper.BlueCharacterSpawner);
        }
        else if(_sharedContent.Health.GetLifeStatus() == LifeStatus.Alright)
        {
            _stateSwitcher.Switch(typeof(IdleState));
        }
        else
        {
            _stateSwitcher.Switch(typeof(DeadState));
        }
    }
}
