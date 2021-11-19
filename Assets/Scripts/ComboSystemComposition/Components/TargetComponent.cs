using BaseProject;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    public class TargetComponent : Component, IValidatorComponent<Being>, ITargetComponent
    {
        [CanBeNull] public IBeing Target { get; private set; }
        public UnitType UnitType { get; }
        private bool Applier { get; }
        private Being _owner;

        public TargetComponent(UnitType unitType = UnitType.Self, bool applier = false)
        {
            UnitType = unitType;
            Applier = applier;
        }

        public void SetupOwner(Being owner)
        {
            _owner = owner;
        }

        public bool Validate(Being target)
        {
            if (target == null)
            {
                if (UnitType.HasFlag(UnitType.Ground))
                    return true;
                return false;
            }

            //Check if target is self
            if (UnitType.HasFlag(UnitType.Self))
                return _owner == target;

            if (UnitType.HasFlag(UnitType.Ally))
                return target.BaseBeing.UnitType == UnitType.Ally;

            if (UnitType.HasFlag(UnitType.Enemy))
                return target.BaseBeing.UnitType == UnitType.Enemy;

            //TODO rest of flags

            Target = target;
            return true;
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
            Target = target;
        }
    }
}