using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : EffectComponent
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

        public override void Effect()
        {
            //Log.Info($"Status effect {StatusEffect} with duration {Duration}");
            TargetComponent.Target.ChangeStatusEffect(StatusEffect, Duration);
        }
    }
}