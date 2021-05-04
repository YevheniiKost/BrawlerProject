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
   [SerializeField] private AIMapHelper _mapHelper;

    private FiniteStateMachine<AISharedContent> _finiteStateMachine;
    private Dictionary<Type, BaseState<AISharedContent>> _allAIStates = new Dictionary<Type, BaseState<AISharedContent>>();
    private AISharedContent _aiSharedContent;

    private void Start()
    {
        CreateSharedContent();
        InitFSM();
    }
    private void Update()
    {
        _finiteStateMachine.Update();
    }

    private void InitFSM()
    {
        throw new NotImplementedException();
    }

    private void CreateSharedContent()
    {
        _aiSharedContent = new AISharedContent(_identifier, _combat, _movement, _health, _mapHelper);
    }
}
