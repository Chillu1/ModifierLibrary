namespace ComboSystem
{
    public abstract class SingleUseDurationComboModifier<TDataType> : DurationComboModifier<TDataType> where TDataType : DurationModifierData
    {
        protected SingleUseDurationComboModifier(string id, TDataType data, ComboRecipe recipe, ModifierProperties modifierProperties = default)
            : base(id, data, recipe, modifierProperties)
        {
        }

        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}