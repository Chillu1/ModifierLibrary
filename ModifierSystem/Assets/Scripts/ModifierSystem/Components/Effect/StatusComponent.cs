using BaseProject;

namespace ModifierSystem
{
    public sealed class StatusComponent : EffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StatusComponentStackEffect StackEffectType { get; }

        public StatusComponent(StatusEffect statusEffect, float duration,
            StatusComponentStackEffect stackEffectType = StatusComponentStackEffect.None, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            StatusEffect = statusEffect;
            Duration = duration;
            StackEffectType = stackEffectType;

            Info = $"Status: {StatusEffect}, duration: {Duration}\n";
        }

        protected override void ActualEffect(BaseBeing receiver, BaseBeing acter)
        {
            if (StatusEffect == StatusEffect.Taunt)
            {
                BaseBeing tauntTarget = receiver == acter ? ApplierOwner : acter;
                receiver.StatusEffects.ChangeTauntEffect(Duration, tauntTarget);
            }
            else
                receiver.StatusEffects.ChangeStatusEffect(StatusEffect, Duration);
        }

        protected override void RevertEffect(BaseBeing receiver, BaseBeing acter)
        {
            if (StatusEffect == StatusEffect.Taunt)
                receiver.StatusEffects.DecreaseTauntEffect(Duration);
            else
                receiver.StatusEffects.DecreaseStatusEffect(StatusEffect, Duration);
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