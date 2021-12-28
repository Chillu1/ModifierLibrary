using System;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class TargetComponent : Component, IValidatorComponent<Being>, ITargetComponent, ICloneable
    {
        public ConditionTarget ConditionTarget { get; private set; }
        [CanBeNull] public Being Target { get; private set; }
        public LegalTarget LegalTarget { get; }
        private bool Applier { get; }

        public Being Owner { get; private set; }

        public TargetComponent(LegalTarget legalTarget = LegalTarget.Self, bool applier = false)
        {
            if(legalTarget == LegalTarget.None)
                Log.Error("Illegal target `None`", "modifiers");

            LegalTarget = legalTarget;
            Applier = applier;
        }

        public TargetComponent(LegalTarget legalTarget, ConditionTarget conditionTarget, bool applier = false)
        {
            if(legalTarget == LegalTarget.None)
                Log.Error("Illegal target `None`", "modifiers");
            if(conditionTarget == ConditionTarget.None)
                Log.Error("Illegal conditionalTarget `None`", "modifiers");

            LegalTarget = legalTarget;
            ConditionTarget= conditionTarget;
            Applier = applier;
        }

        public void SetupOwner(Being owner)
        {
            Owner = owner;
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
                Log.Error("Target is not valid, id: "+target?.Id, "modifiers");
                return false;
            }

            Target = target;
            return true;
        }

        public void HandleTarget(BaseBeing receiver, BaseBeing acter, Action<BaseBeing, BaseBeing> effect)
        {
            switch (ConditionTarget)
            {
                case ConditionTarget.Self:
                    effect(receiver, acter);
                    break;
                case ConditionTarget.Acter:
                    effect(acter, receiver);
                    break;
            }
        }

        public object Clone()
        {
            return this.Copy();
        }
    }

    public enum ConditionTarget
    {
        None = 0,
        /// <summary>
        ///     Owner
        /// </summary>
        Self = 1,
        Acter = 2,
    }
}