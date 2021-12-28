using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : IEffectComponent, IConditionEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }

        private readonly ITargetComponent _targetComponent;

        public StatusComponent(StatusEffect effect, float duration, ITargetComponent targetComponent)
        {
            StatusEffect = effect;
            Duration = duration;
            _targetComponent = targetComponent;
        }

        public void Effect()
        {
            //Log.Info($"Status effect {StatusEffect} with duration {Duration}");
            _targetComponent.Target.ChangeStatusEffect(StatusEffect, Duration);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            _targetComponent.HandleTarget(owner, acter,
                (receiverLocal, acterLocal) => receiverLocal.ChangeStatusEffect(StatusEffect, Duration));
        }
    }
}