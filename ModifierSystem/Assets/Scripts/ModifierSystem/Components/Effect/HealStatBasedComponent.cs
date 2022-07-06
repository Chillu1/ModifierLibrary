using BaseProject;

namespace ModifierSystem
{
    public sealed class HealStatBasedComponent : EffectComponent
    {
        public HealStatBasedComponent(ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Info = $"HealAct\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            BaseProject.Unit.Heal(receiver, acter);
        }
    }
}