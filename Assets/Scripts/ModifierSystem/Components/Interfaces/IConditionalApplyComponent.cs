namespace ModifierSystem
{
    public interface IConditionalApplyComponent : IApplyComponent
    {
        bool IsConditionEvent { get; }
    }
}