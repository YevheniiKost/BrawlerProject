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
        _stateSwitcher.Switch(typeof(IdleState));
    }

}
