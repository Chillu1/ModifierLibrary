using UnitLibrary;

namespace ModifierLibrary
{
    public delegate bool UnitCondition(Unit receiver, Unit acter);

    public static class ConditionGenerator
    {
        private static readonly UnitCondition healthIsLow = (receiver, acter) => receiver.Stats.Health.IsLow;
        private static readonly UnitCondition healthIsHalf = (receiver, acter) => !receiver.Stats.Health.HasPercent(0.5);
        private static readonly UnitCondition healthIsFull = (receiver, acter) => receiver.Stats.Health.IsFull;

        public static UnitCondition GenerateUnitCondition(ConditionCheckData checkData)
        {
            if (checkData.Status != ConditionUnitStatus.None)
            {
                switch (checkData.Status)
                {
                    case ConditionUnitStatus.HealthIsLow:
                        return healthIsLow;
                    case ConditionUnitStatus.HealthIsHalf:
                        return healthIsHalf;
                    case ConditionUnitStatus.HealthIsFull:
                        return healthIsFull;
                }
            }

            if(checkData.StatType != StatType.None)
                return (receiver, acter)
                    => receiver.Stats.HasStat(checkData.StatType, checkData.Value, checkData.ComparisonCheck);

            if(!string.IsNullOrEmpty(checkData.ModifierId))
                return (receiver, acter) => receiver.ContainsModifier(checkData.ModifierId);

            if(checkData.ElementType != ElementType.None)
            {
                return checkData.ElementalIntensityCheck
                    ? (receiver, acter) => receiver.ElementController.HasIntensity(checkData.ElementType, checkData.Value)
                    : (receiver, acter) => receiver.ElementController.HasValue(checkData.ElementType, checkData.Value);
            }

            return delegate { return true; };
        }
    }
}