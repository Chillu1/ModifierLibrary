using System.Collections.Generic;

namespace ComboSystemTest
{
    public class ComboSystemController
    {
        private readonly List<Modifier> _modifiers;

        public ComboSystemController(List<Modifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public void Init()
        {
        }

        public void Update(float deltaTime)
        {
            foreach (var modifier in _modifiers)
            {
                modifier.Update(deltaTime);
            }
        }
    }
}