using BaseProject;

namespace ModifierSystem
{
    public sealed class StatusComponent : EffectComponent, IStackEffectComponent
    {
        private StatusEffect StatusEffect { get; }
        private float Duration { get; }
        private StackEffectType StackType { get; }

        public StatusComponent(StatusEffect statusEffect, float duration,
            StackEffectType stackType = StackEffectType.None, ConditionCheckData conditionCheckData = null,
            bool isRevertible = false) : base(conditionCheckData, isRevertible)
        {
            StatusEffect = statusEffect;
            Duration = duration;
            StackType = stackType;

            Info = $"Status: {StatusEffect}, duration: {Duration}\n";
        }

        protected override void Effect(BaseBeing receiver, BaseBeing acter)
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
            if (StackType.HasFlag(StackEffectType.Effect))
                SimpleEffect();
        }

        public enum StackEffectType
        {
            None = 0,
            Effect,
            //TODO Increase duration
        }
    }
}