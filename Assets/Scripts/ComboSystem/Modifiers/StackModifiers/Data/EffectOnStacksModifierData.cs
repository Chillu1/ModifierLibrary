namespace ComboSystem
{
    public class EffectOnStacksModifierData
    {
        public int AmountOfStacksForEffect { get; protected set; }

        public EffectOnStacksModifierData(int amountOfStacksForEffect)
        {
            AmountOfStacksForEffect = amountOfStacksForEffect;
        }
    }

    public class StatChangeStacksModifierData : EffectOnStacksModifierData
    {
        public StatType StatType { get; protected set; }

        public float Value { get; protected set; }

        public StatChangeStacksModifierData(StatType statType, float value, int amountOfStacksForEffect) : base(amountOfStacksForEffect)
        {
            StatType = statType;
            Value = value;
        }
    }
}