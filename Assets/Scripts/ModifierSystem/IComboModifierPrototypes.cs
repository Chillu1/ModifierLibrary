namespace ModifierSystem
{
    public interface IComboModifierPrototypes
    {
        ModifierPrototypesBase<ComboModifier> ModifierPrototypes { get; }
        ComboModifier GetItem(string id);
    }
}