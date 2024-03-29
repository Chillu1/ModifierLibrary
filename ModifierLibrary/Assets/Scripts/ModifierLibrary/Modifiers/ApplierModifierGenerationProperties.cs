using UnitLibrary;

namespace ModifierLibrary
{
	public class ApplierModifierGenerationProperties : IModifierGenerationProperties
	{
		public Modifier AppliedModifier { get; }
		public ModifierInfo Info { get; }
		public LegalTarget LegalTarget { get; }


		public bool IsApplier => ApplierType != ApplierType.None;
		public ApplierType ApplierType { get; private set; }
		public bool HasConditionData { get; private set; }
		public ConditionEventTarget ConditionEventTarget { get; private set; }
		public ConditionEvent ConditionEvent { get; private set; }

		public AddModifierParameters AddModifierParameters { get; private set; } = AddModifierParameters.NullStartTarget;

		public (PoolStatType Type, double Value) Cost { get; private set; }

		public float Cooldown { get; private set; } = -1;
		public bool AutomaticAct { get; private set; }
		public double Chance { get; private set; } = -1;

		public ApplierModifierGenerationProperties(Modifier appliedModifier, ModifierInfo info,
			LegalTarget legalTarget = LegalTarget.Units)
		{
			Info = info;
			AppliedModifier = appliedModifier;
			LegalTarget = legalTarget;
		}

		public void SetApplier(ApplierType applierType)
		{
			if (HasConditionData)
				Log.Error("Cannot set applier type together with condition data");

			ApplierType = applierType;
			if (applierType.HasFlag(ApplierType.Attack))
				SetAutomaticAct();
		}

		public void SetCondition(ConditionEventTarget conditionEventTarget, ConditionEvent conditionEvent)
		{
			if (ApplierType != ApplierType.None)
				Log.Error("ApplierType can't be set together with condition?");
			if (conditionEventTarget == ConditionEventTarget.None)
				Log.Error("Wrong ConditionTarget, None");
			if (conditionEvent == ConditionEvent.None)
				Log.Error("Wrong UnitConditionEvent, None");

			HasConditionData = true;
			ConditionEventTarget = conditionEventTarget;
			ConditionEvent = conditionEvent;
			//SetEffectOnApply(); //Always true?
		}

		public void SetCost(PoolStatType costType, double costAmount)
		{
			Cost = (costType, costAmount);
		}

		public void SetCooldown(float cooldown)
		{
			Cooldown = cooldown;
		}

		public void SetAutomaticAct()
		{
			AutomaticAct = true;
		}

		public void SetChance(double chance)
		{
			Chance = chance;
		}

		public void SetAddModifierParameters(AddModifierParameters parameters)
		{
			AddModifierParameters = parameters;
		}
	}
}