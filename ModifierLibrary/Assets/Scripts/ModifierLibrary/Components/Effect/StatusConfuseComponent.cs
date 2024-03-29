using UnitLibrary;

namespace ModifierLibrary
{
	public sealed class StatusConfuseComponent : EffectComponent
	{
		private double Duration { get; }
		private TargetType TargetType { get; }
		private ConfuseType ConfuseType { get; }

		public StatusConfuseComponent(double duration, TargetType targetType, ConfuseType confuseType,
			ConditionCheckData conditionCheckData = null, bool isRevertible = false) : base(conditionCheckData, isRevertible)
		{
			Duration = duration;
			TargetType = targetType;
			ConfuseType = confuseType;

			Info = $"StatusEffect, Confuse: {duration}s, {TargetType} target, confuse type: {ConfuseType}";
		}

		protected override void Effect(Unit receiver, Unit acter)
		{
			receiver.StatusEffects.ChangeConfuseEffect(Duration, TargetType, ConfuseType);
		}

		protected override void RevertEffect(Unit receiver, Unit acter)
		{
			receiver.StatusEffects.DecreaseConfuseEffect(Duration, TargetType);
		}
	}
}