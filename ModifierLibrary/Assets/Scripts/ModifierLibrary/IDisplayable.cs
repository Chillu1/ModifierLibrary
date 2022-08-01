namespace ModifierLibrary
{
    /// <summary>
    ///     Displays UI info
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        ///     Tooltip info
        /// </summary>
        string GetBasicInfo();

        /// <summary>
        ///     Info for battle UI
        /// </summary>
        string GetBattleInfo() => GetBasicInfo();//TODO Rename, to not always battle ui, but also map ui, etc?
        
        /// <summary>
        ///     Info for choice/wiki UI
        /// </summary>
        string GetFullInfo() => GetBasicInfo();
    }
}