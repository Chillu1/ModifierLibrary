using BaseProject;

namespace ComboSystem
{
    public class StatChangeModifierData
    {
        public StatType StatType { get; protected set; }
        public float Value { get; protected set; }

        public StatChangeModifierData(StatType statType, float value)
        {
            StatType = statType;
            Value = value;
        }
    }
}