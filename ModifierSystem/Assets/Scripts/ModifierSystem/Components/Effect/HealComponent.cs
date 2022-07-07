using BaseProject;

namespace ModifierSystem
{
    public sealed class HealComponent : EffectComponent
    {
        private double Heal { get; }

        public HealComponent(double heal, ConditionCheckData conditionCheckData = null, bool isRevertible = false)
            : base(conditionCheckData, isRevertible)
        {
            Heal = heal;

            Info = $"Heal: {Heal}\n";
        }

        protected override void Effect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            receiver.Stats.Health.Heal(Heal);
        }

        protected override void RevertEffect(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            receiver.Stats.Health.Heal(-Heal);
        }
    }
}