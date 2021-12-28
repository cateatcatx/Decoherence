#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Decoherence.StateMachining
{
    public class StateMachine<TState, TAction, TActionArg>
        where TState : notnull
        where TAction : notnull
    {
        //public static ActionFunc ToActionFunc<TErrData>(ActionFunc<TErrData> func)
        //    where TErrData : class
        //{
        //    return (machine, arg) => func(machine, arg);
        //}

        public delegate object? ActionFunc(StateMachine<TState, TAction, TActionArg> machine, TActionArg arg);

        //public delegate TErrData? ActionFunc<TErrData>(StateMachine<TState, TAction, TActionArg> machine, TActionArg arg) 
        //    where TErrData : class;

        public class StateConfiguration
        {
            public Dictionary<TAction, ActionFunc> ActionFuncs
            {
                get
                {
                    if (mActionFuncs == null)
                    {
                        mActionFuncs = new Dictionary<TAction, ActionFunc>();
                    }
                    return mActionFuncs;
                }
                set
                {
                    mActionFuncs = value;
                }
            }

            private Dictionary<TAction, ActionFunc>? mActionFuncs;
        }

        private TState mState;

        private readonly Dictionary<TState, StateConfiguration> mConfigurations;

#if NET35
        public StateMachine(TState initState, Dictionary<TState, StateConfiguration> configurations)
#else
        public StateMachine(TState initState, IReadOnlyDictionary<TState, StateConfiguration> configurations)
#endif
        {
            mState = initState;
            mConfigurations = new Dictionary<TState, StateConfiguration>();

            foreach (var pair in configurations)
            {
                ((ICollection<KeyValuePair<TState, StateConfiguration>>)mConfigurations).Add(pair);
            }
        }

        public bool CanTrigger(TAction action)
        {
            return _CanTrigger(action, out _);
        }

        public ActionResult<TState, TErrData> Trigger<TErrData>(TAction action, TActionArg arg)
            where TErrData : class
        {
            if (!_CanTrigger(action, out var func))
            {
                throw new InvalidOperationException();
            }

#if GREATER_EQUAL_NETSTANDARD21
            var errData = func(this, arg);
#else
            var errData = func!(this, arg);
#endif

            return new ActionResult<TState, TErrData>(mState, errData);
        }

#if GREATER_EQUAL_NETSTANDARD21
        private bool _CanTrigger(TAction action, [NotNullWhen(true)] out ActionFunc? func)
#else
        private bool _CanTrigger(TAction action, out ActionFunc? func)
#endif
        {
            func = null;

            if (!mConfigurations.TryGetValue(mState, out var configuration))
            {
                return false;
            }

            return configuration.ActionFuncs.TryGetValue(action, out func);
        }
    }
}



#nullable disable