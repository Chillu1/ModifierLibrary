using System;
using BaseProject;

namespace ComboSystemComposition
{
    public abstract class ModifierPrototypesBase<TModifierType> : BasePrototypeController<string, TModifierType>
        where TModifierType : Modifier, IEventCopy<TModifierType>, ICloneable
    {
        protected abstract void SetupModifierPrototypes();

        protected void SetupModifier(TModifierType modifier)
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
    }
}