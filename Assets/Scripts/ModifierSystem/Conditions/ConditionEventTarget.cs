using BaseProject;

namespace ModifierSystem
{
    public enum ConditionEventTarget
    {
        None = 0,
        /// <summary>
        ///     Owner
        /// </summary>
        Self = 1,
        Acter = 2,
        SelfSelf = 3,
        ActerActer = 4,
    }

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