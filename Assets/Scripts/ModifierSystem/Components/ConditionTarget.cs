using BaseProject;

namespace ModifierSystem
{
    public enum ConditionTarget
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

    public struct ConditionData
    {
        public ConditionTarget ConditionTarget;
        public BeingConditionEvent BeingConditionEvent;

        public ConditionData(ConditionTarget conditionTarget, BeingConditionEvent beingConditionEvent)
        {
            ConditionTarget = conditionTarget;
            BeingConditionEvent = beingConditionEvent;

            if(ConditionTarget == ConditionTarget.None)
                Log.Error("Wrong ConditionTarget, None");
            if(BeingConditionEvent == BeingConditionEvent.None)
                Log.Error("Wrong BeingConditionEvent, None");
        }
    }
}