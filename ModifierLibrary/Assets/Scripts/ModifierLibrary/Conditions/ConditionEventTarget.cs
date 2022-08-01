using System;
using UnitLibrary;

namespace ModifierLibrary
{
    /// <summary>
    ///     Owner/Retriever - Acter/Giver
    /// </summary>
    public enum ConditionEventTarget
    {
        None = 0,
        /// <summary>
        ///     Default. Owner/Retriever - Acter/Giver
        /// </summary>
        SelfActer = 1,
        ActerSelf = 2,
        SelfSelf = 3,
        ActerActer = 4,

        Default = SelfActer,
    }

    [Obsolete]
    public struct ConditionEventData
    {
        public readonly ConditionEventTarget ConditionEventTarget;
        public readonly ConditionEvent ConditionEvent;

        public ConditionEventData(ConditionEventTarget conditionEventTarget, ConditionEvent conditionEvent)
        {
            ConditionEventTarget = conditionEventTarget;
            ConditionEvent = conditionEvent;

            if(ConditionEventTarget == ConditionEventTarget.None)
                Log.Error("Wrong ConditionTarget, None");
            if(ConditionEvent == ConditionEvent.None)
                Log.Error("Wrong UnitConditionEvent, None");
        }
    }
}