using UnitLibrary;

namespace ModifierLibrary
{
    public sealed class StatBurnComponent : EffectComponent, IStackEffectComponent
    {
        private PoolStatType StatType { get; }
        private double Value { get; set; }
        private StatBurnType StatBurnType { get; }
        
        private StackEffectType StackType { get; }

        public StatBurnComponent(PoolStatType statType, double value, StatBurnType statBurnType = StatBurnType.Flat,
            StackEffectType stackType = StackEffectType.None, ConditionCheckData conditionCheckData = null, bool isRevertible = false) :
            base(conditionCheckData, isRevertible)
        {
            StatType = statType;
            Value = value;
            StatBurnType = statBurnType;
            StackType = stackType;
            
            if (StatBurnType == StatBurnType.Percent && Value > 1)
                Value /= 100d;

            switch (statBurnType)
            {
                case StatBurnType.Flat:
                    Info = $"{StatType}Burn: {Value}";
                    break;
                case StatBurnType.Percent:
                    Info = $"{StatType}Burn: {Value*100d}%";
                    break;
                default:
                    Log.Error("Invalid StatBurnType: " + statBurnType, "modifiers");
                    break;
            }
        }
        
        protected override void Effect(Unit receiver, Unit acter)
        {
            receiver.Stats.BurnStat(StatType, Value, StatBurnType);
        }

        public void StackEffect(int stacks, double value)
        {
            if (StackType.HasFlag(StackEffectType.Add))
                Value += value;
            if (StackType.HasFlag(StackEffectType.AddStacksBased))
                Value += value * stacks;
            if (StackType.HasFlag(StackEffectType.Multiply))
                Value *= value;
            if (StackType.HasFlag(StackEffectType.MultiplyStacksBased))
                Value *= value * stacks;

            
            if (StackType.HasFlag(StackEffectType.Effect))
                SimpleEffect();
        }
    }
}