using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class TargetComponent : Component, IValidatorComponent<IBeing>, ITargetComponent
    {
        [CanBeNull] public IBeing Target { get; private set; }
        public LegalTarget LegalTarget { get; }
        private bool Applier { get; }
        private IBeing _owner;

        public TargetComponent(LegalTarget legalTarget = LegalTarget.Self, bool applier = false)
        {
            if(legalTarget == LegalTarget.None)
                Log.Error("Illegal target `None`", "modifiers");

            LegalTarget = legalTarget;
            Applier = applier;
        }

        public void SetupOwner(IBeing owner)
        {
            _owner = owner;
        }

        public bool Validate(IBeing target)
        {
            if (target == null)
                return LegalTarget.HasFlag(LegalTarget.Ground);

            if (target.BaseBeing.UnitType == UnitType.None)
                Log.Error("Illegal UnitType on: " + target.BaseBeing.Id, "modifiers");

            //Check if target is self
            if (LegalTarget.HasFlag(LegalTarget.Self) && _owner == target)
                return true;

            if (LegalTarget.HasFlag(LegalTarget.Ally) && target.BaseBeing.UnitType == UnitType.Ally)
                return true;

            if (LegalTarget.HasFlag(LegalTarget.Enemy) && target.BaseBeing.UnitType == UnitType.Enemy)
                return true;

            return false;
        }

        [CanBeNull]
        public IBeing GetTarget()
        {
            return Target;
        }

        public IBeing GetOwner()
        {
            return _owner;
        }

        public void SetTarget(IBeing target)
        {
            if (!Applier && Target != null)
            {
                Log.Error("Already has a target", "modifiers");
                return;
            }

            if (!Validate(target))
            {
                Log.Error("Target is not valid, id: "+target?.BaseBeing.Id, "modifiers");
                return;
            }

            Target = target;
        }
    }
}