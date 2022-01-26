using BaseProject;

namespace ModifierSystem
{
    public class HealComponent : EffectComponent
    {
        public double Heal { get; private set; }

        public HealComponent(double heal, ConditionCheckData conditionCheckData = null) : base(conditionCheckData)
        {
            Heal = heal;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Stats.Health.Heal(Heal);
        }
    }
}