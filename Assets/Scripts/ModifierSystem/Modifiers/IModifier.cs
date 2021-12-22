using System;
using BaseProject;

namespace ModifierSystem
{
    public interface IModifier : IEntity<string>, ICloneable
    {
        TargetComponent TargetComponent { get; }
        bool ApplierModifier { get; }
        void Init(ModifierController modifierController);
        void TryApply(Being target);
        void Update(float deltaTime, double statusResistance);
        bool Stack();
        bool Refresh();
        bool ValidatePrototypeSetup();
    }
}