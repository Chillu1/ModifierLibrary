using System;

namespace ComboSystem
{
    //TODO, make one for generic conditions, OnDeath, OnKill, etc
    public abstract class ConditionalOneTimeUseModifier<TDataType> : Modifier<TDataType>
    {
        public ConditionalOneTimeUseModifier(string id, TDataType data, Func<Modifier, bool> condition, ModifierProperties modifierProperties = default)
            : base(id, data, modifierProperties)
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