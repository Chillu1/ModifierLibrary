using BaseProject;

namespace ModifierSystem
{
    public class HealStatBasedComponent : EffectComponent
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