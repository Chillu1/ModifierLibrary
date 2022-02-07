namespace ModifierSystem
{
    public interface IComboModifierPrototypes
    {
        ModifierPrototypesBase ModifierPrototypes { get; }
        IComboModifier GetItem(string id);
    }
}