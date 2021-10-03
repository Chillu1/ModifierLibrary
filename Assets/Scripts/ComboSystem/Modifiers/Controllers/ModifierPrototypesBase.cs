using System.Globalization;
using BaseProject;
using JetBrains.Annotations;

namespace ComboSystem
{
    public abstract class ModifierPrototypesBase<TModifierType> : BasePrototypeController<string, TModifierType>
        where TModifierType : Modifier, IEventCopy<TModifierType>
    {
        protected abstract void SetupModifierPrototypes();

        protected void SetupModifier(TModifierType modifier)
        {
            if (prototypes.ContainsKey(modifier.Id))
            {
                Log.Error("A modifier with id: "+modifier.Id+" already exists", "modifiers");
                return;
            }
            prototypes.Add(modifier.Id, modifier);
        }

        [CanBeNull]
        public Modifier<TDataType> GetModifier<TDataType>(string modifierName)
        {
            return GetItem(modifierName) as Modifier<TDataType>;
        }
        
        [CanBeNull]
        public ModifierApplier<ModifierApplierData> GetModifierApplier(string modifierName)
        {
            if (!modifierName.EndsWith("Applier", true, CultureInfo.InvariantCulture))
            {
                if (modifierName.EndsWith("Buff", true, CultureInfo.InvariantCulture) ||
                    modifierName.EndsWith("Debuff", true, CultureInfo.InvariantCulture))
                    Log.Error(
                        "Getting a modifier applier that either has a wrong name, or that isn't a modifierApplier. Name: " + modifierName +
                        ". You should probably remove Buff/Debuff, and have 'Applier'", "modifiers");
                else
                    Log.Error(
                        "Getting a modifier applier that either has a wrong name, or that isn't a modifierApplier. Name: " + modifierName +
                        ". Did you mean: " + modifierName + "Applier", "modifiers");
            }

            return GetModifier<ModifierApplierData>(modifierName) as ModifierApplier<ModifierApplierData>;
        }
    }
}