using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : IEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }

        private ITargetComponent TargetComponent { get; }

        public StatusComponent(StatusEffect effect, float duration, ITargetComponent targetComponent)
        {
            StatusEffect = effect;
            Duration = duration;
            TargetComponent = targetComponent;
        }

        public void Effect()
        {
            TargetComponent.Target.ChangeStatusEffect(StatusEffect, Duration);
        }
    }
}