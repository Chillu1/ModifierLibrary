using BaseProject;

namespace ComboSystem
{
    public class ResistanceChangeDurationModifierData : DurationModifierData
    {
        public DamageType DamageType { get; protected set; }
        public double Value { get; protected set; }
        public double Multiplier { get; protected set; }

        public ResistanceChangeDurationModifierData(float duration, DamageType damageType, double value = 0d, double multiplier = 0d) :
            base(duration)
        {
            DamageType = damageType;
            Value = value;
            Multiplier = multiplier;
        }
    }
}