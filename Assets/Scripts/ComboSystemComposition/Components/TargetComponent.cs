using System;
using BaseProject;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    public class TargetComponent : Component, IValidatorComponent<Being>, ITargetComponent
    {
        [CanBeNull] public Being Target { get; private set; }
        public TargetType TargetType { get; }
        private Being _owner;

        public TargetComponent(TargetType targetType)
        {
            TargetType = targetType;
        }

        public void SetupOwner(Being owner)
        {
            _owner = owner;
        }

        public bool Validate(Being target)
        {
            if (target == null)
            {
                if (TargetType.HasFlag(TargetType.Ground))
                    return true;
                return false;
            }

            //Check if target is self
            if (TargetType.HasFlag(TargetType.Self))
                return _owner == target;

            if (TargetType.HasFlag(TargetType.Ally))
                return target.TargetType == TargetType.Ally;

            if (TargetType.HasFlag(TargetType.Enemy))
                return target.TargetType == TargetType.Enemy;

            //TODO rest of flags

            Target = target;
            return true;
        }

        [CanBeNull]
        public Being GetTarget()
        {
            return Target;
        }

        public void SetTarget(Being target)
        {
            if (Target != null)
            {
                Log.Error("Already has a target");
                return;
            }
            Target = target;
        }
    }

    [Flags]
    public enum TargetType
    {
        None = 0,
        Self = 1,
        Ally = 2,
        Enemy = 4,
        Ground = 8,

        DefaultFriendly = Self | Ally,
        DefaultOffensive = Enemy,
        DefaultFriendlyEnemy = Self | Enemy,
        Beings = Self | Ally | Enemy,
        All = Self | Ally | Enemy | Ground
    }
}