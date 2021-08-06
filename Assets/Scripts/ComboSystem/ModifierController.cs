using System.Collections.Generic;

namespace ComboSystem
{
    public class ModifierController
    {
        private List<Modifier> Modifiers { get; }

        public ModifierController()
        {
            Modifiers = new List<Modifier>();
        }

        public void Update(float deltaTime)
        {
            //Log.Info(Modifiers.Count);
            for (int index = 0; index < Modifiers.Count; index++)
            {
                var modifier = Modifiers[index];
                modifier.Update(deltaTime);
            }
        }

        public void AddModifier(Modifier modifier)
        {
            RegisterModifier(modifier);
            Modifiers.Add(modifier);
            modifier.Init();
            Log.Verbose("Added modifier " + modifier.GetType().Name +" with target: " + modifier.Target?.Name);
        }

        public bool RemoveModifier(Modifier modifier)
        {
            return Modifiers.Remove(modifier);
        }
        
        private void RegisterModifier(Modifier modifier)
        {
            modifier.Removed += modifierEventItem => RemoveModifier(modifierEventItem);
            modifier.Removed += modifierEventItem => Log.Verbose(modifierEventItem.GetType().Name + " removed");
        }
    }
}