namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for everything timing related, interval, duration, etc
    /// </summary>
    public class TimeComponent : Component, ITimeComponent
    {
        private IEffectComponent EffectComponent { get; }
        private double Duration { get; set; }
        private bool ResetOnFinished { get; }
        private double _timer;
        private bool _finished;

        public TimeComponent(IEffectComponent effectComponent, double duration, bool resetOnFinished = false)
        {
            EffectComponent = effectComponent;
            Duration = duration;
            ResetOnFinished = resetOnFinished;
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
            if (_finished)
                return;
            _timer += deltaTime;
            if (_timer >= Duration)
            {
                EffectComponent.Effect();
                _finished = true;

                if(ResetOnFinished)
                {
                    _timer = 0;
                    _finished = false;
                }
            }
        }

        public void RefreshTimer()
        {
            _timer = 0;
        }
    }
}