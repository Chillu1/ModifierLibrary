namespace ComboSystem
{
    /// <summary>
    ///     Interface to use for all modifiers that do damage
    /// </summary>
    public interface IDamageData
    {
        public DamageData[] DamageData { get; }
    }
}