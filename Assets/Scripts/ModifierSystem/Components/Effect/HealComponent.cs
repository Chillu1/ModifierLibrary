using BaseProject;

namespace ModifierSystem
{
    public class HealComponent : IEffectComponent, IConditionEffectComponent
    {
        public double Heal { get; private set; }

        private readonly ITargetComponent _targetComponent;

        public HealComponent(double heal, ITargetComponent targetComponent)
        {
            Heal = heal;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            _targetComponent.Target.Stats.Health.Heal(Heal);
        }

        public void Effect(BaseBeing owner, BaseBeing healer)
        {
            _targetComponent.HandleTarget(owner, healer,
                (receiverLocal, acterLocal) => receiverLocal.Heal(Heal, acterLocal));
        }
    }
}