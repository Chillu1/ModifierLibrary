using System.Collections.Generic;
using BaseProject;
using JetBrains.Annotations;

namespace ComboSystem
{
    public abstract class ModifierPrototypesBase<TModifierType> where TModifierType : Modifier
    {
        protected readonly Dictionary<string, TModifierType> modifierPrototypes;

        protected ModifierPrototypesBase()
        {
            modifierPrototypes = new Dictionary<string, TModifierType>();
        }

        protected abstract void SetupModifierPrototypes();

        protected void SetupModifier(TModifierType modifier)
        {
            if (modifierPrototypes.ContainsKey(modifier.Id))
            {
                Log.Error("A modifier with id: "+modifier.Id+" already exists");
                return;
            }
            modifierPrototypes.Add(modifier.Id, modifier);
        }

        [CanBeNull]
        public TModifierType GetModifier(string modifierName)
        {
            if (modifierPrototypes.TryGetValue(modifierName, out TModifierType modifier))
            {
                modifier = (TModifierType)modifier.Clone();
                return modifier;
            }
            else
            {
                Log.Error("Could not find modifier of name " + modifierName);
                return null;
            }
        }

        [CanBeNull]
        public Modifier<TDataType> GetModifier<TDataType>(string modifierName)
        {
            return GetModifier(modifierName) as Modifier<TDataType>;
        }
    }
}