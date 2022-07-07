using BaseProject;

namespace ModifierSystem
{
    public sealed class HealComponent : EffectComponent
    {
        private Properties EffectProperties { get; }
        private double Heal { get; }

        public HealComponent(Properties effectProperties, IBaseEffectProperties baseProperties = null) : base(baseProperties)
        {
            EffectProperties = effectProperties;
        }

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

        public struct Properties : IEffectProperties
        {
            public double Heal { get; }

            public Properties(double heal)
            {
                Heal = heal;
            }
        }
    }
}