using BaseProject;

namespace ComboSystem
{
    public abstract class ComboModifier : Modifier, IEventCopy<ComboModifier>
    {
        public ComboRecipe Recipe { get; }
        public float Cooldown { get; protected set; }//TODO? Cooldown that the modifier can be applied again?
        protected ComboModifier(string id, ComboRecipe recipe, ModifierProperties modifierProperties = default) : base(id, modifierProperties)
        {
            Recipe = recipe;
        }

        public void CopyEvents(ComboModifier prototype)
        {
            base.CopyEvents(prototype);
        }
    }

    public abstract class ComboModifier<TDataType> : ComboModifier
    {
        public TDataType Data { get; }

        protected ComboModifier(string id, TDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, recipe, modifierProperties)
        {
            Data = data;
        }
    }
}