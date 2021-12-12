using System;
using BaseProject;

namespace ModifierSystem
{
    public interface IModifier : IEntity<string>, ICloneable
    {
        TargetComponent TargetComponent { get; }
        bool ApplierModifier { get; }
        void Init();
        void TryApply(Being target);
        void Update(float deltaTime);
        bool Stack();
        bool Refresh();
        bool ValidatePrototypeSetup();
    }
}