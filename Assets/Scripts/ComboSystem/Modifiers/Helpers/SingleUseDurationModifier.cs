namespace ComboSystem
{
    /// <summary>
    ///     Single use "forever" modifier
    /// </summary>
    public abstract class SingleUseModifier<TDataType> : Modifier<TDataType>
    {
        protected SingleUseModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        public override void Init()
        {
            Apply();
            base.Init();
        }
    }

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