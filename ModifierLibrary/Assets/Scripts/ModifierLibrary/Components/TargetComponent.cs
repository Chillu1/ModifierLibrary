using System;
using UnitLibrary;
using Force.DeepCloner;
using JetBrains.Annotations;

namespace ModifierLibrary
{
	public class TargetComponent : Component, IValidatorComponent<Unit>, ITargetComponent, ICloneable
	{
		public ConditionEventTarget ConditionEventTarget { get; private set; }
		[CanBeNull] public Unit Target { get; private set; }
		public LegalTarget LegalTarget { get; }
		private bool Applier { get; }

		/// <summary>
		///     Current owner of this modifier
		/// </summary>
		public Unit Owner { get; private set; }

		/// <summary>
		///     Unit that applied this modifier to current unit
		/// </summary>
		[CanBeNull]
		public Unit ApplierOwner { get; private set; }

		public TargetComponent(LegalTarget legalTarget = LegalTarget.Self, bool applier = false)
		{
			if (legalTarget == LegalTarget.None)
				Log.Error("Illegal target `None`", "modifiers");

			LegalTarget = legalTarget;
			Applier = applier;
		}

		public TargetComponent(LegalTarget legalTarget, ConditionEventTarget conditionEventTarget, bool applier = false)
		{
			if (legalTarget == LegalTarget.None)
				Log.Error("Illegal target `None`", "modifiers");
			if (conditionEventTarget == ConditionEventTarget.None)
				Log.Error("Illegal conditionalTarget `None`", "modifiers");

			LegalTarget = legalTarget;
			ConditionEventTarget = conditionEventTarget;
			Applier = applier;
		}

		public void SetupOwner(Unit owner)
		{
			Owner = owner;
		}

		public void SetupApplierOwner(Unit owner)
		{
			ApplierOwner = owner;
		}

		public bool ValidateTarget(Unit target)
		{
			if (target == null)
				return LegalTarget.HasFlag(LegalTarget.Ground);

			//Log.Info($"LegalTarget: {LegalTarget}. TargetType: {target.UnitType}. OwnerType: {Owner.UnitType}", "modifiers");

			if (target.UnitType == UnitType.None)
				Log.Error("Illegal UnitType on: " + target.Id, "modifiers");

			if (Owner == null)
			{
				Log.Warning("Owner is null, if this isn't an unit test. This is bad", "modifiers");
				return true;
			}

			//Check if target is self
			if (LegalTarget.HasFlag(LegalTarget.Self) && Owner == target)
				return true;

			//If target is self, and we are not allowed to target self
			if (!LegalTarget.HasFlag(LegalTarget.Self) && Owner == target)
			{
				//TODO Won't be an error with the new targeting self/allies mechanic, still should be checked & logged in case a modifier has been setup wrong
				Log.Error("Targeting ourselves, while legalTarget doesn't have self as a valid target. Target: " + target.Id, "modifiers");
				return false;
			}

			if (LegalTarget.HasFlag(LegalTarget.Same) && target.UnitType == Owner.UnitType)
				return true;

			if (LegalTarget.HasFlag(LegalTarget.Opposite) && target.UnitType != Owner.UnitType)
				return true;

			Log.Error("LegalTargets are: " + LegalTarget + ". But our unit type is: " + target.UnitType);
			return false;
		}

		public bool SetTarget(Unit target)
		{
			if (!Applier && Target != null)
			{
				Log.Error("Already has a target", "modifiers");
				return false;
			}

			if (!ValidateTarget(target))
			{
				//Log.Error("Target is not valid, id: " + target?.Id, "modifiers");
				return false;
			}

			Target = target;
			return true;
		}

		public void HandleTarget(Unit receiver, Unit acter, UnitEvent effect)
		{
			switch (ConditionEventTarget)
			{
				case ConditionEventTarget.SelfActer:
					effect(receiver, acter);
					break;
				case ConditionEventTarget.ActerSelf:
					effect(acter, receiver);
					break;
				case ConditionEventTarget.SelfSelf:
					effect(receiver, receiver);
					break;
				case ConditionEventTarget.ActerActer:
					effect(acter, acter);
					break;
			}
		}

		public object Clone()
		{
			return this.DeepClone();
		}

		public override string ToString()
		{
			return $"LegalTarget: {LegalTarget}.Target: {Target?.Id}. Owner: {Owner?.Id}. ApplierOwner: {ApplierOwner?.Id}";
		}
	}
}