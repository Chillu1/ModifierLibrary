using System;
using BaseProject;

namespace ModifierSystem
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
        public ConditionEventTarget conditionEventTarget;
        public ConditionEvent conditionEvent;

        public ConditionEventData(ConditionEventTarget conditionEventTarget, ConditionEvent conditionEvent)
        {
            this.conditionEventTarget = conditionEventTarget;
            this.conditionEvent = conditionEvent;

            if(this.conditionEventTarget == ConditionEventTarget.None)
                Log.Error("Wrong ConditionTarget, None");
            if(this.conditionEvent == ConditionEvent.None)
                Log.Error("Wrong BeingConditionEvent, None");
        }
    }
}