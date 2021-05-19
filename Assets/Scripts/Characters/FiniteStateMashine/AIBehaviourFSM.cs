using AILogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviourFSM : MonoBehaviour
{
   [SerializeField] private CharacterIdentifier _identifier;
   [SerializeField] private CharacterCombat _combat;
   [SerializeField] private CharacterMovement _movement;
   [SerializeField] private CharacterHealth _health;

    private MapHelper _mapHelper;
    private FiniteStateMachine<AISharedContent> _finiteStateMachine;
    private Dictionary<Type, BaseState<AISharedContent>> _allAIStates = new Dictionary<Type, BaseState<AISharedContent>>();
    private AISharedContent _aiSharedContent;

    private void Start()
    {
        if (!_identifier.IsControlledByThePlayer)
        {
            _mapHelper = ServiceLocator.Resolve<MapHelper>();
            CreateSharedContent();
            InitFSM();
        }
        
    }
    private void Update()
    {
        _finiteStateMachine?.Update();
    }

    private void InitFSM()
    {
        _finiteStateMachine = new FiniteStateMachine<AISharedContent>();
        _allAIStates[typeof(AttackEnemyState)] = new AttackEnemyState(_aiSharedContent);
        _allAIStates[typeof(CrystalSearchState)] = new CrystalSearchState(_aiSharedContent);
        _allAIStates[typeof(EnemySearchState)] = new EnemySearchState(_aiSharedContent);
        _allAIStates[typeof(IdleState)] = new IdleState(_aiSharedContent);
        _allAIStates[typeof(TacticalRetreatState)] = new TacticalRetreatState(_aiSharedContent);
        _allAIStates[typeof(DeadState)] = new DeadState(_aiSharedContent);
        _finiteStateMachine.InitStates(_allAIStates);
        _finiteStateMachine.Switch(typeof(IdleState));
    }

    private void CreateSharedContent()
    {
        _aiSharedContent = new AISharedContent(_identifier, _combat, _movement, _health, _mapHelper);
    }
}
