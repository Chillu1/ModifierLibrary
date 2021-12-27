using System;
using BaseProject;

namespace ModifierSystem
{
    public interface IModifier : IEntity<string>, ICloneable
    {
        TargetComponent TargetComponent { get; }
        bool ApplierModifier { get; }
        bool ToRemove { get; }
        void Init();
        void TryApply(Being target);
        void Update(float deltaTime, StatusResistances ownerStatusResistances);
        bool Stack();
        bool Refresh();
        void SetForRemoval();
        bool ValidatePrototypeSetup();
    }
}