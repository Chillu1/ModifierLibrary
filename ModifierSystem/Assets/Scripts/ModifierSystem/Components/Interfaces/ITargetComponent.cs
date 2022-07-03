using System;
using BaseProject;

namespace ModifierSystem
{
    public interface ITargetComponent
    {
        ConditionEventTarget ConditionEventTarget { get; }
        Being Target { get; }
        Being Owner { get; }
        Being ApplierOwner { get; }
        bool SetTarget(Being target);
        void HandleTarget(BaseBeing receiver, BaseBeing acter, BaseBeingEvent effect);
    }
}