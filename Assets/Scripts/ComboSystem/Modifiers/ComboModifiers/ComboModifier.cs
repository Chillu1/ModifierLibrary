namespace ComboSystem
{
    public abstract class ComboModifier : Modifier
    {
        public ComboRecipe Recipe { get; }
        protected ComboModifier(string id, ComboRecipe recipe, ModifierProperties modifierProperties = default) : base(id, modifierProperties)
        {
            Recipe = recipe;
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