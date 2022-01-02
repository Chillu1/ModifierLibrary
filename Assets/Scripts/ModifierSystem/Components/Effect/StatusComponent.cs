using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : IEffectComponent, IConditionEffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StatusComponentStackEffect StackEffectType { get; }

        private readonly ITargetComponent _targetComponent;

        public StatusComponent(StatusEffect effect, float duration, ITargetComponent targetComponent,
            StatusComponentStackEffect stackEffectType = StatusComponentStackEffect.None)
        {
            StatusEffect = effect;
            Duration = duration;
            _targetComponent = targetComponent;
            StackEffectType = stackEffectType;
        }

        public void SimpleEffect()
        {
            //Log.Info($"Status effect {StatusEffect} with duration {Duration}");
            _targetComponent.Target.StatusEffects.ChangeStatusEffect(StatusEffect, Duration);
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing acter)
        {
            _targetComponent.HandleTarget(receiver, acter,
                (receiverLocal, acterLocal) => receiverLocal.StatusEffects.ChangeStatusEffect(StatusEffect, Duration));
        }

        public void StackEffect(int stacks, double value)
        {
            switch (StackEffectType)
            {
                case StatusComponentStackEffect.Effect:
                    SimpleEffect();
                    break;
                default:
                    Log.Error($"StackEffectType {StackEffectType} unsupported for {GetType()}");
                    return;
            }
        }

        public enum StatusComponentStackEffect
        {
            None = 0,
            Effect,
            //TODO Increase duration
        }
    }
}