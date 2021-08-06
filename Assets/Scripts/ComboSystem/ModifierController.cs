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
            for (int index = 0; index < Modifiers.Count; index++)
            {
                var modifier = Modifiers[index];
                modifier.Update(deltaTime);
            }
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            modifier.Init();
        }

        public bool RemoveModifier(Modifier modifier)
        {
            return Modifiers.Remove(modifier);
        }
    }
}