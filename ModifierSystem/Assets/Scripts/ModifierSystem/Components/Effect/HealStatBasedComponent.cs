namespace ModifierSystem
{
    public sealed class HealStatBasedComponent : EffectComponent
    {
        public HealStatBasedComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Info = $"HealAct\n";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            acter.Heal(receiver);
        }
    }
}