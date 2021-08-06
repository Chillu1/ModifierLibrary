namespace ComboSystem
{
    public abstract class SingleUseModifier<TDataType> : Modifier<TDataType>
    {
        public override void Init()
        {
            Apply();
            base.Init();
        }
    }
}