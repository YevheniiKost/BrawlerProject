
using AILogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState<AISharedContent>
{

    public DeadState(AISharedContent sharedContent) : base(sharedContent)
    {

    }

    public override void Execute()
    {
        if(_sharedContent.Health.GetLifeStatus() == LifeStatus.Alright)
        {
            _stateSwitcher.Switch(typeof(IdleState));
        }
        else if (_sharedContent.Health.GetLifeStatus() == LifeStatus.NeedHealth)
        {
            _stateSwitcher.Switch(typeof(TacticalRetreatState));
        }
        else
        {
            return;
        }
    }
}
