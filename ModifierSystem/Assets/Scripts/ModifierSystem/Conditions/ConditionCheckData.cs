using BaseProject;

namespace ModifierSystem
{
    public class ConditionCheckData
    {
        public ConditionBeingStatus Status { get; } = ConditionBeingStatus.None;

        public ComparisonCheck ComparisonCheck { get; } = ComparisonCheck.None;
        public double Value { get; }

        public StatType StatType { get; }
        public string ModifierId { get; }
        public ElementType ElementType { get; } = ElementType.None;
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

        public ConditionCheckData(ElementType elementType, ComparisonCheck comparisonCheck, double value,
            bool elementalIntensityCheck = true)
        {
            ElementType = elementType;
            ComparisonCheck = comparisonCheck;
            Value = value;
            ElementalIntensityCheck = elementalIntensityCheck;
        }
    }
}