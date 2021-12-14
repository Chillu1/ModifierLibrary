namespace ModifierSystem
{
    public class HealComponent : IEffectComponent
    {
        public double Heal { get; private set; }
        private ITargetComponent TargetComponent { get; }

        public HealComponent(double heal, ITargetComponent targetComponent)
        {
            Heal = heal;
            TargetComponent = targetComponent;
        }

        public void Effect()
        {
            TargetComponent.Target.BaseBeing.Heal(Heal);
        }
    }
}