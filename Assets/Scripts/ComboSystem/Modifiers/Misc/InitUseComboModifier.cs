namespace ComboSystem
{
    /// <summary>
    ///     Single use "forever" modifier
    /// </summary>
    public abstract class InitUseComboModifier<TDataType> : ComboModifier<TDataType>
    {
        protected InitUseComboModifier(string id, TDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
            base(id, data, recipe, modifierProperties)
        {
        }

        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}