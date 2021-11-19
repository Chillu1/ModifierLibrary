using System;

namespace ComboSystemComposition
{
    public class StackComponent : Component, IStackComponent
    {
        private int Stacks { get; set; }
        private int MaxStacks { get; set; }

        private Action StackAction { get; }

        public StackComponent(Action stackAction, int maxStacks)
        {
            MaxStacks = maxStacks;
            StackAction = stackAction;
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
            if (Stacks + -1 < 0)
                return;

            Stacks--;
        }

        public void ResetStacks()
        {
            Stacks = 0;
        }
    }
}