using System;
using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public sealed class ModifierPrototypesBase<TModifierType> where TModifierType : class, IModifier, IEntity<string>, ICloneable, IEventCopy<TModifierType>
    {
        private readonly BasePrototypeController<string, TModifierType> _prototypeController;

        public Dictionary<string, TModifierType>.ValueCollection Values => _prototypeController.Values;

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
            var target = new TargetComponent(legalTarget, true);
            var effect = new ApplierComponent(appliedModifier, target);
            var apply = new ApplyComponent(effect);
            modifierApplier.AddComponent(apply);
            modifierApplier.AddComponent(target);
            modifierApplier.FinishSetup();//"No tags", for now?
            AddModifier((TModifierType)(IModifier)modifierApplier);
        }

        [CanBeNull]
        public TModifierType GetItem(string key)
        {
            var modifier = _prototypeController.GetItem(key);
            ValidateModifier(modifier);

            return modifier;
        }

        private bool ValidateModifier(TModifierType modifier)
        {
            if (modifier.TargetComponent.Target != null || modifier.TargetComponent.Owner != null)
            {
                Log.Error("Cloned prototype modifier has a Target or Owner");
                return false;
            }

            return true;
        }
    }
}