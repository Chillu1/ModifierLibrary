using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : EffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StatusComponentStackEffect StackEffectType { get; }

        public StatusComponent(StatusEffect effect, float duration,
            StatusComponentStackEffect stackEffectType = StatusComponentStackEffect.None) : base()
        {
            StatusEffect = effect;
            Duration = duration;
            StackEffectType = stackEffectType;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.StatusEffects.ChangeStatusEffect(StatusEffect, Duration);
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