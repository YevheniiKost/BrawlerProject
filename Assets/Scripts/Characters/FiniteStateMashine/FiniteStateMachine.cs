using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILogic
{
    public class FiniteStateMachine<T> : IStateSwitcher<T>
        where T : class
    {
        private Dictionary<Type, BaseState<T>> _states = new Dictionary<Type, BaseState<T>>();

        private BaseState<T> _currentState;
        private BaseState<T> _previousState;

        public void InitStates(Dictionary<Type, BaseState<T>> states)
        {
            _states = states;
            foreach(var state in _states)
            {
                state.Value.InitSwitcher(this);
            }
        }

        public void AddState(BaseState<T> state)
        {
            var key = state.GetType();
            _states.Add(key, state);
            state.InitSwitcher(this);
        }

        public void Switch(Type nextState)
        {
            if (!_states.ContainsKey(nextState))
            {
                return;
            }

            if(_currentState != null)
            {
                _currentState.OnStateExit();
            }

            _currentState = _states[nextState];
            _currentState.OnStateEnter();
        }

        public void Update()
        {
            if(_currentState != null)
            {
                _currentState.Execute();
            }
        }
    }

    public abstract class BaseState
    {
        public virtual void OnStateEnter()
        {
        }
        public abstract void Execute();

        public virtual void OnStateExit()
        {

        }
    }
    public abstract class BaseState<T> : BaseState
       where T : class
    {
        protected T _sharedContent;
        protected IStateSwitcher<T> _stateSwitcher;

        public BaseState(T sharedContent)
        {
            _sharedContent = sharedContent;
        }

        public void InitSwitcher(IStateSwitcher<T> stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
    }

    public interface IStateSwitcher<T> where T : class
    {
        void Switch(Type nextState);
    }
}