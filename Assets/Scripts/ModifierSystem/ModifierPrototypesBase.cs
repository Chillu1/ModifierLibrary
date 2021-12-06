using System;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public sealed class ModifierPrototypesBase<TModifierType> where TModifierType : class, IModifier, IEntity<string>, ICloneable, IEventCopy<TModifierType>
    {
        private readonly BasePrototypeController<string, TModifierType> _prototypeController;

        public ModifierPrototypesBase()
        {
            _prototypeController = new BasePrototypeController<string, TModifierType>();
        }

        public void AddModifier(TModifierType modifier)
        {
            if (!modifier.ValidatePrototypeSetup())
                return;

            if (_prototypeController.ContainsKey(modifier.Id))
            {
                Log.Error("A modifier with id: "+modifier.Id+" already exists", "modifiers");
                return;
            }
            _prototypeController.AddItem(modifier.Id, modifier);
        }

        //Generic non-removable (permanent) applier, for now
        public void SetupModifierApplier(TModifierType appliedModifier, LegalTarget legalTarget = LegalTarget.DefaultOffensive)
        {
            var modifierApplier = new Modifier(appliedModifier.Id+"Applier", true);
            var modifierApplierTarget = new TargetComponent(legalTarget, true);
            var modifierApplierEffect = new ApplierComponent(appliedModifier, modifierApplierTarget);
            var modifierApplierApply = new ApplyComponent(modifierApplierEffect, modifierApplierTarget);
            modifierApplier.AddComponent(modifierApplierApply);
            modifierApplier.AddComponent(modifierApplierTarget);
            AddModifier((TModifierType)(IModifier)modifierApplier);
        }

        [CanBeNull]
        public TModifierType GetItem(string key)
        {
            return _prototypeController.GetItem(key);
        }
    }
}