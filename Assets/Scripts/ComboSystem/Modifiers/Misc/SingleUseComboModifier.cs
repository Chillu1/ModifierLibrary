namespace ComboSystem
{
    /// <summary>
    ///     Single use "forever" modifier
    /// </summary>
    public abstract class SingleUseComboModifier<TDataType> : ComboModifier<TDataType>
    {
        protected SingleUseComboModifier(string id, TDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default) :
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