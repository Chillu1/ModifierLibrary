namespace ModifierSystem
{
    public interface IApplyComponent
    {
        bool IsConditionEvent { get; }
        void Apply();
    }
}