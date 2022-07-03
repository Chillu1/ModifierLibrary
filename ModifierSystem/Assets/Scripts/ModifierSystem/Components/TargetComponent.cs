using System;
using BaseProject;
using Force.DeepCloner;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class TargetComponent : Component, IValidatorComponent<Being>, ITargetComponent, ICloneable
    {
        public ConditionEventTarget ConditionEventTarget { get; private set; }
        [CanBeNull] public Being Target { get; private set; }
        public LegalTarget LegalTarget { get; }
        private bool Applier { get; }

        /// <summary>
        ///     Current owner of this modifier
        /// </summary>
        public Being Owner { get; private set; }
        /// <summary>
        ///     Being that applied this modifier to current being
        /// </summary>
        public Being ApplierOwner { get; private set; }

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

        public void SetupOwner(Being owner)
        {
            Owner = owner;
        }

        public void SetupApplierOwner(Being owner)
        {
            ApplierOwner = owner;
        }

        public bool ValidateTarget(Being target)
        {
            if (target == null)
                return LegalTarget.HasFlag(LegalTarget.Ground);

            if (target.UnitType == UnitType.None)
                Log.Error("Illegal UnitType on: " + target.Id, "modifiers");

            //Check if target is self
            if (LegalTarget.HasFlag(LegalTarget.Self) && Owner == target)
                return true;

            if (LegalTarget.HasFlag(LegalTarget.Ally) && target.UnitType == UnitType.Ally)
                return true;

            if (LegalTarget.HasFlag(LegalTarget.Enemy) && target.UnitType == UnitType.Enemy)
                return true;

            Log.Error("LegalTargets are: " + LegalTarget + ". But our unit type is: " + target.UnitType);
            return false;
        }

        public bool SetTarget(Being target)
        {
            if (!Applier && Target != null)
            {
                Log.Error("Already has a target", "modifiers");
                return false;
            }

            if (!ValidateTarget(target))
            {
                Log.Error("Target is not valid, id: " + target?.Id, "modifiers");
                return false;
            }

            Target = target;
            return true;
        }

        public void HandleTarget(BaseBeing receiver, BaseBeing acter, BaseBeingEvent effect)
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
    }
}