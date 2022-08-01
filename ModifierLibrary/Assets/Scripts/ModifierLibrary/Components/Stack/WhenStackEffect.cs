namespace ModifierLibrary
{
    /// <summary>
    ///     When should the StackEffect trigger
    /// </summary>
    public enum WhenStackEffect
    {
        None = 0,
        Always = 1,
        OnXStacks = 2,
        EveryXStacks = 3,
        ZeroStacks = 4,
    }
}