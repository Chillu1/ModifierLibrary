using System;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public interface IModifier : IEntity<string>, ICloneable, IEventCopy<IModifier>
    {
        TargetComponent TargetComponent { get; }
        StatusTag[] StatusTags { get; }
        bool ApplierModifier { get; }
        bool ToRemove { get; }
        void Init();
        void TryApply(Being target);
        void Update(float deltaTime, StatusResistances ownerStatusResistances);
        bool Stack();
        bool Refresh();
        void SetForRemoval();
        bool ValidatePrototypeSetup();
        void FinishSetup([CanBeNull] DamageData[] damageData = null);
    }
}