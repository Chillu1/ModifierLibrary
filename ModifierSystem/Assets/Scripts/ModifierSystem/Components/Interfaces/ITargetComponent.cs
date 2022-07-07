using System;
using BaseProject;

namespace ModifierSystem
{
    public interface ITargetComponent
    {
        ConditionEventTarget ConditionEventTarget { get; }
        Unit Target { get; }
        Unit Owner { get; }
        Unit ApplierOwner { get; }
        bool SetTarget(Unit target);
        void HandleTarget(Unit receiver, Unit acter, UnitEvent effect);
    }
}