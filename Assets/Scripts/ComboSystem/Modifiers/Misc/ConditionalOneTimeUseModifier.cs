using System;

namespace ComboSystem
{
    //Better to use already defined modifiers, and just change their logic inside there, instead of having this condition thingy?
    public abstract class ConditionalOneTimeUseModifier<TDataType> : Modifier<TDataType>
    {
        public ConditionalOneTimeUseModifier(string id, TDataType data, Func<Modifier, bool> condition,
            ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
            Condition = condition;
        }

        public override void Update(float deltaTime)
        {
            if (Condition.Invoke(this))
                Apply();
        }
    }
}