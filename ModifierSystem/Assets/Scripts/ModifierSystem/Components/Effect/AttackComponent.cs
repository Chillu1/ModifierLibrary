namespace ModifierSystem
{
    public sealed class AttackComponent : EffectComponent
    {
        public AttackComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Info = "Attack\n";
        }

        protected override void Effect(Unit receiver, Unit acter)
        {
            acter.Attack(receiver); //TODO Not sure about this
        }
    }
}