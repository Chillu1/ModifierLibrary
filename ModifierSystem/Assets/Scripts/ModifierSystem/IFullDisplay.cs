namespace ModifierSystem
{
    /// <summary>
    ///     Full UI display text, for extra info on choices, etc.
    /// </summary>
    public interface IFullDisplay : IDisplay
    {
        string FullText();
    }
}