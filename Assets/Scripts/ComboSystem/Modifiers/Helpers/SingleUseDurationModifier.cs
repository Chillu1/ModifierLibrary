namespace ComboSystem
{
    /// <summary>
    ///     Single use "forever" modifier
    /// </summary>
    public abstract class SingleUseModifier<TDataType> : Modifier<TDataType>
    {
        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
    public abstract class SingleUseDurationModifier<TDataType> : DurationModifier<TDataType> where TDataType : DurationModifierData
    {
        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}