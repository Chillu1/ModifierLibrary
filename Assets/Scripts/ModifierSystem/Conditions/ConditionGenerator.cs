using System;
using BaseProject;

namespace ModifierSystem
{
    [Flags]
    public enum ConditionCheck
    {
        Bool = 0,
        GreaterThan = 1,
        EqualTo = 2,
        SmallerThan = 4,
    }

    public delegate bool BeingCondition(BaseBeing receiver, BaseBeing acter);

    public static class ConditionGenerator
    {
        //public static ConditionGenerator Instance { get; private set; }
        private static readonly BeingCondition healthIsLow = (receiver, acter) => receiver.Stats.Health.IsLow;
        private static readonly BeingCondition healthIsFull = (receiver, acter) => receiver.Stats.Health.IsFull;


        public static BeingCondition GenerateBeingCondition(ConditionBeingStatus status)
        {
            switch (status)
            {
                case ConditionBeingStatus.None:
                    break;
                case ConditionBeingStatus.HealthIsLow:
                    return healthIsLow;
                case ConditionBeingStatus.HealthIsFull:
                    return healthIsFull;
            }

            return delegate { return true; };
        }

        public static BeingCondition GenerateBeingCondition(ConditionCheck check = ConditionCheck.Bool, double value = 0)
        {
            if (check != ConditionCheck.Bool && value != 0)
            {
                //TODO
            }

            return delegate { return true; };
        }
    }
}