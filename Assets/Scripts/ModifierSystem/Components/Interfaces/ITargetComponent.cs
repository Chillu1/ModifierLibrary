using System;
using BaseProject;

namespace ModifierSystem
{
    public interface ITargetComponent
    {
        ConditionTarget ConditionTarget { get; }
        Being Target { get; }
        Being Owner { get; }
        bool SetTarget(Being target);
        void HandleTarget(BaseBeing receiver, BaseBeing acter, Action<BaseBeing, BaseBeing> effect);
    }
}