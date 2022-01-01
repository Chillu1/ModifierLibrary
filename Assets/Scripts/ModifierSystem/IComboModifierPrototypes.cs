namespace ModifierSystem
{
    public interface IComboModifierPrototypes
    {
        ModifierPrototypesBase<IComboModifier> ModifierPrototypes { get; }
        IComboModifier GetItem(string id);
    }
}