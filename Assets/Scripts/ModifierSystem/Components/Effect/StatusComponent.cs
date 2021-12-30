using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : IEffectComponent, IConditionEffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StatusComponentStackEffect StackEffect { get; }

        private readonly ITargetComponent _targetComponent;

        public StatusComponent(StatusEffect effect, float duration, ITargetComponent targetComponent,
            StatusComponentStackEffect stackEffect = StatusComponentStackEffect.None)
        {
            StatusEffect = effect;
            Duration = duration;
            _targetComponent = targetComponent;
            StackEffect = stackEffect;
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

        public void Effect(int stacks, double value)
        {
            switch (StackEffect)
            {
                case StatusComponentStackEffect.Effect:
                    Effect();
                    break;
                default:
                    Log.Error($"StackEffectType {StackEffect} unsupported for {GetType()}");
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