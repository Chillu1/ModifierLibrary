using System;
using System.Collections.Generic;
using System.Linq;

namespace ComboSystem
{
    public class ModifierController
    {
        private Dictionary<string, Modifier> Modifiers { get; }

        public ModifierController()
        {
            Modifiers = new Dictionary<string, Modifier>();
        }

        public void Update(float deltaTime)
        {
            //Log.Info(Modifiers.Count);
            foreach (var valuePair in Modifiers.ToArray())//TODO Making it into an array every frame is uncool
            {
                valuePair.Value.Update(deltaTime);
            }
        }

        public void TryAddModifier(Modifier modifier)
        {
            if (HasModifier(modifier))
            {
                //var test = Modifiers.Find()
                Log.Info("HasModifier " + modifier.GetType().Name);
                switch (modifier.ModifierProperties)
                {
                    case ModifierProperties.None:
                        return;
                    case ModifierProperties.Stackable:
                        modifier.Stack();
                        break;
                    case ModifierProperties.Refreshable:
                        modifier.Refresh();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
                AddModifier(modifier);
        }

        public void AddModifier(Modifier modifier)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier.Id, modifier);
            modifier.Init();
            //Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.Target?.Name);
        }

        public bool RemoveModifier(Modifier modifier)
        {
            return Modifiers.Remove(modifier.Id);
        }

        public bool HasModifier(Modifier modifier)
        {
            return Modifiers.ContainsKey(modifier.Id);
            //return Modifiers.All(internalModifier => internalModifier.Id == modifier.Id && internalModifier.GetType() == modifier.GetType());
        }

        public IEnumerable<ModifierApplier<ModifierApplierData>> GetModifierAppliers()
        {
            return (IEnumerable<ModifierApplier<ModifierApplierData>>)Modifiers.Values.Where(mod =>
                mod.GetType() == typeof(ModifierApplier<ModifierApplierData>));
        }

        private void RegisterModifier(Modifier modifier)
        {
            modifier.Removed += modifierEventItem => RemoveModifier(modifierEventItem);
            modifier.Removed += modifierEventItem => Log.Verbose(modifierEventItem.GetType().Name + " removed");
        }
    }
}