namespace ComboSystemComposition
{
    /// <summary>
    ///     Responsible for everything timing related, interval, duration, etc
    /// </summary>
    public class TimeComponent : Component, ITimeComponent
    {
        public IEffectComponent EffectComponent { get; }
        public double Duration { get; private set; }
        private bool ResetOnFinished { get; }
        private double _timer;

        public TimeComponent(IEffectComponent effectComponent, double duration, bool resetOnFinished = false)
        {
            Duration = duration;
            ResetOnFinished = resetOnFinished;
            EffectComponent = effectComponent;
        }

        /// <summary>
        ///     Constructor for remove timer
        /// </summary>
        public TimeComponent(RemoveComponent removeComponent, double lingerDuration = 0.5d)
        {
            Duration = lingerDuration;
            ResetOnFinished = false;
            EffectComponent = removeComponent;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= Duration)
            {
                EffectComponent.Effect();
                if(ResetOnFinished)
                    _timer = 0;
            }
        }
    }
}