namespace ComboSystem
{
    public class EffectOnStacksDurationModifierData : DurationModifierData
    {
        public int AmountOfStacksForEffect { get; protected set; }

        public EffectOnStacksDurationModifierData(int amountOfStacksForEffect, float duration) : base(duration)
        {
            AmountOfStacksForEffect = amountOfStacksForEffect;
        }
    }
}