#nullable enable

using Decoherence.StateMachining;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decoherence.StateMachining
{
    public class ActionResult<TState>
        where TState : notnull
    {
        public bool Successed { get; }

        public TState StateAfterAction { get; set; }

        public object? ErrData { get; set; }

        public ActionResult(TState state, object? errData)
        {
            Successed = errData == null;
            StateAfterAction = state;
            ErrData = errData;
        }
    }

    public class ActionResult<TState, TErrData> : ActionResult<TState>
        where TState : notnull
        where TErrData : class
    {
        public new TErrData? ErrData { get; set; }

        public ActionResult(TState state, object? errData)
            : base(state, errData)
        {
        }
    }
}


#nullable disable