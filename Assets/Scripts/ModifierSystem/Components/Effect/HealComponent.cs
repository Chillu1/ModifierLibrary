namespace ModifierSystem
{
    public class HealComponent : EffectComponent
    {
        public double Heal { get; private set; }
        private ITargetComponent TargetComponent { get; }

        public HealComponent(double heal, ITargetComponent targetComponent)
        {
            Heal = heal;
            TargetComponent = targetComponent;
        }

        public override void Effect()
        {
            TargetComponent.Target.Stats.Health.Heal(Heal);
        }
    }
}