using System;
using System.Collections.Generic;
using System.Linq;
using BelousovGameDev.FiniteStateMachine.Predicates;

namespace BelousovGameDev.FiniteStateMachine
{
    internal class StateMachine
    {
        private readonly HashSet<Transition> _anyTransitions = new ();
        private readonly Dictionary<Type, StateNode> _nodes = new ();
        private StateNode _current;

        public event Action StateChanged;

        public IState Current => _current?.State;

        public void Update(float deltaTime)
        {
            Transition transition = GetActivatedTransition();

            if (transition is not null)
            {
                SwitchStateTo(transition.To.GetType());
            }
        }

        public void AddAnyTransition(IState to, IPredicate condition) =>
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));

        public void AddTransition(IState from, IState to, IPredicate condition) =>
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);

        public void SetFirstState(Type type)
        {
            if (_current is null)
            {
                _current = _nodes[type];
                StateChanged?.Invoke();
            }
        }

        private StateNode GetOrAddNode(IState state)
        {
            StateNode node = _nodes.GetValueOrDefault(state.GetType());

            if (node is null)
            {
                node = new StateNode(state);
                _nodes.Add(state.GetType(), node);
            }

            return node;
        }

        private void SwitchStateTo(Type type)
        {
            if (_current.State.GetType() == type)
            {
                return;
            }
            
            _current = _nodes[type];
            StateChanged?.Invoke();
        }

        private Transition GetActivatedTransition() =>
            CheckTransitions(_anyTransitions) ?? CheckTransitions(_current.Transitions);

        private Transition CheckTransitions(IEnumerable<Transition> transitions) =>
            transitions.FirstOrDefault(transition => transition.Condition.Check());

        private class StateNode
        {
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<Transition>();
            }
            
            public IState State { get; }
            
            public HashSet<Transition> Transitions { get; }

            public void AddTransition(IState to, IPredicate predicate) =>
                Transitions.Add(new Transition(to, predicate));
        }
    }
}