using BaseProject;

namespace ModifierSystem
{
    public sealed class HealStatBasedComponent : EffectComponent
    {
        public HealStatBasedComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Info = $"HealAct\n";
        }

        protected override void Effect(BaseBeing receiver, BaseBeing acter)
        {
            BaseBeing.Heal(receiver, acter);
        }
    }
}