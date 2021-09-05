using BaseProject;

namespace ComboSystem
{
    public class StatChangeDurationModifierData : DurationModifierData
    {
        public StatType StatType { get; protected set; }
        public float Value { get; protected set; }

        public StatChangeDurationModifierData(StatType statType, float value, float duration) : base(duration)
        {
            StatType = statType;
            Value = value;
        }
    }
}