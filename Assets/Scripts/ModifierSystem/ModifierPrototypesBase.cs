using System;
using BaseProject;

namespace ModifierSystem
{
    public abstract class ModifierPrototypesBase<TModifierType> : BasePrototypeController<string, TModifierType>
        where TModifierType : Modifier, IEventCopy<TModifierType>, ICloneable
    {
        protected abstract void SetupModifierPrototypes();

        protected void AddModifier(TModifierType modifier)
        {
            if (!modifier.ValidatePrototypeSetup())
                return;

            if (prototypes.ContainsKey(modifier.Id))
            {
                Log.Error("A modifier with id: "+modifier.Id+" already exists", "modifiers");
                return;
            }
            prototypes.Add(modifier.Id, modifier);
        }

        //Generic non-removable applier, for now
        protected void SetupModifierApplier(TModifierType appliedModifier, UnitType unitType = UnitType.DefaultOffensive)
        {
            var modifierApplier = new Modifier(appliedModifier.Id+"Applier", true);
            var modifierApplierTarget = new TargetComponent(unitType, true);
            var modifierApplierEffect = new ApplierComponent(appliedModifier, modifierApplierTarget);
            var modifierApplierApply = new ApplyComponent(modifierApplierEffect, modifierApplierTarget);
            modifierApplier.AddComponent(modifierApplierApply);
            modifierApplier.AddComponent(modifierApplierTarget);
            AddModifier((TModifierType)modifierApplier);
        }
    }
}