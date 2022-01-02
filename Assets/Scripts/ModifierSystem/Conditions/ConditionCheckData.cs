using BaseProject;

namespace ModifierSystem
{
    public class ConditionCheckData
    {
        public ConditionBeingStatus Status { get; }

        public ComparisonCheck ComparisonCheck { get; }
        public double Value { get; }

        public StatType StatType { get; }
        public string ModifierId { get; }
        public ElementalType ElementalType { get; }
        public bool ElementalIntensityCheck { get; }

        public ConditionCheckData(ConditionBeingStatus status)
        {
            Status = status;
        }
        public ConditionCheckData(string modifierId)
        {
            ModifierId = modifierId;
        }
        public ConditionCheckData(StatType statType, ComparisonCheck comparisonCheck, double value)
        {
            StatType = statType;
            ComparisonCheck = comparisonCheck;
            Value = value;
        }

        public ConditionCheckData(ElementalType elementalType, ComparisonCheck comparisonCheck, double value,
            bool elementalIntensityCheck = true)
        {
            ElementalType = elementalType;
            ComparisonCheck = comparisonCheck;
            Value = value;
            ElementalIntensityCheck = elementalIntensityCheck;
        }
    }
}