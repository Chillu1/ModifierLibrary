namespace ComboSystem
{
    public abstract class SingleUseDurationModifier<TDataType> : DurationModifier<TDataType> where TDataType : DurationModifierData
    {
        protected SingleUseDurationModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}