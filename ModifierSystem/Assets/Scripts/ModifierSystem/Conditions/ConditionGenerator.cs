using BaseProject;

namespace ModifierSystem
{
    public delegate bool BeingCondition(BaseBeing receiver, BaseBeing acter);

    public static class ConditionGenerator
    {
        private static readonly BeingCondition healthIsLow = (receiver, acter) => receiver.Stats.Health.IsLow;
        private static readonly BeingCondition healthIsHalf = (receiver, acter) => !receiver.Stats.Health.HasPercent(0.5);
        private static readonly BeingCondition healthIsFull = (receiver, acter) => receiver.Stats.Health.IsFull;

        public static BeingCondition GenerateBeingCondition(ConditionCheckData checkData)
        {
            if (checkData.Status != ConditionBeingStatus.None)
            {
                switch (checkData.Status)
                {
                    case ConditionBeingStatus.HealthIsLow:
                        return healthIsLow;
                    case ConditionBeingStatus.HealthIsHalf:
                        return healthIsHalf;
                    case ConditionBeingStatus.HealthIsFull:
                        return healthIsFull;
                }
            }

            if(checkData.StatType != StatType.None)
                return (receiver, acter)
                    => receiver.Stats.HasStat(checkData.StatType, checkData.Value, checkData.ComparisonCheck);

            if(!string.IsNullOrEmpty(checkData.ModifierId))
                return (receiver, acter) => ((Being)receiver).ContainsModifier(checkData.ModifierId);

            if(checkData.ElementalType != ElementalType.None)
            {
                return checkData.ElementalIntensityCheck
                    ? (receiver, acter) => receiver.ElementController.HasIntensity(checkData.ElementalType, checkData.Value)
                    : (receiver, acter) => receiver.ElementController.HasValue(checkData.ElementalType, checkData.Value);
            }

            return delegate { return true; };
        }
    }
}