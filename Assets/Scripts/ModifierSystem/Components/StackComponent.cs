using System;

namespace ModifierSystem
{
    public class StackComponent : Component, IStackComponent
    {
        private int Stacks { get; set; } = 1;
        private int MaxStacks { get; set; }

        private Action StackAction { get; }

        public StackComponent(object temp, int maxStacks)
        {
            StackAction = delegate
            {
                //damageData[0].BaseDamage += amount;
                //Log.Info("1: "+_damageData[0].BaseDamage);
                //Log.Info(damageData.GetHashCode() + "_"+damageData[0].BaseDamage);
            };
            //StackAction = stackAction;
            MaxStacks = maxStacks;
        }

        public StackComponent(StackComponent prototypeStackComponent)
        {
            MaxStacks = prototypeStackComponent.MaxStacks;
        }

        public void Stack()
        {
            if (Stacks + 1 >= MaxStacks)
                return;
            //Log.Verbose($"Stacks: {Stacks}/{MaxStacks}", "modifiers");

            Stacks++;
            StackAction();
        }

        /// <summary>
        ///     Can be caused by duration, timing out, etc
        /// </summary>
        public void RemoveStack()
        {
            if (Stacks - 1 < 1)
                return;

            Stacks--;
        }

        public void ResetStacks()
        {
            Stacks = 0;
        }
    }
}