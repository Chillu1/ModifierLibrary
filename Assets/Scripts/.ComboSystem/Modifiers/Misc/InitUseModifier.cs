namespace ComboSystem
{
    /// <summary>
    ///     Single use "forever" modifier
    /// </summary>
    public abstract class InitUseModifier<TDataType> : Modifier<TDataType>
    {
        protected InitUseModifier(string id, TDataType data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}