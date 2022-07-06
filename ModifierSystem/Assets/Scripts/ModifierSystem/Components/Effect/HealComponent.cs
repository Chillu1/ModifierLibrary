using BaseProject;

namespace ModifierSystem
{
    public class HealComponent : EffectComponent
    {
        public double Heal { get; private set; }

        public HealComponent(double heal, ConditionCheckData conditionCheckData = null, bool isRevertible = false) : base(conditionCheckData,
            isRevertible)
        {
            Heal = heal;

            Info = $"Heal: {Heal}\n";
        }

        protected override void Effect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Stats.Health.Heal(Heal);
        }

        protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.Stats.Health.Heal(-Heal);
        }
    }
}