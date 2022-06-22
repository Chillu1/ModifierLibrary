using BaseProject;

namespace ModifierSystem
{
    public class StatusComponent : EffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StatusComponentStackEffect StackEffectType { get; }

        public StatusComponent(StatusEffect statusEffect, float duration,
            StatusComponentStackEffect stackEffectType = StatusComponentStackEffect.None, ConditionCheckData conditionCheckData = null) :
            base(conditionCheckData)
        {
            StatusEffect = statusEffect;
            Duration = duration;
            StackEffectType = stackEffectType;
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            receiver.StatusEffects.ChangeStatusEffect(StatusEffect, Duration);
        }

        public void StackEffect(int stacks, double value)
        {
            if (StackEffectType.HasFlag(StatusComponentStackEffect.Effect))
            {
                SimpleEffect();
            }
            else
            {
                Log.Error($"StackEffectType {StackEffectType} unsupported for {GetType()}");
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