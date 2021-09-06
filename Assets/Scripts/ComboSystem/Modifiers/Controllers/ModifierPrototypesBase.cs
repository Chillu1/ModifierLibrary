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
        public Modifier<TDataType> GetItem<TDataType>(string modifierName)
        {
            return GetItem(modifierName) as Modifier<TDataType>;
        }
    }
}