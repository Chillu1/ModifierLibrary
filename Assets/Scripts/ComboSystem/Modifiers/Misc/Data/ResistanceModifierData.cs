using BaseProject;

namespace ComboSystem
{
    public class ResistanceModifierData
    {
        public DamageType DamageType { get; protected set; }
        public double Value { get; protected set; }
        public double Multiplier { get; protected set; }

        public ResistanceModifierData(DamageType damageType, double value = 0d, double multiplier = 0d)
        {
            DamageType = damageType;
            Value = value;
            Multiplier = multiplier;
        }
    }
}