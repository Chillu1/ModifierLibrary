namespace ModifierSystem
{
    public class RefreshComponent : IRefreshComponent
    {
        private TimeComponent TimeComponent { get; }

        public RefreshComponent(TimeComponent timeComponent)
        {
            TimeComponent = timeComponent;
        }

        public void Refresh()
        {
            TimeComponent.RefreshTimer();
        }
    }
}