using System;

namespace BelousovGameDev.FiniteStateMachine.Predicates
{
    internal class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _func;

        public FuncPredicate(Func<bool> func) =>
            _func = func;
        
        public bool Check() =>
            _func.Invoke();
    }
}